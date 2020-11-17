using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockIndexerBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents)
		{
			var method = result.Value.GetMethod!;
			var methodException =
				$"{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} this[{string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))}";

			writer.WriteLine("get");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if (i == 0)
				{
					writer.WriteLine(
						$"if (((Arg<{parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if (i == 1)
					{
						writer.Indent++;
					}

					writer.WriteLine(
						$"((Arg<{parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

					if (i == method.Parameters.Length - 1)
					{
						writer.Indent--;
					}
				}
			}

			writer.WriteLine("{");
			writer.Indent++;

			MockMethodValueBuilder.BuildMethodHandler(writer, method, raiseEvents);
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents)
		{
			var method = result.Value.SetMethod!;
			var methodException =
				$"{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} this[{string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))}";

			writer.WriteLine("set");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if (i == 0)
				{
					writer.WriteLine(
						$"if (((Arg<{parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if (i == 1)
					{
						writer.Indent++;
					}

					writer.WriteLine(
						$"((Arg<{parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

					if (i == method.Parameters.Length - 1)
					{
						writer.Indent--;
					}
				}
			}

			writer.WriteLine("{");
			writer.Indent++;

			MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
			writer.WriteLine("return;");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, bool raiseEvents)
		{
			var attributes = result.Value.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var memberIdentifierAttribute = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{MockIndexerBuilder.GetSignature(result.Value.GetMethod!.Parameters)}"")]");
				memberIdentifierAttribute++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{MockIndexerBuilder.GetSignature(result.Value.SetMethod!.Parameters)}"")]");
			}

			var indexerSignature = MockIndexerBuilder.GetSignature(result.Value.Parameters);
			writer.WriteLine($"public {(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{result.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {indexerSignature}");
			writer.WriteLine("{");
			writer.Indent++;

			var memberIdentifier = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockIndexerBuilder.BuildGetter(writer, result, memberIdentifier, raiseEvents);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockIndexerBuilder.BuildSetter(writer, result, memberIdentifier, raiseEvents);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static string GetSignature(ImmutableArray<IParameterSymbol> parameters)
		{
			var methodParameters = string.Join(", ", parameters.Select(_ =>
			{
				var parameter = $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));
			
			return $"this[{string.Join(", ", methodParameters)}]";
		}
	}
}