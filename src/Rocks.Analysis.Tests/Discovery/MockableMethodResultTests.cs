using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockableMethodResultTests
{
	[Test]
	public static void GetResult()
	{
		var code = "public class Target { public void Foo() { } }";
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		var methodSymbol = model.GetDeclaredSymbol(methodSyntax)!;

		var result = new MockableMethodResult(methodSymbol, methodSymbol.ContainingType,
			RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.Yes, RequiresHiding.Yes, 3);

		Assert.Multiple(() =>
		{
			Assert.That(result.Value, Is.SameAs(methodSymbol));
			Assert.That(result.MockType, Is.SameAs(methodSymbol.ContainingType));
			Assert.That(result.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			Assert.That(result.RequiresHiding, Is.EqualTo(RequiresHiding.Yes));
			Assert.That(result.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(result.MemberIdentifier, Is.EqualTo(3));
		});
	}
}