using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

// TODO: We should be using VariableNamingContext
// for things like methodHandlers, methodHandler, etc.
internal static class MockPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer,
		PropertyMockableResult result, Compilation compilation, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents, string explicitTypeName)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;
		var methodName = propertyGetMethod.Name;
		var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
			$"{propertyGetMethod.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
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

		if (property.ReturnsByRef || property.ReturnsByRefReadonly)
		{
			writer.WriteLine($"this.rr{result.MemberIdentifier} = @methodHandler.Method is not null ?");
		}
		else
		{
			writer.WriteLine("var @result = @methodHandler.Method is not null ?");
		}

		writer.Indent++;

		var methodCast = propertyGetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
			DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
		var propertyReturnType = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
			propertyGetMethod.ReturnType.GetFullyQualifiedName();
		var handlerName = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(property.Type, result.MockType) :
			$"global::Rocks.HandlerInformation<{propertyReturnType}>";

		writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{methodCast}>(@methodHandler.Method)() :");
		if (propertyGetMethod.ReturnType.IsPointer() || !propertyGetMethod.ReturnType.IsRefLikeType)
		{
			writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{handlerName}>(@methodHandler).ReturnValue;");
		}
		else
		{
			writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{handlerName}>(@methodHandler).ReturnValue!.Invoke();");
		}
		writer.Indent--;

		if (raiseEvents)
		{
			writer.WriteLine("@methodHandler.RaiseEvents(this);");
		}

		if (property.ReturnsByRef || property.ReturnsByRefReadonly)
		{
			writer.WriteLine($"return ref this.rr{result.MemberIdentifier};");
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
			var refReturn = property.ReturnsByRef || property.ReturnsByRefReadonly ? "ref " : string.Empty;
			var target = property.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{property.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
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
		PropertyMockableResult result, Compilation compilation, string propertyVisibility,
		uint memberIdentifier, bool raiseEvents, string explicitTypeName, bool allowNull)
	{
		var propertySetMethod = result.Value.SetMethod!;
		var methodName = propertySetMethod.Name;
		var property = result.Value;
		var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
			$"{propertySetMethod.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var visibility = methodVisibility != propertyVisibility ?
			methodVisibility : string.Empty;
		var accessor = result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit ?
			"init" : "set";

		var nullableFlag = allowNull ? "!" : string.Empty;
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

		var argType = property.Type.IsPointer() ?
			PointerArgTypeBuilder.GetProjectedFullyQualifiedName(property.Type, result.MockType) :
				property.Type.IsRefLikeType ?
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(property.Type, result.MockType) :
					$"global::Rocks.Argument<{property.Type.GetFullyQualifiedName()}>";

		writer.WriteLine($"if (global::System.Runtime.CompilerServices.Unsafe.As<{argType}>(@methodHandler.Expectations[0]).IsValid(@value{nullableFlag}))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("@methodHandler.IncrementCallCount();");
		writer.WriteLine("@foundMatch = true;");
		writer.WriteLine();
		writer.WriteLine("if (@methodHandler.Method is not null)");
		writer.WriteLine("{");
		writer.Indent++;

		var methodCast = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, result.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);

		writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{methodCast}>(@methodHandler.Method)(@value{nullableFlag});");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		writer.WriteLine("if (!@foundMatch)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {explicitTypeName}{methodName}(@value)\");");
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
				$"this.shimFor{property.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
			writer.WriteLine($"{target}.{property.Name} = @value;");
		}
		else
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {explicitTypeName}{methodName}(@value)\");");
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer,
		PropertyMockableResult result, bool raiseEvents, Compilation compilation)
	{
		var property = result.Value;
		var attributes = property.GetAllAttributes();
		var isGetterVisible = false;
		var isSetterVisible = false;

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var memberIdentifierAttribute = result.MemberIdentifier;
		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.GetFullyQualifiedName()}.";

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			isGetterVisible = result.Value.GetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly);

			if (isGetterVisible)
			{
				writer.WriteLine($@"[global::Rocks.MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{property.GetMethod!.Name}()"")]");
				memberIdentifierAttribute++;
			}
		}

		if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit)
		{
			isSetterVisible = result.Value.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly);

			if (isSetterVisible)
			{
				writer.WriteLine($@"[global::Rocks.MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{property.SetMethod!.Name}(@value)"")]");
			}
		}

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetFullyQualifiedName()} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		var memberIdentifier = result.MemberIdentifier;

		if (isGetterVisible)
		{
			MockPropertyBuilder.BuildGetter(writer, result, compilation, visibility, memberIdentifier, raiseEvents, explicitTypeName);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
			var allowNullAttributeType = compilation.GetTypeByMetadataName("System.Diagnostics.CodeAnalysis.AllowNullAttribute");
			var allowNull = attributes.Any(_ => _.AttributeClass?.Equals(allowNullAttributeType, SymbolEqualityComparer.Default) ?? false);
			MockPropertyBuilder.BuildSetter(writer, result, compilation, visibility, memberIdentifier, raiseEvents, explicitTypeName, allowNull);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}