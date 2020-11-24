using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class ITypeSymbolExtensionsGetName
	{
		[TestCase("public class Target { }", GenericsOption.NoGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> { }", GenericsOption.NoGenerics, "Target")]
		[TestCase("public class Target { }", GenericsOption.IncludeGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> { }", GenericsOption.IncludeGenerics, "Target<T, T2, TSomething>")]
		[TestCase("public class Target { }", GenericsOption.FlattenGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> { }", GenericsOption.FlattenGenerics, "TargetOfT_T2_TSomething")]
		public static void GetName(string code, GenericsOption option, string expectedName)
		{
			var typeSymbol = ITypeSymbolExtensionsGetName.GetTypeSymbol(code);
			var name = typeSymbol.GetName(option);

			Assert.Multiple(() =>
			{
				Assert.That(name, Is.EqualTo(expectedName));
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
				.OfType<TypeDeclarationSyntax>().Where(_ => _.Identifier.Text == "Target").Single();
			return model.GetDeclaredSymbol(typeSyntax)!;
		}
	}
}