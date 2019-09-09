using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	internal static partial class ExpressionEvaluation
	{
		// Maybe what I should really do is just provide the method name and a list of 
		// all argument values. That would be easier, no need for weird pseudo-method
		// code.
		// Also, if "Arg" was passed, that would become the literal expectation
		// to pass to the mock machinery.
		// This solves large argument restrictions....does it solve others?
		// Pointers? By-ref? Spans?
		internal static async Task RunEvaluationsAsync()
		{
			await ExpressionEvaluation.EvaluateExpressionAsync<ExpressionTarget>("Target(3, \"value\")", null);
			await ExpressionEvaluation.EvaluateExpressionAsync<ExpressionTarget>("TargetWithNonLiteralValue(3, \"value\")", 
				new Dictionary<int, object>
				{
					{ 2, Guid.NewGuid() }
				});

			await ExpressionEvaluation.EvaluateAsync<ExpressionTarget>(
				nameof(ExpressionTarget.Target), new object[] { 3, "value" });
			await ExpressionEvaluation.EvaluateAsync<ExpressionTarget>(
				nameof(ExpressionTarget.TargetWithNothing), Array.Empty<object>());
			await ExpressionEvaluation.EvaluateAsync<ExpressionTarget>(
				nameof(ExpressionTarget.TargetWithNonLiteralValue), new object[] { 3, "value", Guid.NewGuid() });
		}

		private static async Task EvaluateAsync<T>(string methodName, object[] values)
		{
			await Console.Out.WriteLineAsync($"Method name: {methodName}");
			await Console.Out.WriteLineAsync($"Argument count: {values.Length}");

			foreach(var value in values)
			{
				await Console.Out.WriteLineAsync($"Value is {value}, type is {value.GetType().Name}");
			}

			var match = typeof(T).GetMethod(methodName, values.Select(_ => _.GetType()).ToArray());

			if (!(match is null))
			{
				await Console.Out.WriteLineAsync($"Match found: {match.ToString()}");
			}
			else
			{
				await Console.Out.WriteLineAsync("No match found");
			}

			await Console.Out.WriteLineAsync();
		}

		private static async Task EvaluateExpressionAsync<T>(string expression, Dictionary<int, object>? nonLiterals)
		{
			await Console.Out.WriteLineAsync($"Code: {expression}");

			if (SyntaxFactory.ParseExpression(expression) is InvocationExpressionSyntax invocation)
			{
				if (!invocation.ContainsDiagnostics)
				{
					var invocationName = invocation.Expression as IdentifierNameSyntax;
					var invocationArguments = invocation.ArgumentList.Arguments;
					var invocationNameValue = invocationName.TryGetInferredMemberName();

					await Console.Out.WriteLineAsync($"Method name: {invocationNameValue}");
					await Console.Out.WriteLineAsync($"Argument count: {invocationArguments.Count}");

					var invocationArgumentTypes = new List<Type>();

					if (invocationArguments.Count > 0)
					{
						foreach (var expressionArgument in invocationArguments)
						{
							switch (expressionArgument.Expression)
							{
								case LiteralExpressionSyntax literal:
									var value = literal.Token.Value;
									await Console.Out.WriteLineAsync($"Value is {value}, type is {value.GetType().Name}");
									invocationArgumentTypes.Add(value.GetType());
									break;
								default:
									await Console.Out.WriteLineAsync($"Unknown expression type: {expressionArgument.Expression.GetType().Name}");
									break;
							}
						}
					}

					if(!(nonLiterals is null))
					{
						foreach (var nonLiteralPair in nonLiterals)
						{
							await Console.Out.WriteLineAsync($"Non-literal: {nonLiteralPair.Key}, {nonLiteralPair.Value}");
							invocationArgumentTypes.Insert(nonLiteralPair.Key, nonLiteralPair.Value.GetType());
						}
					}

					var matchInvocation = typeof(T).GetMethod(invocationNameValue, invocationArgumentTypes.ToArray());

					if(!(matchInvocation is null))
					{
						await Console.Out.WriteLineAsync($"Match found: {matchInvocation.ToString()}");
					}
					else
					{
						await Console.Out.WriteLineAsync("No match found");
					}
				}
				else
				{
					foreach (var diagnostic in invocation.GetDiagnostics())
					{
						await Console.Out.WriteLineAsync($"Diagnostic: {diagnostic.ToString()}");
					}
				}
			}
			else
			{
				await Console.Out.WriteLineAsync($"The given expression cannot be parsed into a {nameof(InvocationExpressionSyntax)}");
			}

			await Console.Out.WriteLineAsync();
		}
	}
}