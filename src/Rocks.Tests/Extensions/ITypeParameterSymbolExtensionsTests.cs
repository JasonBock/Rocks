using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeParameterSymbolExtensionsTests
{
	[TestCase("public class Target<T> { }", "")]
	[TestCase("public class Base { } public class Target<T> where T : Base { }", "where T : Base")]
	[TestCase("public class Base { } public class Target<T> where T : Base? { }", "where T : Base?")]
	[TestCase("public class Target<T, U> where T : U { }", "where T : U")]
	[TestCase("public class Target<T> where T : class { }", "where T : class")]
	[TestCase("public class Target<T> where T : class, new() { }", "where T : class, new()")]
	[TestCase("public class Target<T> where T : class? { }", "where T : class?")]
	[TestCase("public class Target<T> where T : new() { }", "where T : new()")]
	[TestCase("public class Target<T> where T : notnull { }", "where T : notnull")]
	[TestCase("public class Target<T> where T : unmanaged { }", "where T : unmanaged, struct")]
	[TestCase("public class Target<T> where T : struct { }", "where T : struct")]
	public static void GetConstraints(string code, string expectedConstraints)
	{
		var constraints = ITypeParameterSymbolExtensionsTests.GetTypeParameterSymbol(code).GetConstraints();

		Assert.That(constraints, Is.EqualTo(expectedConstraints));
	}

	private static ITypeParameterSymbol GetTypeParameterSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Where(_ => _.Identifier.Text == "Target").Single();
		return model.GetDeclaredSymbol(typeSyntax)!.TypeParameters[0];
	}
}