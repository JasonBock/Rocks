using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;

namespace Rocks.Tests.Builders.Create;

public static class DelegateBuilderTests
{
	[TestCase("public class Test { public void Foo() { } }", "global::System.Action")]
	[TestCase("public class Test { public void Foo(int a) { } }", "global::System.Action<int>")]
	[TestCase("public class Test { public void Foo(string a = null) { } }", "global::System.Action<string?>")]
	[TestCase("public class Test { public int Foo() { } }", "global::System.Func<int>")]
	[TestCase("public class Test { public int Foo(int a) { } }", "global::System.Func<int, int>")]
	[TestCase("public class Test { public int Foo(string a = null) { } }", "global::System.Func<string?, int>")]
	public static void Build(string code, string expectedValue)
	{
		(var method, var modelContext) = DelegateBuilderTests.GetMethod(code);
		var methodModel = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), 
			modelContext, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 0u);
		Assert.That(DelegateBuilder.Build(methodModel), Is.EqualTo(expectedValue));
	}

	private static (IMethodSymbol, ModelContext) GetMethod(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return (model.GetDeclaredSymbol(syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!, new(model));
	}
}