using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;
using Rocks.Builders;

namespace Rocks.Tests.Builders.Create;

public static class PointerArgTypeBuilderTests
{
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"ArgumentForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"ArgumentForTargetOfstring")]
	public static void GetProjectedName(string code, string expectedValue)
	{
		var (type, compilation) = PointerArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.PointerArgProjectedName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgumentForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgumentForTargetOfstring")]
	public static void GetProjectedFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, _) = PointerArgTypeBuilderTests.GetTypeSymbols(code);
		Assert.That(PointerArgTypeBuilderV3.GetProjectedFullyQualifiedName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"ArgumentEvaluationForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"ArgumentEvaluationForTargetOfstring")]
	public static void GetProjectedEvaluationDelegateName(string code, string expectedValue)
	{
		var (type, compilation) = PointerArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.PointerArgProjectedEvaluationDelegateName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgumentEvaluationForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.ArgumentEvaluationForTargetOfstring")]
	public static void GetProjectedEvaluationDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, model) = PointerArgTypeBuilderTests.GetTypeSymbols(code);
		Assert.That(PointerArgTypeBuilderV3.GetProjectedEvaluationDelegateFullyQualifiedName(
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
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation, model);
	}

	private static (ITypeSymbol typeToMock, Compilation compilation) GetTypeSymbolFromParameter(string source)
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