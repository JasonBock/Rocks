using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockableConstructorDiscoveryTests
{
	[Test]
	public static async Task GetMockableConstructorsForClassAsync()
	{
		var code =
			"""
			public class Target 
			{
				public Target() { }
			}
			""";
		var (typeSymbol, obsoleteAttribute, compilation) = await GetTypeSymbolAsync(code);
		var constructors = new MockableConstructorDiscovery(typeSymbol, typeSymbol.ContainingAssembly, obsoleteAttribute, compilation).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(1));
	}

	[Test]
	public static async Task GetMockableConstructorsForClassWhereConstructorCannotBeSeenByInvocationAssemblyAsync()
	{
		var code =
			"""
			public class Target 
			{
				internal Target() { }
			}
			""";
		var (typeSymbol, obsoleteAttribute, compilation) = await GetTypeSymbolAsync(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create("InvocationAssembly", [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var constructors = new MockableConstructorDiscovery(typeSymbol, containingCompilation.Assembly, obsoleteAttribute, compilation).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(0));
	}

	[Test]
	public static async Task GetMockableConstructorsForInterfaceAsync()
	{
		var code = "public interface Target { }";
		var (typeSymbol, obsoleteAttribute, compilation) = await GetTypeSymbolAsync(code);
		var constructors = new MockableConstructorDiscovery(typeSymbol, typeSymbol.ContainingAssembly, obsoleteAttribute, compilation).Constructors;

		Assert.That(constructors, Has.Length.EqualTo(0));
	}

	private static async Task<(ITypeSymbol, INamedTypeSymbol, Compilation)> GetTypeSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var obsoleteAttribute = model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName!)!;
		return (model.GetDeclaredSymbol(typeSyntax)!, obsoleteAttribute, compilation);
	}
}