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
		[TestCase("public class Target<T, T2, TSomething> : Base", GenericsOption.FlattenGenerics, "TargetOfT_T2_TSomething")]
		public static void GetName(string code, GenericsOption option, string expectedName)
		{
			var typeSymbol = ITypeSymbolExtensionsGetName.GetTypeSymbol(code);
			var name = typeSymbol.GetName(option);

			Assert.Multiple(() =>
			{
				Assert.That(name, Is.EqualTo(expectedName));
			});
		}

		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			GenericsOption.NoGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			GenericsOption.NoGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			GenericsOption.NoGenerics, "Base")]
		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			GenericsOption.IncludeGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			GenericsOption.IncludeGenerics, "Base<int, string, Guid>")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			GenericsOption.IncludeGenerics, "Base<T, string, TSomething>")]
		[TestCase("public class Base { } public class Target { Base Data { get; } }",
			GenericsOption.FlattenGenerics, "Base")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
			GenericsOption.FlattenGenerics, "BaseOfint_string_Guid")]
		[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
			GenericsOption.FlattenGenerics, "BaseOfT_string_TSomething")]
		public static void GetNameFromDeclaredType(string code, GenericsOption option, string expectedName)
		{
			var parameterSymbol = ITypeSymbolExtensionsGetName.GetDeclaredTypeSymbol(code);
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