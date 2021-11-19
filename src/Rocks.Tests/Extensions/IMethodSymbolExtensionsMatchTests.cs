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
	public static void MatchWhenMethodsAreExactNoParameters()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithParameters()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a) {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithParamsParameter()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a, params object[] b) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a, params object[] b) {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithSameModifiers()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(out int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(out int a) {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsAreExactWithRefAndOutModifiers()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(ref int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(out int a) {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.Exact));
	}

	[Test]
	public static void MatchWhenMethodsDifferByReturnType()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public int {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public string {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.DifferByReturnTypeOnly));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByName()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodTwo}() {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodTwo),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterCount()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}() {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterType()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(string a) {{ }}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	[Test]
	public static void MatchWhenMethodsDoNotMatchByParameterModifier()
	{
		var code =
 $@"public class {IMethodSymbolExtensionsMatchTests.ClassOne}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(int a) {{ }}
}}

public class {IMethodSymbolExtensionsMatchTests.ClassTwo}
{{
	public void {IMethodSymbolExtensionsMatchTests.MethodOne}(out int a) 
	{{
		a = 3;
	}}
}}";

		Assert.That(IMethodSymbolExtensionsMatchTests.MatchMethods(code, IMethodSymbolExtensionsMatchTests.MethodOne, IMethodSymbolExtensionsMatchTests.MethodOne),
			Is.EqualTo(MethodMatch.None));
	}

	private static MethodMatch MatchMethods(string source, string methodOneName, string methodTwoName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
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

		return methodOneSymbol.Match(methodTwoSymbol);
	}
}