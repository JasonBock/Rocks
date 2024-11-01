using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ISymbolExtensionsTests
{
	[TestCase("public class Target { public void Foo() { } }", true, "public")]
	[TestCase("public class Target { private void Foo() { } }", true, "private")]
	[TestCase("public class Target { protected void Foo() { } }", true, "protected")]
	[TestCase("public class Target { internal void Foo() { } }", true, "internal")]
	[TestCase("public class Target { protected internal void Foo() { } }", true, "protected internal")]
	[TestCase("public class Target { protected internal void Foo() { } }", false, "protected")]
	[TestCase("public class Target { private protected void Foo() { } }", true, "private protected")]
	public static void GetOverridingCodeValueTest(string source, bool areAssembliesIdentical, string codeValue)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var method = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		var symbol = compilation.GetSemanticModel(syntaxTree, true).GetDeclaredSymbol(method)!;

		IAssemblySymbol? compilationAssembly = null;

		if (areAssembliesIdentical)
		{
			compilationAssembly = compilation.Assembly;
		}
		else
		{
			var selfSource = "public static class Stuff { }";
			var selfSyntaxTree = CSharpSyntaxTree.ParseText(selfSource);
			var selfCompilation = CSharpCompilation.Create("generator", [selfSyntaxTree],
				Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			compilationAssembly = selfCompilation.Assembly;
		}

		Assert.That(symbol.GetAccessibilityValue(compilationAssembly), Is.EqualTo(codeValue));
	}

}