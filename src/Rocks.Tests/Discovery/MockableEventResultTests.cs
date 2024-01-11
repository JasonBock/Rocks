using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Discovery;

namespace Rocks.Tests.Discovery;

public static class MockableEventResultTests
{
	[Test]
	public static void GetResults()
	{
		var code = "public class Target { public event EventHandler MyEvent; }";
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var eventSymbol = (IEventSymbol)model.GetDeclaredSymbol(typeSyntax)!.GetMembers().Single(_ => _ is IEventSymbol);

		var result = new MockableEventResult(eventSymbol, RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.Yes);

		Assert.Multiple(() =>
		{
			Assert.That(result.Value, Is.SameAs(eventSymbol));
			Assert.That(result.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			Assert.That(result.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}
}