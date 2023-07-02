﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;
using Rocks.Builders;

namespace Rocks.Tests.Builders.Create;

public static class RefLikeArgTypeBuilderTests
{
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }", 
		"ArgForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }", 
		"ArgForTargetOfstring")]
	public static void GetProjectedName(string code, string expectedValue)
	{
		var (type, compilation) = RefLikeArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.RefLikeArgProjectedName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgForTargetOfstring")]
	public static void GetProjectedFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, _) = RefLikeArgTypeBuilderTests.GetTypeSymbols(code);
		Assert.That(RefLikeArgTypeBuilderV3.GetProjectedFullyQualifiedName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"ArgEvaluationForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"ArgEvaluationForTargetOfstring")]
	[TestCase(
		"public static class Test { public static void Foo<TSource>(System.Span<TSource> t) { } }",
		"ArgEvaluationForSpan<TSource>")]
	public static void GetProjectedEvaluationDelegateName(string code, string expectedValue)
	{
		var (type, compilation) = RefLikeArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.RefLikeArgProjectedEvaluationDelegateName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgEvaluationForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgEvaluationForTargetOfstring")]
	public static void GetProjectedEvaluationDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, model) = RefLikeArgTypeBuilderTests.GetTypeSymbols(code);
		Assert.That(RefLikeArgTypeBuilderV3.GetProjectedEvaluationDelegateFullyQualifiedName(
			new TypeReferenceModel(type, compilation), MockModel.Create(typeToMock, model, BuildType.Create, true)!.Type!), Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, ITypeSymbol type, Compilation compilation, SemanticModel model) GetTypeSymbols(string source)
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
		var mockType = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == "IMock");
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type,
			compilation, model);
	}

	private static (ITypeSymbol, Compilation) GetTypeSymbolFromParameter(string source)
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
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}