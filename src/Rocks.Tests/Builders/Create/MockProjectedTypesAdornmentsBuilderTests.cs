using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;

namespace Rocks.Tests.Builders.Create;

internal static class MockProjectedTypesAdornmentsBuilderTests
{
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"AdornmentsForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"AdornmentsForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"AdornmentsForTargetOfstring")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"AdornmentsForTargetOfstring")]
	public static void GetProjectedAdornmentName(string code, string expectedValue)
	{
		var (type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbolFromParameter(code);
		Assert.That(MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsName(
			new TypeReferenceModel(type, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.AdornmentsForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.AdornmentsForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.AdornmentsForTargetOfstring")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.AdornmentsForTargetOfstring")]
	public static void GetProjectedAdornmentFullyQualifiedNameName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbols(code);
		Assert.That(MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsFullyQualifiedNameName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"HandlerForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"HandlerForTargetOfstring")]
	public static void GetProjectedHandlerInformationName(string code, string expectedValue)
	{
		var (type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbolFromParameter(code);
		Assert.That(MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerName(
			new TypeReferenceModel(type, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.HandlerForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.HandlerForTargetOfstring")]
	public static void GetProjectedHandlerInformationFullyQualifiedNameName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbols(code);
		Assert.That(MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerFullyQualifiedNameName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, ITypeSymbol type, Compilation compilation) GetTypeSymbols(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		var mockType = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == "IMock");
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}

	private static (ITypeSymbol, Compilation) GetTypeSymbolFromParameter(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}