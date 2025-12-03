using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

internal static class IPropertySymbolExtensionsGetAccessorsTests
{
	[TestCase("public class Target { public int Foo { get; } }", PropertyAccessor.Get)]
	[TestCase("public class Target { public int Foo { get; set; } }", PropertyAccessor.GetAndSet)]
	[TestCase("public class Target { public int Foo { get; init; } }", PropertyAccessor.GetAndInit)]
	[TestCase("public class Target { public int Foo { set; } }", PropertyAccessor.Set)]
	[TestCase("public class Target { public int Foo { init; } }", PropertyAccessor.Init)]
	public static async Task IsUnsafeAsync(string code, PropertyAccessor expectedValue)
	{
		var propertySymbol = await IPropertySymbolExtensionsGetAccessorsTests.GetPropertySymbolAsync(code);

		Assert.That(propertySymbol.GetAccessors(), Is.EqualTo(expectedValue));
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