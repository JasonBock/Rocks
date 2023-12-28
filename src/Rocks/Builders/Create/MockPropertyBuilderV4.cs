using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

// TODO: We should be using VariableNamingContext
// for things like methodHandlers, methodHandler, etc.
internal static class MockPropertyBuilderV4
{
	private static void BuildGetter(IndentedTextWriter writer,
		PropertyModel property, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents, string explicitTypeName)
	{
		var propertyGetMethod = property.GetMethod!;
		var methodName = propertyGetMethod.Name;
		var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{propertyGetMethod.OverridingCodeValue} " : string.Empty;
		var visibility = methodVisibility != propertyVisibility ?
			methodVisibility : string.Empty;

		writer.WriteLine($"{visibility}get");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.expectations.handlers{memberIdentifier}.Count > 0)");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"var @handler = this.expectations.handlers{memberIdentifier}[0];");
		writer.WriteLine("@handler.CallCount++;");

		if (property.ReturnsByRef || property.ReturnsByRefReadOnly)
		{
			writer.WriteLine($"this.rr{property.MemberIdentifier} = @handler.Callback?.Invoke() ?? @handler.ReturnValue;");
		}
		else
		{
			writer.WriteLine("var @result = @handler.Callback?.Invoke() ?? @handler.ReturnValue;");
		}

		// TODO: I took a lot out here because sometimes I apparently pass in 
		// a delegate for the return value for certain return types. But...
		// I'm thinking I don't need to do that anymore? We'll see.

		if (raiseEvents)
		{
			writer.WriteLine("@handler.RaiseEvents(this);");
		}

		if (property.ReturnsByRef || property.ReturnsByRefReadOnly)
		{
			writer.WriteLine($"return ref this.rr{property.MemberIdentifier};");
		}
		else
		{
			writer.WriteLine("return @result!;");
		}

		writer.Indent--;
		writer.WriteLine("}");

		if (!property.IsAbstract)
		{
			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;

			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			var refReturn = property.ReturnsByRef || property.ReturnsByRefReadOnly ? "ref " : string.Empty;
			var target = property.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{property.ContainingType.FlattenedName}" : "base";
			writer.WriteLine($"return {refReturn}{target}.{property.Name};");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {explicitTypeName}{methodName}())\");");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer,
		PropertyModel property, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents, string explicitTypeName)
	{
		var methodName = property.SetMethod!.Name;
		var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{property.SetMethod!.OverridingCodeValue} " : string.Empty;
		var visibility = methodVisibility != propertyVisibility ?
			methodVisibility : string.Empty;
		var accessor = property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit ?
			"init" : "set";

		writer.WriteLines(
			$$"""
			{{visibility}}{{accessor}}
			{
				if (this.expectations.handlers{{memberIdentifier}}.Count > 0)
				{
					var @foundMatch = false;
					foreach (var @handler in this.expectations.handlers{{memberIdentifier}})
					{
						if (@handler.value.IsValid(value!))
						{
							@handler.CallCount++;
							@foundMatch = true;
							@handler.Callback?.Invoke(value!);
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for {{explicitTypeName}}{{methodName}}(value)");
							}
							
			""");

		writer.Indent += 4;
		
		if (raiseEvents)
		{
			writer.WriteLine("@handler.RaiseEvents(this);");
		}

		writer.Indent -= 4;

		writer.WriteLines(
			"""
							break;
						}
					}
				}
				else
				{
			""");

		writer.Indent += 2;

		if (!property.IsAbstract)
		{
			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			var target = property.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{property.ContainingType.FlattenedName}" : "base";
			writer.WriteLine($"{target}.{property.Name} = value!;");
		}
		else
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {explicitTypeName}{methodName}(value)\");");
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer,
		PropertyModel property, bool raiseEvents)
	{
		var isGetterVisible = false;
		var isSetterVisible = false;

		if (property.AllAttributesDescription.Length > 0)
		{
			writer.WriteLine(property.AllAttributesDescription);
		}

		var memberIdentifierAttribute = property.MemberIdentifier;
		var explicitTypeName = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.FullyQualifiedName}.";

		if (property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet ||
			property.Accessors == PropertyAccessor.GetAndInit)
		{
			isGetterVisible = property.GetCanBeSeenByContainingAssembly;

			if (isGetterVisible)
			{
				writer.WriteLine($$"""[global::Rocks.MemberIdentifier({{memberIdentifierAttribute}}, "{{explicitTypeName}}{{property.GetMethod!.Name}}()")]""");
				memberIdentifierAttribute++;
			}
		}

		if (property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ||
			property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit)
		{
			isSetterVisible = property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly;

			if (isSetterVisible)
			{
				writer.WriteLine($$"""[global::Rocks.MemberIdentifier({{memberIdentifierAttribute}}, "{{explicitTypeName}}{{property.SetMethod!.Name}}(value)")]""");
			}
		}

		var visibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{property.OverridingCodeValue} " : string.Empty;
		var isOverriden = property.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var isUnsafe = property.IsUnsafe ? "unsafe " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.FullyQualifiedName} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		var memberIdentifier = property.MemberIdentifier;

		if (isGetterVisible)
		{
			MockPropertyBuilderV4.BuildGetter(writer, property, visibility, memberIdentifier, raiseEvents, explicitTypeName);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
			MockPropertyBuilderV4.BuildSetter(writer, property, visibility, memberIdentifier, raiseEvents, explicitTypeName);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}