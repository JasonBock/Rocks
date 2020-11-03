using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	/*
	
	Parameters:
			[MemberIdentifier(0, "Foo(int a)")]
			public void Foo(int a)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var foundMatch = false;

					foreach (var methodHandler in methodHandlers)
					{
						if (((Arg<int>)methodHandler.Expectations[0]).IsValid(a))
						{
							foundMatch = true;

							if (methodHandler.Method != null)
							{
#pragma warning disable CS8604
								((Action<int>)methodHandler.Method)(a);
#pragma warning restore CS8604
							}

							methodHandler.IncrementCallCount();
							break;
						}
					}

					if (!foundMatch)
					{
						throw new ExpectationException($"No handlers were found for Foo({a})");
					}
				}
				else
				{
					throw new ExpectationException($"No handlers were found for Foo({a})");
				}
			}

	No Parameters:
        public void Target()
        {
            if (this.handlers.TryGetValue(100663535, out var methodHandlers))
            {
                var methodHandler = methodHandlers[0];
                if (methodHandler.Method != null)
                {
#pragma warning disable CS8604
                    ((Action)methodHandler.Method)();
#pragma warning restore CS8604
                }

                methodHandler.RaiseEvents(this);
                methodHandler.IncrementCallCount();
            }
            else
            {
                throw new RE.ExpectationException($"No handlers were found for void Target()");
            }
        }
	*/

	internal static class MockMethodBuilder
	{
		private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, IMethodSymbol method)
		{
			writer.WriteLine("var methodHandler = methodHandlers[0];");
			MockMethodBuilder.BuildMethodHandler(writer, method);
		}

		private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, IMethodSymbol method,
			string methodException)
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
			MockMethodBuilder.BuildMethodHandler(writer, method);
			writer.WriteLine("break;");
			writer.Indent--;
			writer.WriteLine("}");


			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine("if (!foundMatch)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildMethodHandler(IndentedTextWriter writer, IMethodSymbol method)
		{
			writer.WriteLine("if (methodHandler.Method is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			var methodCast = method.Parameters.Length == 0 ? "(Action)" :
				$"(Action<{string.Join(", ", method.Parameters.Select(_ => _.Type.Name))}>)";
			var methodArguments = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ => _.Name));
			writer.WriteLine($"({methodCast}methodHandler.Method)({methodArguments});");

			writer.Indent--;
			writer.WriteLine("}");

			// TODO: We need to detect if the entire mock has events to include "methodHandler.RaiseEvents(this);"
			writer.WriteLine();
			writer.WriteLine("methodHandler.IncrementCallCount();");
		}

		internal static void Build(IndentedTextWriter writer, MethodMockableResult	result, ref uint memberIdentifier)
		{
			var method = result.Value;
			var methodSignature =
				$"{(method.ReturnsVoid ? "void" : method.ReturnType.Name)} {method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {_.Name}"))})";
			var methodException =
				$"{(method.ReturnsVoid ? "void" : method.ReturnType.Name)} {method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))})";

			writer.WriteLine($@"[MemberIdentifier({memberIdentifier}, ""{methodSignature}"")]");
			writer.WriteLine($"public {methodSignature}");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			if(method.Parameters.Length > 0)
			{
				MockMethodBuilder.BuildMethodValidationHandlerWithParameters(writer, method, methodException);
			}
			else
			{
				MockMethodBuilder.BuildMethodValidationHandlerNoParameters(writer, method);
			}

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
			memberIdentifier++;
			writer.WriteLine();
		}
	}
}