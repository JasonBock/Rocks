using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Discovery;

namespace Rocks.Tests.Discovery;

public static class PropertyMockableResultTests
{
	[Test]
	public static void GetResult()
	{
		var code = "public class Target { public int Foo { get; set; } }";
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<PropertyDeclarationSyntax>().Single();
		var propertySymbol = model.GetDeclaredSymbol(propertySyntax)!;

		var result = new MockablePropertyResult(propertySymbol, propertySymbol.ContainingType,
			RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.Yes, 3);

		Assert.Multiple(() =>
		{
			Assert.That(result.Value, Is.SameAs(propertySymbol));
			Assert.That(result.MockType, Is.SameAs(propertySymbol.ContainingType));
			Assert.That(result.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			Assert.That(result.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(result.Accessors, Is.EqualTo(PropertyAccessor.GetAndSet));
			Assert.That(result.MemberIdentifier, Is.EqualTo(3));
		});
	}
}