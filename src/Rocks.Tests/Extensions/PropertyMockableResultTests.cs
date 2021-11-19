using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class PropertyMockableResultTests
{
	[Test]
	public static void GetResult()
	{
		var code = "public class Target { public int Foo { get; set; } }";
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<PropertyDeclarationSyntax>().Single();
		var propertySymbol = model.GetDeclaredSymbol(propertySyntax)!;

		var result = new PropertyMockableResult(propertySymbol, propertySymbol.ContainingType,
			RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.Yes, PropertyAccessor.GetAndSet, 3);

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