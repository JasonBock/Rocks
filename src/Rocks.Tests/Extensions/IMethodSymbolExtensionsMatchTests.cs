using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsMatchTests
{
	private const string ClassOne = nameof(IMethodSymbolExtensionsMatchTests.ClassOne);
	private const string ClassTwo = nameof(IMethodSymbolExtensionsMatchTests.ClassTwo);
	private const string MethodOne = nameof(IMethodSymbolExtensionsMatchTests.MethodOne);
	private const string MethodTwo = nameof(IMethodSymbolExtensionsMatchTests.MethodTwo);

	[Test]
	public static void MatchWhenMethodsDifferByGenericParameterName()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByGenericParameterNameRecursive()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByParameterPointerTypeNullability()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDifferByParameterArrayTypeNullability()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByParameterArrayDimensionsNullability()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDifferByParameterTypeNullability()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByReturnTypeNullability()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByTypeParameterCountOnly()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithTypeParametersInReturnType()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactNoParameters()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithParameters()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithParamsParameter()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithSameModifiers()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithRefAndOutModifiers()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByReturnType()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.DifferByReturnTypeOrConstraintOnly));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByName()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodTwo),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterCount()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterType()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterModifier()
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

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	private static MethodMatch MatchMethods(string source, string methodOneName, string methodTwoName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodOneSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(
				_ => _.Identifier.Text == IMethodSymbolExtensionsMatchTests.ClassOne).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodOneName);
		var methodTwoSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(
				_ => _.Identifier.Text == IMethodSymbolExtensionsMatchTests.ClassTwo).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(
				_ => _.Identifier.Text == methodTwoName);
		var methodOneSymbol = model.GetDeclaredSymbol(methodOneSyntax)!;
		var methodTwoSymbol = model.GetDeclaredSymbol(methodTwoSyntax)!;

		return methodOneSymbol.Match(methodTwoSymbol, compilation);
	}
}