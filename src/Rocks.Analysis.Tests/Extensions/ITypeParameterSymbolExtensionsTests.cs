using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeParameterSymbolExtensionsTests
{
	[TestCase("public class Base { } public class Target<T> where T : Base { }", "where T : global::Base")]
	[TestCase("public class Base { } public class Target<T> where T : Base? { }", "where T : global::Base?")]
	[TestCase("public class Target<T, U> where T : U { }", "where T : U")]
	[TestCase("public class Target<T> where T : class { }", "where T : class")]
	[TestCase("public class Target<T> where T : class, new() { }", "where T : class, new()")]
	[TestCase("public class Target<T> where T : class? { }", "where T : class?")]
	[TestCase("public class Target<T> where T : new() { }", "where T : new()")]
	[TestCase("public class Target<T> where T : notnull { }", "where T : notnull")]
	[TestCase("public class Target<T> where T : unmanaged { }", "where T : unmanaged")]
	[TestCase("public class Target<T> where T : struct { }", "where T : struct")]
	[TestCase("public interface IThing { } public class Target<T> where T : struct, IThing { }", "where T : struct, global::IThing")]
	[TestCase("public class Thing { } public interface IThing { } public class Target<T> where T : Thing, IThing { }", "where T : global::Thing, global::IThing")]
	public static async Task GetConstraintsAsync(string code, string expectedConstraints)
	{
		var (type, compilation) = await GetTypeParameterSymbolAsync(code);
		var constraint = type.GetConstraints(compilation);

		Assert.That(constraint!.ToString(), Is.EqualTo(expectedConstraints));
	}

	[Test]
	public static async Task GetConstraintsWhenNoConstraintsExistAsync()
	{
		var code = "public class Target<T> { }";
		var (type, compilation) = await GetTypeParameterSymbolAsync(code);
		var constraint = type.GetConstraints(compilation);

		Assert.That(constraint, Is.Null);
	}

	private static async Task<(ITypeParameterSymbol, Compilation)> GetTypeParameterSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == "Target");
		return (model.GetDeclaredSymbol(typeSyntax)!.TypeParameters[0], compilation);
	}
}