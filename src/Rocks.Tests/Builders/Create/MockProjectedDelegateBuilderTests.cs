using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;
using Rocks.Extensions;
using Rocks.Builders.Create;

namespace Rocks.Tests.Builders.Create;

public static class MockProjectedDelegateBuilderTests
{
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target { } 
				
				public static class Test 
				{ 
					public static void Foo(ref Target t) { } 
				} 
			} 
		}
		""",
		"FooCallback_497363238751704866053452189283926723308443685748")]
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target<T> { } 
				
				public static class Test 
				{ 
					public static void Foo(ref Target<string> t) { } 
				} 
			} 
		}
		""",
		"FooCallback_509704329523946660150386999463474580341671541387")]
	public static void GetProjectedCallbackDelegateName(string code, string expectedValue)
	{
		(var symbol, var compilation) = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		var method = new MethodModel(symbol, new TypeReferenceModel(symbol.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, 1);
		Assert.That(method.ProjectedCallbackDelegateName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"""
		namespace Mock 
		{ 
			public interface IMock { } 
		} 
		
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target { } 
				
				public static class Test 
				{ 
					public static void Foo(ref Target t) { } 
				} 
			} 
		}
		""",
		"global::Mock.ProjectionsForIMock.FooCallback_497363238751704866053452189283926723308443685748")]
	[TestCase(
		"""
		namespace Mock 
		{ 
			public interface IMock { } 
		} 
		
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target<T> { } 
				
				public static class Test 
				{ 
					public static void Foo(ref Target<string> t) { } 
				} 
			} 
		}
		""",
		"global::Mock.ProjectionsForIMock.FooCallback_509704329523946660150386999463474580341671541387")]
	public static void GetProjectedCallbackDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, method, compilation) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		var name = MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(
			new MethodModel(method, new TypeReferenceModel(typeToMock, compilation), compilation,
				RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, 1),
			new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target { } 
				
				public static class Test 
				{ 
					public static System.Span<int> Foo(Target t) => default; 
				} 
			} 
		}
		""",
		"FooReturnValue_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target<T> { } 
				
				public static class Test 
				{ 
					public static System.Span<int> Foo(Target<string> t) => default; 
				} 
			} 
		}
		""",
		"FooReturnValue_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedReturnValueDelegateName(string code, string expectedValue)
	{
		var (method, compilation) = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, 1);
		Assert.That(model.ProjectedReturnValueDelegateName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"""
		namespace Mock 
		{ 
			public interface IMock { } 
		} 
		
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target { } 

				public static class Test 
				{ 
					public static System.Span<int> Foo(Target t) => default; 
				} 
			} 
		}
		""",
		"global::Mock.ProjectionsForIMock.FooReturnValue_150544254424482794293143350194542396194679863893")]
	[TestCase(
		"""
		namespace Mock 
		{ 
			public interface IMock { } 
		} 
		
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public class Target<T> { } 

				public static class Test 
				{ 
					public static System.Span<int> Foo(Target<string> t) => default; 
				} 
			} 
		}
		""",
		"global::Mock.ProjectionsForIMock.FooReturnValue_597877384104637463797567380593086296660561401666")]
	public static void GetProjectedReturnValueDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, method, compilation) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		var name = MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(
			new MethodModel(method, new TypeReferenceModel(typeToMock, compilation), compilation,
				RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, 1),
			new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, IMethodSymbol method, Compilation compilation) GetSymbols(string source)
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
		return (model.GetDeclaredSymbol(mockType)!, model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}

	private static (IMethodSymbol, Compilation) GetMethodSymbol(string source)
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
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}