using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

public static class ConstructorPropertyModelTests
{
	[Test]
	public static void Create()
	{
		var code =
			"""
			public class Target
			{
				public string Value { get; init; }
			}
			""";

		(var type, var property, var compilation) = ConstructorPropertyModelTests.GetSymbolsCompilation(code);
		var model = new ConstructorPropertyModel(property, new TypeReferenceModel(type, compilation), compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.Type.FullyQualifiedName, Is.EqualTo("string"));
			Assert.That(model.Name, Is.EqualTo("Value"));
			Assert.That(model.IsRequired, Is.False);
			Assert.That(model.IsIndexer, Is.False);
			Assert.That(model.Accessors, Is.EqualTo(PropertyAccessor.GetAndInit));
			Assert.That(model.CanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.Parameters, Is.Empty);
			Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.None));
			Assert.That(model.IsReferenceType, Is.True);
		});
	}

	[Test]
	public static void CreateWithRequired()
	{
		var code =
			"""
			public class Target
			{
				public required string Value { get; init; }
			}
			""";

		(var type, var property, var compilation) = ConstructorPropertyModelTests.GetSymbolsCompilation(code);
		var model = new ConstructorPropertyModel(property, new TypeReferenceModel(type, compilation), compilation);

		Assert.That(model.IsRequired, Is.True);
	}

	[Test]
	public static void CreateWithIndexer()
	{
		var code =
			"""
			public class Target
			{
				public string this[string data] { get => string.Empty; init { } }
			}
			""";

		(var type, var property, var compilation) = ConstructorPropertyModelTests.GetSymbolsCompilationWithIndexer(code);
		var model = new ConstructorPropertyModel(property, new TypeReferenceModel(type, compilation), compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.IsIndexer, Is.True);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("data"));
		});
	}

	[Test]
	public static void CreateWithNullableAnnotation()
	{
		var code =
			"""
			public class Target
			{
				public string? Value { get; init; }
			}
			""";

		(var type, var property, var compilation) = ConstructorPropertyModelTests.GetSymbolsCompilation(code);
		var model = new ConstructorPropertyModel(property, new TypeReferenceModel(type, compilation), compilation);

		Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.Annotated));
	}

	[Test]
	public static void CreateWithValueType()
	{
		var code =
			"""
			public class Target
			{
				public int Value { get; init; }
			}
			""";

		(var type, var property, var compilation) = ConstructorPropertyModelTests.GetSymbolsCompilation(code);
		var model = new ConstructorPropertyModel(property, new TypeReferenceModel(type, compilation), compilation);

		Assert.That(model.IsReferenceType, Is.False);
	}

	private static (ITypeSymbol, IPropertySymbol, Compilation) GetSymbolsCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<PropertyDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, model.GetDeclaredSymbol(propertySyntax)!, compilation);
	}

	private static (ITypeSymbol, IPropertySymbol, Compilation) GetSymbolsCompilationWithIndexer(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<IndexerDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, model.GetDeclaredSymbol(propertySyntax)!, compilation);
	}
}