using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsRequiresDefaultConstraintTests
{
	private static IEnumerable<TestCaseData> GetDefaultConstraints()
	{
		yield return new TestCaseData("public class Target { public void Foo<T>(T? a) { } }", "where T : default");
		yield return new TestCaseData("public interface IGeneric<T> { } public class Target { public void Foo<T>(IGeneric<T?> a) { } }", "where T : default");
		yield return new TestCaseData("public class Target { public T? Foo<T>() => default!; }", "where T : default");
		yield return new TestCaseData("public interface IGeneric<T> { } public class Target { public IGeneric<T?> Foo<T>() => default!; }", "where T : default");
	}

	private static IEnumerable<TestCaseData> GetDefaultConstraintsWhenNoneExist()
	{
		yield return new TestCaseData("public class Target { public void Foo() { } }");
		yield return new TestCaseData("public class Target { public void Foo<T>() { } }");
		yield return new TestCaseData("public class Target { public void Foo<T>(T a) { } }");
		yield return new TestCaseData("public class Target { public T Foo<T>() => default!; }");
	}

	[TestCaseSource(nameof(GetDefaultConstraints))]
	public static async Task RequiresDefaultConstraintAsync(string code, string expectedValue)
	{
		var methodSymbol = await GetMethodSymbolAsync(code);
		var requiresDefaultConstraints = methodSymbol.GetDefaultConstraints();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(requiresDefaultConstraints, Has.Length.EqualTo(1));
			Assert.That(requiresDefaultConstraints[0].ToString(), Is.EqualTo(expectedValue));
		}
	}

	[TestCaseSource(nameof(GetDefaultConstraintsWhenNoneExist))]
	public static async Task NoDefaultConstraintAsync(string code)
	{
		var methodSymbol = await GetMethodSymbolAsync(code);
		var requiresDefaultConstraints = methodSymbol.GetDefaultConstraints();

		Assert.That(requiresDefaultConstraints, Is.Empty);
	}

	private static async Task<IMethodSymbol> GetMethodSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(_ => _.Identifier.Text == "Foo");
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}