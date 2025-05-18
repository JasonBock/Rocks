using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Shim;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, TypeMockModel type,
		PropertyModel property, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents)
	{
		var propertyGetMethod = property.GetMethod!;
		var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{propertyGetMethod.OverridingCodeValue} " : string.Empty;
		var visibility = methodVisibility != propertyVisibility ?
			methodVisibility : string.Empty;

		writer.WriteLine($"{visibility}get");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.{type.ExpectationsPropertyName}.handlers{memberIdentifier} is not null)");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"var @handler = this.{type.ExpectationsPropertyName}.handlers{memberIdentifier}.First;");
		writer.WriteLine("@handler.CallCount++;");

		var returnValueCall = property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
			".ReturnValue!()" : ".ReturnValue";

		if (property.ReturnsByRef || property.ReturnsByRefReadOnly)
		{
			writer.WriteLines(
				$$"""
				this.rr{{property.MemberIdentifier}} = @handler.Callback is not null ?
					@handler.Callback() : @handler{{returnValueCall}};
				""");
		}
		else
		{
			writer.WriteLines(
				$$"""
				var @result = @handler.Callback is not null ?
					@handler.Callback() : @handler{{returnValueCall}};
				""");
		}

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
				$"this.shimFor{ShimBuilder.GetShimName(property.ContainingType)}" : "base";
			writer.WriteLine($"return {refReturn}{target}.{property.Name};");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			ExpectationExceptionBuilder.Build(
				writer, propertyGetMethod, "No handlers match for", memberIdentifier);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, TypeMockModel type,
		PropertyModel property, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents)
	{
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
				if (this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}} is not null)
				{
					var @foundMatch = false;
					foreach (var @handler in this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}})
					{
						if (@handler.value.IsValid(value!))
						{
							@handler.CallCount++;
							@foundMatch = true;
							@handler.Callback?.Invoke(value!);
							
							if (!@foundMatch)
							{
			""");

		writer.Indent += 5;
		ExpectationExceptionBuilder.Build(
			writer, property.SetMethod!, "No handlers match for", memberIdentifier);
		writer.Indent -= 5;

		writer.WriteLines(
			"""
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
				$"this.shimFor{ShimBuilder.GetShimName(property.ContainingType)}" : "base";
			writer.WriteLine($"{target}.{property.Name} = value!;");
		}
		else
		{
			ExpectationExceptionBuilder.Build(
				writer, property.SetMethod!, "No handlers were found for", memberIdentifier);
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type,
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
				writer.WriteLine($"[global::Rocks.MemberIdentifier({memberIdentifierAttribute}, global::Rocks.PropertyAccessor.Get)]");
				memberIdentifierAttribute++;
			}
		}

		if (property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ||
			property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit)
		{
			isSetterVisible = property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly;

			if (isSetterVisible)
			{
				writer.WriteLine($"[global::Rocks.MemberIdentifier({memberIdentifierAttribute}, global::Rocks.PropertyAccessor.Set)]");
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
			MockPropertyBuilder.BuildGetter(writer, type, property, visibility, memberIdentifier, raiseEvents);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
			MockPropertyBuilder.BuildSetter(writer, type, property, visibility, memberIdentifier, raiseEvents);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}