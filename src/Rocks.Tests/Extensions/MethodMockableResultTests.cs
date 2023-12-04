using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using Rocks.Models;

namespace Rocks.Tests.Extensions;

public static class MethodMockableResultTests
{
	[Test]
	public static void GetResult()
	{
		var code = "public class Target { public void Foo() { } }";
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		var methodSymbol = model.GetDeclaredSymbol(methodSyntax)!;

		var result = new MethodMockableResult(methodSymbol, methodSymbol.ContainingType,
			RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.Yes, 3);

		Assert.Multiple(() =>
		{
			Assert.That(result.Value, Is.SameAs(methodSymbol));
			Assert.That(result.MockType, Is.SameAs(methodSymbol.ContainingType));
			Assert.That(result.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			Assert.That(result.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(result.MemberIdentifier, Is.EqualTo(3));
		});
	}
}