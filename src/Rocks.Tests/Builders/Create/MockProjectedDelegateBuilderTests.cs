using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;

namespace Rocks.Tests.Builders.Create;

public static class MockProjectedDelegateBuilderTests
{
	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"FooCallback_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"FooCallback_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedCallbackDelegateName(string code, string expectedValue)
	{
		var type = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		Assert.That(MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(type), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.FooCallback_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.FooCallback_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedCallbackDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, method) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		Assert.That(MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, typeToMock), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"FooReturnValue_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"FooReturnValue_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedReturnValueDelegateName(string code, string expectedValue)
	{
		var type = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		Assert.That(MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(type), Is.EqualTo(expectedValue));
	}

	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target { } public static class Test { public static void Foo(Target t) { } } } }",
		"global::Mock.ProjectionsForIMock.FooReturnValue_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"namespace Mock { public interface IMock { } } namespace Outer { namespace Inner { public class Target<T> { } public static class Test { public static void Foo(Target<string> t) { } } } }",
		"global::Mock.ProjectionsForIMock.FooReturnValue_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedReturnValueDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, method) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		Assert.That(MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, typeToMock), Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, IMethodSymbol method) GetSymbols(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		var mockType = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == "IMock");
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!);
	}

	private static IMethodSymbol GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}