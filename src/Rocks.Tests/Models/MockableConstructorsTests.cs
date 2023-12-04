using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

public static class MockableConstructorsTests
{
	[Test]
	public static void GetMockableConstructorsForClass()
	{
		var code =
			"""
			public class Target 
			{
				public Target() { }
			}
			""";
		var (typeSymbol, obsoleteAttribute) = MockableConstructorsTests.GetTypeSymbol(code);
		var constructors = new MockableConstructors(typeSymbol, typeSymbol.ContainingAssembly, obsoleteAttribute).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(1));
	}

	[Test]
	public static void GetMockableConstructorsForClassWhereConstructorCannotBeSeenByInvocationAssembly()
	{
		var code =
			"""
			public class Target 
			{
				internal Target() { }
			}
			""";
		var (typeSymbol, obsoleteAttribute) = MockableConstructorsTests.GetTypeSymbol(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var containingCompilation = CSharpCompilation.Create("InvocationAssembly", new SyntaxTree[] { containingSyntaxTree },
			containingReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var constructors = new MockableConstructors(typeSymbol, containingCompilation.Assembly, obsoleteAttribute).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(0));
	}

	[Test]
	public static void GetMockableConstructorsForInterface()
	{
		var code = "public interface Target { }";
		var (typeSymbol, obsoleteAttribute) = MockableConstructorsTests.GetTypeSymbol(code);
		var constructors = new MockableConstructors(typeSymbol, typeSymbol.ContainingAssembly, obsoleteAttribute).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(0));
	}

	private static (ITypeSymbol type, INamedTypeSymbol obsoleteAttribute) GetTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var obsoleteAttribute = model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName!)!;
		return (model.GetDeclaredSymbol(typeSyntax)!, obsoleteAttribute);
	}
}