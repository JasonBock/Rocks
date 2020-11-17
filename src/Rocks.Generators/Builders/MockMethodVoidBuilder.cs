using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockMethodVoidBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result, bool raiseEvents)
		{
			var method = result.Value;
			var methodDescription =
				$"void {method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}"))})";
			var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				var parameter = $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));
			var methodSignature =
				$"void {method.Name}({methodParameters})";
			var methodException =
				$"void {method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))})";

			var attributes = method.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			writer.WriteLine($@"[MemberIdentifier({result.MemberIdentifier}, ""{methodDescription}"")]");
			writer.WriteLine($"public {(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({result.MemberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			if (method.Parameters.Length > 0)
			{
				MockMethodVoidBuilder.BuildMethodValidationHandlerWithParameters(writer, method, methodException, raiseEvents);
			}
			else
			{
				MockMethodVoidBuilder.BuildMethodValidationHandlerNoParameters(writer, method, raiseEvents);
			}

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($@"throw new ExpectationException({(method.ReturnsVoid ? string.Empty : "$")}""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
		}

		private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
		{
			writer.WriteLine("var methodHandler = methodHandlers[0];");
			MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
		}

		private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, IMethodSymbol method,
			string methodException, bool raiseEvents)
		{
			writer.WriteLine("var foundMatch = false;");
			writer.WriteLine();
			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if(i == 0)
				{
					writer.WriteLine(
						$"if (((Arg<{parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>)methodHandler.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if(i == 1)
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
			writer.WriteLine("foundMatch = true;");
			writer.WriteLine();
			MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
			writer.WriteLine("break;");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			writer.WriteLine("if (!foundMatch)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void BuildMethodHandler(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
		{
			writer.WriteLine("if (methodHandler.Method is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			var methodCast = method.Parameters.Length == 0 ? "(Action)" :
				$"(Action<{string.Join(", ", method.Parameters.Select(_ => _.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)))}>)";
			var methodArguments = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ => _.Name));
			writer.WriteLine($"({methodCast}methodHandler.Method)({methodArguments});");

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();

			if (raiseEvents)
			{
				writer.WriteLine("methodHandler.RaiseEvents(this);");
			}

			writer.WriteLine("methodHandler.IncrementCallCount();");
		}
	}
}