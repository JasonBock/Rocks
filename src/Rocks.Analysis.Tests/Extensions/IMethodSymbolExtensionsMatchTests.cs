using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsMatchTests
{
	private const string ClassOne = nameof(ClassOne);
	private const string ClassTwo = nameof(ClassTwo);
	private const string MethodOne = nameof(MethodOne);
	private const string MethodTwo = nameof(MethodTwo);

	[Test]
	public static async Task MatchWhenMethodsDifferByGenericParameterNameAsync()
	{
		var code =
			$$"""
			public class Generic<T> { }

			public class {{ClassOne}}
			{
				public void {{MethodOne}}<B>(Generic<B> a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}<S>(Generic<S> a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByGenericParameterNameRecursiveAsync()
	{
		var code =
			$$"""
			public class KeyValue<K, P> { }

			public class {{ClassOne}}
			{
				public void {{MethodOne}}<B, S>(KeyValue<B, KeyValue<B, S>> a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}<B, Q>(KeyValue<B, KeyValue<B, Q>> a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterPointerTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(int* a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterArrayTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(string[] a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(string?[]? a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterArrayDimensionsNullabilityAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(string[,] a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(string[] a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(string a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(string? a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByReturnTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public string {{MethodOne}}() { }
			}

			public class {{ClassTwo}}
			{
				public string? {{MethodOne}}() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByTypeParameterCountOnlyAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}() { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}<T>() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithTypeParametersInReturnTypeAsync()
	{
		var code =
			$$"""
			public class ReturnType<T> { }

			public class {{ClassOne}}
			{
				public ReturnType<T> {{MethodOne}}<T>() { }
			}

			public class {{ClassTwo}}
			{
				public ReturnType<T> {{MethodOne}}<T>() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactNoParametersAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}() { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithParametersAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(int a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithParamsParameterAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a, params object[] b) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(int a, params object[] b) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithSameModifiersAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(out int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(out int a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithRefAndOutModifiersAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(ref int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(out int a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByReturnTypeAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public int {{MethodOne}}() { }
			}

			public class {{ClassTwo}}
			{
				public string {{MethodOne}}() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.DifferByReturnTypeOrConstraintOnly));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByNameAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}() { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodTwo}}() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodTwo),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterCountAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}() { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterTypeAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(string a) { }
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterModifierAsync()
	{
		var code =
			$$"""
			public class {{ClassOne}}
			{
				public void {{MethodOne}}(int a) { }
			}

			public class {{ClassTwo}}
			{
				public void {{MethodOne}}(out int a) 
				{
					a = 3;
				}
			}
			""";

		Assert.That(await MatchMethodsAsync(code, MethodOne, MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	private static async Task<MethodMatch> MatchMethodsAsync(string source, string methodOneName, string methodTwoName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodOneSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(
				_ => _.Identifier.Text == ClassOne).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodOneName);
		var methodTwoSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(
				_ => _.Identifier.Text == ClassTwo).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodTwoName);
		var methodOneSymbol = model.GetDeclaredSymbol(methodOneSyntax)!;
		var methodTwoSymbol = model.GetDeclaredSymbol(methodTwoSyntax)!;

		return methodOneSymbol.Match(methodTwoSymbol, compilation);
	}
}