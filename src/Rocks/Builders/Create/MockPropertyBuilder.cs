using Microsoft.CodeAnalysis;
using Rocks.Exceptions;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer,
		PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;

		var methodName = propertyGetMethod.Name;
		writer.WriteLine("get");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine("var methodHandler = methodHandlers[0];");

		if (property.ReturnsByRef || property.ReturnsByRefReadonly)
		{
			writer.WriteLine($"this.rr{result.MemberIdentifier} = methodHandler.Method is not null ?");
		}
		else
		{
			writer.WriteLine("var result = methodHandler.Method is not null ?");
		}

		writer.Indent++;

		var methodCast = propertyGetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(propertyGetMethod) :
			DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
		var propertyReturnType = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(propertyGetMethod) : propertyGetMethod.ReturnType.GetReferenceableName();
		var handlerName = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(property.Type) :
			$"{nameof(HandlerInformation)}<{propertyReturnType}>";

		writer.WriteLine($"(({methodCast})methodHandler.Method)() :");
		if (propertyGetMethod.ReturnType.IsPointer() || !propertyGetMethod.ReturnType.IsRefLikeType)
		{
			writer.WriteLine($"(({handlerName})methodHandler).ReturnValue;");
		}
		else
		{
			writer.WriteLine($"(({handlerName})methodHandler).ReturnValue!.Invoke();");
		}
		writer.Indent--;

		if (raiseEvents)
		{
			writer.WriteLine($"methodHandler.{nameof(HandlerInformation.RaiseEvents)}(this);");
		}

		writer.WriteLine($"methodHandler.{nameof(HandlerInformation.IncrementCallCount)}();");

		if (property.ReturnsByRef || property.ReturnsByRefReadonly)
		{
			writer.WriteLine($"return ref this.rr{result.MemberIdentifier};");
		}
		else
		{
			writer.WriteLine("return result!;");
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
			writer.WriteLine($"throw new {nameof(ExpectationException)}(\"No handlers were found for {explicitTypeName}{methodName}())\");");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer,
		PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName, bool allowNull)
	{
		var methodName = result.Value.SetMethod!.Name;
		var property = result.Value;

		if (result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit)
		{
			writer.WriteLine("init { }");
		}
		else
		{
			var nullableFlag = allowNull ? "!" : string.Empty;
			writer.WriteLine("set");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("var foundMatch = false;");
			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			var argType = property.Type.IsPointer() ?
				PointerArgTypeBuilder.GetProjectedName(property.Type) :
					property.Type.IsRefLikeType ?
						RefLikeArgTypeBuilder.GetProjectedName(property.Type) :
						$"{nameof(Argument)}<{property.Type.GetName()}>";

			writer.WriteLine($"if ((methodHandler.Expectations[0] as {argType})?.IsValid(value{nullableFlag}) ?? false)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foundMatch = true;");
			writer.WriteLine();
			writer.WriteLine("if (methodHandler.Method is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			var methodCast = property.SetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(property.SetMethod!) :
				DelegateBuilder.Build(property.SetMethod!.Parameters);

			writer.WriteLine($"(({methodCast})methodHandler.Method)(value{nullableFlag});");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			writer.WriteLine("if (!foundMatch)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"throw new {nameof(ExpectationException)}(\"No handlers match for {explicitTypeName}{methodName}(value)\");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			if (raiseEvents)
			{
				writer.WriteLine($"methodHandler.{nameof(HandlerInformation.RaiseEvents)}(this);");
			}

			writer.WriteLine($"methodHandler.{nameof(HandlerInformation.IncrementCallCount)}();");
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
				writer.WriteLine($"{target}.{property.Name} = value;");
			}
			else
			{
				writer.WriteLine($"throw new {nameof(ExpectationException)}(\"No handlers were found for {explicitTypeName}{methodName}(value)\");");
			}

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	internal static void Build(IndentedTextWriter writer, 
		PropertyMockableResult result, bool raiseEvents, Compilation compilation)
	{
		var property = result.Value;
		var attributes = property.GetAllAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var memberIdentifierAttribute = result.MemberIdentifier;
		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.GetName(TypeNameOption.IncludeGenerics)}.";

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{property.GetMethod!.Name}()"")]");
			memberIdentifierAttribute++;
		}

		if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit)
		{
			writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{property.SetMethod!.Name}(value)"")]");
		}

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetName()} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		var memberIdentifier = result.MemberIdentifier;

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			MockPropertyBuilder.BuildGetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName);
			memberIdentifier++;
		}

		if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit)
		{
			var allowNullAttributeType = compilation.GetTypeByMetadataName("System.Diagnostics.CodeAnalysis.AllowNullAttribute");
			var allowNull = attributes.Any(_ => _.AttributeClass?.Equals(allowNullAttributeType, SymbolEqualityComparer.Default) ?? false);
			MockPropertyBuilder.BuildSetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName, allowNull);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}