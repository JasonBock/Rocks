using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

// TODO: We should be using VariableNamingContext
// for things like methodHandlers, methodHandler, etc.
internal static class MockPropertyBuilder
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

		writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var @methodHandlers))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("var @methodHandler = @methodHandlers[0];");
		writer.WriteLine("@methodHandler.IncrementCallCount();");

		if (property.ReturnsByRef || property.ReturnsByRefReadOnly)
		{
			writer.WriteLine($"this.rr{property.MemberIdentifier} = @methodHandler.Method is not null ?");
		}
		else
		{
			writer.WriteLine("var @result = @methodHandler.Method is not null ?");
		}

		writer.Indent++;

		var methodCast = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(ImmutableArray<ParameterModel>.Empty, property.Type);
		var propertyReturnType = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			propertyGetMethod.ReturnType.FullyQualifiedName;
		var handlerName = property.Type.IsPointer ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(property.Type, property.MockType) :
			$"global::Rocks.HandlerInformation<{propertyReturnType}>";

		writer.WriteLine($"(({methodCast})@methodHandler.Method)() :");
		if (propertyGetMethod.ReturnType.IsPointer || !propertyGetMethod.ReturnType.IsRefLikeType)
		{
			writer.WriteLine($"(({handlerName})@methodHandler).ReturnValue;");
		}
		else
		{
			writer.WriteLine($"(({handlerName})@methodHandler).ReturnValue!.Invoke();");
		}
		writer.Indent--;

		if (raiseEvents)
		{
			writer.WriteLine("@methodHandler.RaiseEvents(this);");
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

		writer.WriteLine($"{visibility}{accessor}");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var @methodHandlers))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("var @foundMatch = false;");
		writer.WriteLine("foreach (var @methodHandler in @methodHandlers)");
		writer.WriteLine("{");
		writer.Indent++;

		var argType = property.Type.IsPointer ?
			PointerArgTypeBuilder.GetProjectedFullyQualifiedName(property.Type, property.MockType) :
				property.Type.IsRefLikeType ?
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(property.Type, property.MockType) :
					$"global::Rocks.Argument<{property.Type.FullyQualifiedName}>";

		writer.WriteLine($"if ((({argType})@methodHandler.Expectations[0]).IsValid(value!))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("@methodHandler.IncrementCallCount();");
		writer.WriteLine("@foundMatch = true;");
		writer.WriteLine();
		writer.WriteLine("if (@methodHandler.Method is not null)");
		writer.WriteLine("{");
		writer.Indent++;

		var methodCast = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);

		writer.WriteLine($"(({methodCast})@methodHandler.Method)(value!);");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		writer.WriteLine("if (!@foundMatch)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {explicitTypeName}{methodName}(value)\");");
		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		if (raiseEvents)
		{
			writer.WriteLine("@methodHandler.RaiseEvents(this);");
		}

		writer.WriteLine("break;");

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine("else");
		writer.WriteLine("{");
		writer.Indent++;

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
			MockPropertyBuilder.BuildGetter(writer, property, visibility, memberIdentifier, raiseEvents, explicitTypeName);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
			MockPropertyBuilder.BuildSetter(writer, property, visibility, memberIdentifier, raiseEvents, explicitTypeName);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}