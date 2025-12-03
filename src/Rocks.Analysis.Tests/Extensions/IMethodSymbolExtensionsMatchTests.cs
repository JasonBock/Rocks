using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsMatchTests
{
	private const string ClassOne = nameof(IMethodSymbolExtensionsMatchTests.ClassOne);
	private const string ClassTwo = nameof(IMethodSymbolExtensionsMatchTests.ClassTwo);
	private const string MethodOne = nameof(IMethodSymbolExtensionsMatchTests.MethodOne);
	private const string MethodTwo = nameof(IMethodSymbolExtensionsMatchTests.MethodTwo);

	[Test]
	public static async Task MatchWhenMethodsDifferByGenericParameterNameAsync()
	{
		var code =
			$$"""
			public class Generic<T> { }

			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}<B>(Generic<B> a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}<S>(Generic<S> a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByGenericParameterNameRecursiveAsync()
	{
		var code =
			$$"""
			public class KeyValue<K, P> { }

			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}<B, S>(KeyValue<B, KeyValue<B, S>> a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}<B, Q>(KeyValue<B, KeyValue<B, Q>> a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterPointerTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int* a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterArrayTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string[] a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string?[]? a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterArrayDimensionsNullabilityAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string[,] a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string[] a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByParameterTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string? a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByReturnTypeNullabilityAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public string {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public string? {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByTypeParameterCountOnlyAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}<T>() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithTypeParametersInReturnTypeAsync()
	{
		var code =
			$$"""
			public class ReturnType<T> { }

			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public ReturnType<T> {{IMethodSymbolExtensionsMatchTests.MethodOne}}<T>() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public ReturnType<T> {{IMethodSymbolExtensionsMatchTests.MethodOne}}<T>() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactNoParametersAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithParametersAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithParamsParameterAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a, params object[] b) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a, params object[] b) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithSameModifiersAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(out int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(out int a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsAreExactWithRefAndOutModifiersAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(ref int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(out int a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static async Task MatchWhenMethodsDifferByReturnTypeAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public int {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public string {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.DifferByReturnTypeOrConstraintOnly));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByNameAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodTwo}}() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodTwo),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterCountAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}() { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterTypeAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(string a) { }
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static async Task MatchWhenMethodsDoNotMatchByParameterModifierAsync()
	{
		var code =
			$$"""
			public class {{IMethodSymbolExtensionsMatchTests.ClassOne}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(int a) { }
			}

			public class {{IMethodSymbolExtensionsMatchTests.ClassTwo}}
			{
				public void {{IMethodSymbolExtensionsMatchTests.MethodOne}}(out int a) 
				{
					a = 3;
				}
			}
			""";

		Assert.That(await IMethodSymbolExtensionsMatchTests.MatchMethodsAsync(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
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
				_ => _.Identifier.Text == IMethodSymbolExtensionsMatchTests.ClassOne).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodOneName);
		var methodTwoSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(
				_ => _.Identifier.Text == IMethodSymbolExtensionsMatchTests.ClassTwo).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodTwoName);
		var methodOneSymbol = model.GetDeclaredSymbol(methodOneSyntax)!;
		var methodTwoSymbol = model.GetDeclaredSymbol(methodTwoSyntax)!;

		return methodOneSymbol.Match(methodTwoSymbol, compilation);
	}
}