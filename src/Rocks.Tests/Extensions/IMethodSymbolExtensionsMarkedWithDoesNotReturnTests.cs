using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsMarkedWithDoesNotReturnTests
{
	[Test]
	public static void GetResultWhenMethodHasDoesNotReturnAttribute()
	{
		var code =
@"using System.Diagnostics.CodeAnalysis;

public static class Test
{
	[DoesNotReturn]
	public static void Foo() { }
}";

		var (method, compilation) = IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbol(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.True);
	}

	[Test]
	public static void GetResultWhenMethodDoesNotHaveDoesNotReturnAttribute()
	{
		var code =
@"public static class Test
{
	public static void Foo() { }
}";

		var (method, compilation) = IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbol(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.False);
	}

	private static (IMethodSymbol, Compilation) GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}