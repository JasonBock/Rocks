using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IPropertySymbolExtensionsIsUnsafeTests
{
	[TestCase("public class Target { public int Foo { get; } }", false)]
	[TestCase("public class Target { public unsafe int* Foo { get; } }", true)]
	[TestCase("public class Target { public int this[int value] { get; } }", false)]
	[TestCase("public class Target { public unsafe int this[int* value] { get; } }", true)]
	[TestCase("public class Target { public unsafe int* this[int value] { get; } }", true)]
	public static async Task IsUnsafeAsync(string code, bool expectedValue)
	{
		var propertySymbol = await IPropertySymbolExtensionsIsUnsafeTests.GetPropertySymbolAsync(code);

		Assert.That(propertySymbol.IsUnsafe(), Is.EqualTo(expectedValue));
	}

	private static async Task<IPropertySymbol> GetPropertySymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.Single(_ => _.IsKind(SyntaxKind.IndexerDeclaration) || _.IsKind(SyntaxKind.PropertyDeclaration));
		return (model.GetDeclaredSymbol(propertySyntax) as IPropertySymbol)!;
	}
}