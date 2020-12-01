using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace Rocks.Tests.Extensions
{
	public static class ITypeSymbolExtensionsGetNameTests
	{
		[TestCase("public class Target { }", TypeNameOption.NoGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> { }", TypeNameOption.NoGenerics, "Target")]
		[TestCase("public class Target { }", TypeNameOption.IncludeGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> { }", TypeNameOption.IncludeGenerics, "Target<T, T2, TSomething>")]
		[TestCase("public class Target { }", TypeNameOption.FlattenGenerics, "Target")]
		[TestCase("public class Target<T, T2, TSomething> : Base", TypeNameOption.FlattenGenerics, "TargetOfT_T2_TSomething")]
		public static void GetName(string code, TypeNameOption option, string expectedName)
		{
			var typeSymbol = ITypeSymbolExtensionsGetNameTests.GetTypeSymbol(code);
			var name = typeSymbol.GetName(option);

			Assert.Multiple(() =>
			{
				Assert.That(name, Is.EqualTo(expectedName));
			});
		}

		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			TypeNameOption.NoGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			TypeNameOption.NoGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			TypeNameOption.NoGenerics, "Base")]
		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			TypeNameOption.IncludeGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			TypeNameOption.IncludeGenerics, "Base<int, string, Guid>")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			TypeNameOption.IncludeGenerics, "Base<T, string, TSomething>")]
		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			TypeNameOption.FlattenGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			TypeNameOption.FlattenGenerics, "BaseOfint_string_Guid")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			TypeNameOption.FlattenGenerics, "BaseOfT_string_TSomething")]
		public static void GetNameFromDeclaredType(string code, TypeNameOption option, string expectedName)
		{
			var parameterSymbol = ITypeSymbolExtensionsGetNameTests.GetDeclaredTypeSymbol(code);
			var name = parameterSymbol.Type.GetName(option);

			Assert.Multiple(() =>
			{
				Assert.That(name, Is.EqualTo(expectedName));
			});
		}

		private static IPropertySymbol GetDeclaredTypeSymbol(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<PropertyDeclarationSyntax>().Where(_ => _.Identifier.Text == "Data").Single();
			return model.GetDeclaredSymbol(propertySyntax)!;
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