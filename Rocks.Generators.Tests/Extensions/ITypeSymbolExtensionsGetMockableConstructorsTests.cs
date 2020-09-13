using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class ITypeSymbolExtensionsGetMockableConstructorsTests
	{
		[Test]
		public static void GetMockableConstructorsForClass()
		{
			var code =
@"public class Target 
{
	public Target() { }
}";
			var typeSymbol = ITypeSymbolExtensionsGetMockableConstructorsTests.GetTypeSymbol(code);
			var constructors = typeSymbol.GetMockableConstructors(typeSymbol.ContainingAssembly);

			Assert.Multiple(() =>
			{
				Assert.That(constructors.Length, Is.EqualTo(1));
			});
		}

		[Test]
		public static void GetMockableConstructorsForClassWhereConstructorCannotBeSeenByInvocationAssembly()
		{
			var code =
@"public class Target 
{
	internal Target() { }
}";
			var typeSymbol = ITypeSymbolExtensionsGetMockableConstructorsTests.GetTypeSymbol(code);

			var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
			var containingReferences = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ =>
				{
					var location = _.Location;
					return MetadataReference.CreateFromFile(location);
				});
			var containingCompilation = CSharpCompilation.Create("InvocationAssembly", new SyntaxTree[] { containingSyntaxTree },
				containingReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			var constructors = typeSymbol.GetMockableConstructors(containingCompilation.Assembly);

			Assert.Multiple(() =>
			{
				Assert.That(constructors.Length, Is.EqualTo(0));
			});
		}

		[Test]
		public static void GetMockableConstructorsForInterface()
		{
			var code = "public interface Target { }";
			var typeSymbol = ITypeSymbolExtensionsGetMockableConstructorsTests.GetTypeSymbol(code);
			var constructors = typeSymbol.GetMockableConstructors(typeSymbol.ContainingAssembly);

			Assert.Multiple(() =>
			{
				Assert.That(constructors.Length, Is.EqualTo(0));
			});
		}

		private static ITypeSymbol GetTypeSymbol(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<TypeDeclarationSyntax>().Single();
			return (ITypeSymbol)model.GetDeclaredSymbol(typeSyntax)!;
		}
	}
}