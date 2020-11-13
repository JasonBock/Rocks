using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders
{
	internal static class MockPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents)
		{
			writer.WriteLine("get");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("var methodHandler = methodHandlers[0];");
			writer.WriteLine("var result = methodHandler.Method is not null ?");
			writer.Indent++;
			var methodCast = $"(Func<{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)";
			writer.WriteLine($"({methodCast}methodHandler.Method)() :");
			writer.WriteLine($"((HandlerInformation<{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler).ReturnValue;");
			writer.Indent--;

			if (raiseEvents)
			{
				writer.WriteLine("methodHandler.RaiseEvents(this);");
			}

			writer.WriteLine("methodHandler.IncrementCallCount();");
			writer.WriteLine("return result!;");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($@"throw new ExpectationException(""No handlers were found for get_{result.Value.Name}())"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents)
		{
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

			writer.WriteLine($"if (((Arg<{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[0]).IsValid(value))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foundMatch = true;");
			writer.WriteLine();
			writer.WriteLine("if (methodHandler.Method is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"((Action<{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Method)(value);");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			writer.WriteLine("if (!foundMatch)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for set_{result.Value.Name}({{value}})"");");
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
			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for set_{result.Value.Name}({{value}})"");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, bool raiseEvents)
		{
			var attributes = result.Value.GetAttributes();

			if(attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var memberIdentifierAttribute = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""get_{result.Value.Name}()"")]");
				memberIdentifierAttribute++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""set_{result.Value.Name}(value)"")]");
			}

			writer.WriteLine($"public {(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {result.Value.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			var memberIdentifier = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockPropertyBuilder.BuildGetter(writer, result, memberIdentifier, raiseEvents);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockPropertyBuilder.BuildSetter(writer, result, memberIdentifier, raiseEvents);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}