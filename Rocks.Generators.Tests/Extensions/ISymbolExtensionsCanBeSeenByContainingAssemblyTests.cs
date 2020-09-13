using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class ISymbolExtensionsCanBeSeenByContainingAssemblyTests
	{
		[Test]
		public static void CallWhenSymbolIsPublic()
		{
			var code =
@"public class Source
{
	public void Foo() { }
}";
			var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

			Assert.Multiple(() =>
			{
				Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
			});
		}

		[Test]
		public static void CallWhenSymbolIsProtected()
		{

		}

		[Test]
		public static void CallWhenSymbolIsProtectedInternal()
		{

		}

		[Test]
		public static void CallWhenSymbolIsInternalAndContainingAssemblyEqualsInvocationAssembly()
		{

		}

		[Test]
		public static void CallWhenSymbolIsInternalAndContainingAssemblyExposesToInvocationAssembly()
		{

		}

		[Test]
		public static void CallWhenSymbolIsInternalAndContainingAssemblyDoesNotExposeToInvocationAssembly()
		{

		}

		[Test]
		public static void CallWhenSymbolIsProtectedInternalAndContainingAssemblyEqualsInvocationAssembly()
		{

		}

		[Test]
		public static void CallWhenSymbolIsProtectedInternalAndContainingAssemblyExposesToInvocationAssembly()
		{

		}

		[Test]
		public static void CallWhenSymbolIsProtectedInternalAndContainingAssemblyDoesNotExposeToInvocationAssembly()
		{

		}

		private static ISymbol GetSymbol(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single();
			return model.GetDeclaredSymbol(methodSyntax)!;
		}
	}
}