using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;
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
		"Callback_647318856387459030730720199270116526843799107753")]
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
		"Callback_204004461298901558364159322673297972159356701735")]
	public static void GetProjectedCallbackDelegateName(string code, string expectedValue)
	{
		(var symbol, var compilation) = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		var method = new MethodModel(symbol, new TypeReferenceModel(symbol.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);
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
		"global::Mock.IMockCreateExpectations.Projections.Callback_647318856387459030730720199270116526843799107753")]
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
		"global::Mock.IMockCreateExpectations.Projections.Callback_204004461298901558364159322673297972159356701735")]
	public static void GetProjectedCallbackDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, method, compilation) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		var name = MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(
			new MethodModel(method, new TypeReferenceModel(typeToMock, compilation), compilation,
				RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1),
			new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo(expectedValue));
	}

	[Test]
	public static void GetProjectedReturnValueDelegateName()
	{
		var code =
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
			""";
		var (method, compilation) = MockProjectedDelegateBuilderTests.GetMethodSymbol(code);
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);
		Assert.That(model.ProjectedReturnValueDelegateName, Is.EqualTo("ReturnValue_305616756374865012389506681414051734154688895315"));
	}

	[Test]
	public static void GetProjectedReturnValueDelegateFullyQualifiedName()
	{
		var code =
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
			""";
		var (typeToMock, method, compilation) = MockProjectedDelegateBuilderTests.GetSymbols(code);
		var name = MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(
			new MethodModel(method, new TypeReferenceModel(typeToMock, compilation), compilation,
				RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1),
			new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo("global::Mock.IMockCreateExpectations.Projections.ReturnValue_305616756374865012389506681414051734154688895315"));
	}

	private static (ITypeSymbol typeToMock, IMethodSymbol method, Compilation compilation) GetSymbols(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
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
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}