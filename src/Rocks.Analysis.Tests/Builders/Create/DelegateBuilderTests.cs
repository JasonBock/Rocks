using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Builders.Create;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Builders.Create;

public static class DelegateBuilderTests
{
	[TestCase("public class Test { public void Foo() { } }", "global::System.Action")]
	[TestCase("public class Test { public void Foo(int a) { } }", "global::System.Action<int>")]
	[TestCase("public class Test { public void Foo(string a = null) { } }", "global::System.Action<string?>")]
	[TestCase("public class Test { public int Foo() { } }", "global::System.Func<int>")]
	[TestCase("public class Test { public int Foo(int a) { } }", "global::System.Func<int, int>")]
	[TestCase("public class Test { public int Foo(string a = null) { } }", "global::System.Func<string?, int>")]
	public static async Task BuildAsync(string code, string expectedValue)
	{
		(var method, var modelContext) = await GetMethodAsync(code);
		var methodModel = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), 
			modelContext, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 0u);
		Assert.That(DelegateBuilder.Build(methodModel), Is.EqualTo(expectedValue));
	}

	private static async Task<(IMethodSymbol, ModelContext)> GetMethodAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return (model.GetDeclaredSymbol((await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!, new(model));
	}
}