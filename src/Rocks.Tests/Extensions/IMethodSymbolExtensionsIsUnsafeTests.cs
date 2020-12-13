using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class IMethodSymbolExtensionsIsUnsafeTests
	{
		[TestCase("public class Target { public void Foo() { } }", false)]
		[TestCase("public class Target { public void Foo(int a) { } }", false)]
		[TestCase("public class Target { public unsafe void Foo(int* a) { } }", true)]
		[TestCase("public class Target { public int Foo() => default; }", false)]
		[TestCase("public class Target { public unsafe int* Foo() => default; }", true)]
		public static void IsUnsafe(string code, bool expectedValue)
		{
			var methodSymbol = IMethodSymbolExtensionsIsUnsafeTests.GetMethodSymbol(code);

			Assert.Multiple(() =>
			{
				Assert.That(methodSymbol.IsUnsafe(), Is.EqualTo(expectedValue));
			});
		}

		private static IMethodSymbol GetMethodSymbol(string source)
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
			return model.GetDeclaredSymbol(methodSyntax)!;
		}
	}
}