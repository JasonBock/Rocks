using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create
{
	internal static class MockPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName)
		{
			writer.WriteLine("get");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("var methodHandler = methodHandlers[0];");

			var property = result.Value;
			if (property.ReturnsByRef || property.ReturnsByRefReadonly)
			{
				writer.WriteLine($"this.rr{result.MemberIdentifier} = methodHandler.Method is not null ?");
			}
			else
			{
				writer.WriteLine("var result = methodHandler.Method is not null ?");
			}

			writer.Indent++;

			var methodCast = property.GetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.GetMethod!) :
				DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
			var handlerName = property.Type.IsEsoteric() ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(property.Type) :
				$"HandlerInformation<{property.Type.GetName()}>";

			writer.WriteLine($"(({methodCast})methodHandler.Method)() :");
			writer.WriteLine($"(({handlerName})methodHandler).ReturnValue;");
			writer.Indent--;

			if (raiseEvents)
			{
				writer.WriteLine("methodHandler.RaiseEvents(this);");
			}

			writer.WriteLine("methodHandler.IncrementCallCount();");

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

			writer.WriteLine();

			writer.WriteLine($"throw new ExpectationException(\"No handlers were found for {explicitTypeName}get_{property.Name}())\");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName)
		{
			var property = result.Value;

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

			var argType = property.Type.IsPointer() ? PointerArgTypeBuilder.GetProjectedName(property.Type) :
				property.Type.IsRefLikeType ? RefLikeArgTypeBuilder.GetProjectedName(property.Type) :
				$"{nameof(Argument)}<{property.Type.GetName()}>";

			writer.WriteLine($"if ((methodHandler.Expectations[0] as {argType})?.IsValid(value) ?? false)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foundMatch = true;");
			writer.WriteLine();
			writer.WriteLine("if (methodHandler.Method is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			var methodCast = property.SetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.SetMethod!) :
				DelegateBuilder.Build(property.SetMethod!.Parameters);

			writer.WriteLine($"(({methodCast})methodHandler.Method)(value);");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			writer.WriteLine("if (!foundMatch)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"throw new ExpectationException(\"No handlers were found for {explicitTypeName}set_{property.Name}(value)\");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			if (raiseEvents)
			{
				writer.WriteLine("methodHandler.RaiseEvents(this);");
			}

			writer.WriteLine("methodHandler.IncrementCallCount();");
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
			writer.WriteLine($"throw new ExpectationException(\"No handlers were found for {explicitTypeName}set_{property.Name}(value)\");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, bool raiseEvents,
			Compilation compilation)
		{
			var property = result.Value;
			var attributes = property.GetAttributes();

			if(attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var memberIdentifierAttribute = result.MemberIdentifier;
			var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				string.Empty : $"{property.ContainingType.GetName(TypeNameOption.NoGenerics)}.";

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}get_{property.Name}()"")]");
				memberIdentifierAttribute++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}set_{property.Name}(value)"")]");
			}

			var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
			var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;

			var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetName()} {explicitTypeName}{property.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			var memberIdentifier = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockPropertyBuilder.BuildGetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockPropertyBuilder.BuildSetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}