using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockMethodValueBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result, bool raiseEvents)
		{
			var method = result.Value;
			var returnType = method.ReturnType.GetName();
			var parametersDescription = string.Join(", ", method.Parameters.Select(
				_ => $"{_.Type.GetName()} {_.Name}"));
			var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
				$"{method.ContainingType.GetName(TypeNameOption.NoGenerics)}." : string.Empty;
			var methodDescription = $"{returnType} {explicitTypeNameDescription}{method.GetName()}({parametersDescription})";
			
			var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				var parameter = $"{_.Type.GetName()} {_.Name}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));

			var methodSignature =
				$"{returnType} {explicitTypeNameDescription}{method.GetName()}({methodParameters})";
			var methodException =
				$"{returnType} {explicitTypeNameDescription}{method.GetName()}({string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))})";

			var attributes = method.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var returnAttributes = method.GetReturnTypeAttributes();

			if (returnAttributes.Length > 0)
			{
				writer.WriteLine(returnAttributes.GetDescription(AttributeTargets.ReturnValue));
			}

			writer.WriteLine($@"[MemberIdentifier({result.MemberIdentifier}, ""{methodDescription}"")]");
			var isPublic = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			writer.WriteLine($"{isPublic}{(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

			var constraints = method.GetConstraints();

			if(result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
				result.RequiresOverride == RequiresOverride.Yes)
			{
				constraints = constraints.AddRange(method.GetDefaultConstraints());
			}

			if(constraints.Length > 0)
			{
				writer.Indent++;

				foreach(var constraint in constraints)
				{
					writer.WriteLine(constraint);
				}

				writer.Indent--;
			}

			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({result.MemberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			if (method.Parameters.Length > 0)
			{
				MockMethodValueBuilder.BuildMethodValidationHandlerWithParameters(writer, method, raiseEvents);
			}
			else
			{
				MockMethodValueBuilder.BuildMethodValidationHandlerNoParameters(writer, method, raiseEvents);
			}

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
			writer.WriteLine($@"throw new ExpectationException({(method.ReturnsVoid ? string.Empty : "$")}""No handlers were found for {methodException})"");");

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();
		}

		internal static void BuildMethodHandler(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
		{
			writer.WriteLine("var result = methodHandler.Method is not null ?");
			writer.Indent++;

			var methodCast = DelegateBuilder.Build(method.Parameters, method.ReturnType);
			var methodArguments = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ => _.Name));
			writer.WriteLine($"(({methodCast})methodHandler.Method)({methodArguments}) :");
			writer.WriteLine($"((HandlerInformation<{method.ReturnType.GetName()}>)methodHandler).ReturnValue;");

			writer.Indent--;

			if(raiseEvents)
			{
				writer.WriteLine("methodHandler.RaiseEvents(this);");
			}

			writer.WriteLine("methodHandler.IncrementCallCount();");
			writer.WriteLine("return result!;");
		}

		private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
		{
			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if (i == 0)
				{
					writer.WriteLine(
						$"if (((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if (i == 1)
					{
						writer.Indent++;
					}

					writer.WriteLine(
						$"((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

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
		}

		private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
		{
			writer.WriteLine("var methodHandler = methodHandlers[0];");
			MockMethodValueBuilder.BuildMethodHandler(writer, method, raiseEvents);
		}
	}
}