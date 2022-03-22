using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IPropertySymbolExtensionsIsUnsafeTests
{
	[TestCase("public class Target { public int Foo { get; } }", false)]
	[TestCase("public class Target { public unsafe int* Foo { get; } }", true)]
	[TestCase("public class Target { public int this[int value] { get; } }", false)]
	[TestCase("public class Target { public unsafe int this[int* value] { get; } }", true)]
	[TestCase("public class Target { public unsafe int* this[int value] { get; } }", true)]
	public static void IsUnsafe(string code, bool expectedValue)
	{
		var propertySymbol = IPropertySymbolExtensionsIsUnsafeTests.GetPropertySymbol(code);

		Assert.That(propertySymbol.IsUnsafe(), Is.EqualTo(expectedValue));
	}

	private static IPropertySymbol GetPropertySymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.Where(_ => _.IsKind(SyntaxKind.IndexerDeclaration) || _.IsKind(SyntaxKind.PropertyDeclaration)).Single();
		return (model.GetDeclaredSymbol(propertySyntax) as IPropertySymbol)!;
	}
}