using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsRequiresDefaultConstraintTests
{
	private static IEnumerable<TestCaseData> GetDefaultConstraints()
	{
		yield return new TestCaseData("public class Target { public void Foo() { } }", ImmutableArray<string>.Empty);
		yield return new TestCaseData("public class Target { public void Foo<T>() { } }", ImmutableArray<string>.Empty);
		yield return new TestCaseData("public class Target { public void Foo<T>(T a) { } }", ImmutableArray<string>.Empty);
		yield return new TestCaseData("public class Target { public void Foo<T>(T? a) { } }", new[] { "where T : default" }.ToImmutableArray());
		yield return new TestCaseData("public class Target { public T Foo<T>() => default!; }", ImmutableArray<string>.Empty);
		yield return new TestCaseData("public class Target { public T? Foo<T>() => default!; }", new[] { "where T : default" }.ToImmutableArray());
	}

	[TestCaseSource(nameof(GetDefaultConstraints))]
	public static void RequiresDefaultConstraint(string code, ImmutableArray<string> expectedValue)
	{
		var methodSymbol = IMethodSymbolExtensionsRequiresDefaultConstraintTests.GetMethodSymbol(code);
		var requiresDefaultConstraints = methodSymbol.GetDefaultConstraints();

		Assert.That(requiresDefaultConstraints, Is.EquivalentTo(expectedValue));
	}

	private static IMethodSymbol GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}