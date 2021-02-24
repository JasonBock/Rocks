using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class SyntaxNodeExtensionsFindParentTests
	{
		[Test]
		public static void FindParentWhenTypeExists()
		{
			var code =
@"public class Test
{
	public void Foo() { }
}";
			var methodDeclaration = SyntaxNodeExtensionsFindParentTests.GetMethodDeclaration(code);

			Assert.That(methodDeclaration.FindParent<TypeDeclarationSyntax>(), Is.Not.Null);
		}

		[Test]
		public static void FindParentWhenTypeDoesNotExists()
		{
			var code =
@"public class Test
{
	public void Foo() { }
}";
			var methodDeclaration = SyntaxNodeExtensionsFindParentTests.GetMethodDeclaration(code);

			Assert.That(methodDeclaration.FindParent<VariableDeclarationSyntax>(), Is.Null);
		}

		private static MethodDeclarationSyntax GetMethodDeclaration(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			return syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single();
		}
	}
}