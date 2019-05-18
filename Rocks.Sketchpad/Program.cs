using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Rocks.Extensions;
using Rocks.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		public static string[] A() => Array.Empty<string>();

		public static string?[] B() => Array.Empty<string>();

		public static string[]? C() => Array.Empty<string>();

		public static string?[]? D() => Array.Empty<string>();

		static void Main() =>
			//BenchmarkRunner.Run<GenericArgumentsTests>();
			//await EvaluateExpressionAsync("Do()");
			//await EvaluateExpressionAsync("Do(");
			//await EvaluateExpressionAsync("Do(3, 4)");
			//await EvaluateExpressionAsync("Do(3, \"hi\", 'c', 4)");
			//await EvaluateExpressionAsync("Do(3, Guid.NewGuid())");
			//await EvaluateExpressionAsync("Do(3, Guid.Parse(Guid.NewGuid().ToString(\"N\")))");
			//Demo.Demonstrate();
			//Program.HandleVirtualOnClass();
			//Program.HandleFoo();
			Program.InvestigateNullabilityOnObject();

		private static void InvestigateNullabilityOnObject()
		{
			var method = typeof(Program).GetMethod(nameof(D));
			var returnParameter = method.ReturnParameter;

			Console.Out.WriteLine(returnParameter.IsNullableReference());
			Console.Out.WriteLine(returnParameter.ParameterType.IsArray);
		}

		private static void RunBenchmark() =>
			BenchmarkRunner.Run<MetadataReferenceCacheBenchmark>();

		public class IFoo
		{
			public virtual int Bar() => 42;
		}

		public static void HandleFoo()
		{
			var rock = Rock.Create<IFoo>();
			//rock.Handle(_ => _.Bar(3));
			var chunk = rock.Make();
			chunk.Bar();
			rock.Verify();
		}

		public class Virtualized
		{
			public virtual void Foo() => Console.Out.WriteLine("Call me!");
		}

		private static void HandleVirtualOnClass()
		{
			var rock = Rock.Create<Virtualized>();
			rock.Handle(_ => _.Foo()); 
			var chunk = rock.Make();
			chunk.Foo();
			rock.Verify();
		}

		private static async Task EvaluateExpressionAsync(string codeExpression)
		{
			Console.Out.WriteLine($"Code: {codeExpression}");
			var expression = SyntaxFactory.ParseExpression(codeExpression) as InvocationExpressionSyntax;

			if (!expression.ContainsDiagnostics)
			{
				var expressionName = expression.Expression as IdentifierNameSyntax;
				var expressionNameValue = expressionName.TryGetInferredMemberName();
				var expressionArguments = expression.ArgumentList.Arguments;

				Console.Out.WriteLine($"Method name: {expressionNameValue}");
				Console.Out.WriteLine($"Argument count: {expressionArguments.Count}");

				if (expressionArguments.Count > 0)
				{
					foreach (var expressionArgument in expressionArguments)
					{
						switch (expressionArgument.Expression)
						{
							case LiteralExpressionSyntax literal:
								var value = literal.Token.Value;
								Console.Out.WriteLine($"{nameof(LiteralExpressionSyntax)}, value is {value}, type is {value.GetType().Name}");
								break;
							case InvocationExpressionSyntax invocation:
								var result = await CSharpScript.EvaluateAsync(
									invocation.ToString(),
									options: ScriptOptions.Default
										.AddReferences(typeof(object).Assembly)
										.AddImports(typeof(object).Namespace));
								Console.Out.WriteLine($"{nameof(InvocationExpressionSyntax)}, expression is {invocation}, result is {result}, type is {result.GetType().Name}");
								break;
							default:
								Console.Out.WriteLine($"Unknown expression type: {expressionArgument.Expression.GetType().Name}");
								break;
						}
					}
				}
			}
			else
			{
				foreach (var diagnostic in expression.GetDiagnostics())
				{
					Console.Out.WriteLine(diagnostic.ToString());
				}
			}

			Console.Out.WriteLine();
		}

		private static void UnicodeTest()
		{
			var rock = Rock.Create<UnicodeEncoding>(
				new RockOptions(
					level: OptimizationSetting.Debug,
					codeFile: CodeFileOptions.Create));
			rock.Handle(_ => _.GetHashCode()).Returns(1);

			var chunk = rock.Make();

			Console.Out.WriteLine(chunk.GetHashCode());

			rock.Verify();
		}
	}

#pragma warning disable CS0067
	public interface IStupid
	{
		void Foo();
	}

	public interface IBaseEvents
	{
		event EventHandler BaseEvent;
	}

	public interface ISubEvents : IBaseEvents
	{
		event EventHandler SubEvent;
	}

	public class BaseEvents
	{
		public event EventHandler BaseEvent;
	}

	public class SubEvents : BaseEvents
	{
		public event EventHandler SubEvent;
	}

	public abstract class HandleISubEvents : ISubEvents
	{
		public abstract event EventHandler SubEvent;
		public abstract event EventHandler BaseEvent;
	}
#pragma warning restore CS0067

	public interface IHavePrimitives
	{
		char DoSomething(int x);
	}
}