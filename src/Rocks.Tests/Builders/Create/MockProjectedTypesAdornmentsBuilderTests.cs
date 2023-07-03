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
		AdornmentType.Method, false,
		"MethodAdornmentsForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		AdornmentType.Method, true,
		"ExplicitMethodAdornmentsForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		AdornmentType.Method, false,
		"MethodAdornmentsForTargetOfstring")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		AdornmentType.Method, true,
		"ExplicitMethodAdornmentsForTargetOfstring")]
	public static void GetProjectedAdornmentName(string code, AdornmentType adornment, bool isExplicit, string expectedValue)
	{
		var (type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbolFromParameter(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(
			new TypeReferenceModel(type, compilation), adornment, isExplicit), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		AdornmentType.Method, false,
		"global::Mock.ProjectionsForIMock.MethodAdornmentsForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		AdornmentType.Method, true,
		"global::Mock.ProjectionsForIMock.ExplicitMethodAdornmentsForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		AdornmentType.Method, false,
		"global::Mock.ProjectionsForIMock.MethodAdornmentsForTargetOfstring")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		AdornmentType.Method, true,
		"global::Mock.ProjectionsForIMock.ExplicitMethodAdornmentsForTargetOfstring")]
	public static void GetProjectedAdornmentFullyQualifiedNameName(string code, AdornmentType adornment, bool isExplicit, string expectedValue)
	{
		var (typeToMock, type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbols(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation), adornment, isExplicit), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"HandlerInformationForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"HandlerInformationForTargetOfstring")]
	public static void GetProjectedHandlerInformationName(string code, string expectedValue)
	{
		var (type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbolFromParameter(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(
			new TypeReferenceModel(type, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.HandlerInformationForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.HandlerInformationForTargetOfstring")]
	public static void GetProjectedHandlerInformationFullyQualifiedNameName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbols(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"AddForTarget")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"AddForTargetOfstring")]
	public static void GetProjectedAddExtensionMethodName(string code, string expectedValue)
	{
		var (type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbolFromParameter(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(
			new TypeReferenceModel(type, compilation)), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.AddForTarget")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.AddForTargetOfstring")]
	public static void GetProjectedAddExtensionMethodFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation) = MockProjectedTypesAdornmentsBuilderTests.GetTypeSymbols(code);
		Assert.That(MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation)), Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, ITypeSymbol type, Compilation compilation) GetTypeSymbols(string source)
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
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
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