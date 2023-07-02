using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;
using System.Collections.Immutable;

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
		(var method, var compilation) = DelegateBuilderTests.GetMethod(code);
		Assert.That(DelegateBuilderV3.Build(
			method.Parameters.Select(_ => new ParameterModel(_, new TypeReferenceModel(_.ContainingType, compilation), compilation)).ToImmutableArray(),
				method.ReturnsVoid ? null : new TypeReferenceModel(method.ReturnType, compilation)), 
			Is.EqualTo(expectedValue));
	}

	private static (IMethodSymbol, Compilation) GetMethod(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return (model.GetDeclaredSymbol(syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!, compilation);
	}
}