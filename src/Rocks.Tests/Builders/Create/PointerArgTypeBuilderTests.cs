using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;

namespace Rocks.Tests.Builders.Create;

public static class PointerArgTypeBuilderTests
{
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public struct Target { } 
				
				public static class Test 
				{ 
					public unsafe static void Foo(Target* t) { } 
				} 
			} 
		}
		""",
		"ArgumentForOuter_Inner_TargetPointer")]
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public struct Target<T> { } 
				
				public static class Test 
				{ 
					public unsafe static void Foo(Target<string>* t) { } 
				} 
			} 
		}
		""",
		"ArgumentForOuter_Inner_TargetOfstringPointer")]
	public static void GetProjectedName(string code, string expectedValue)
	{
		var (type, compilation) = PointerArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.PointerArgProjectedName, Is.EqualTo(expectedValue));
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
				public struct Target { } 
				
				public static class Test 
				{ 
					public unsafe static void Foo(Target* t) { } 
				} 
			} 
		}
		""",
		"global::Mock.IMockCreateExpectations.Projections.ArgumentForOuter_Inner_TargetPointer")]
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
				public struct Target<T> { } 
				
				public static class Test 
				{ 
					public unsafe static void Foo(Target<string>* t) { } 
				} 
			} 
		}
		""",
		"global::Mock.IMockCreateExpectations.Projections.ArgumentForOuter_Inner_TargetOfstringPointer")]
	public static void GetProjectedFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, _) = PointerArgTypeBuilderTests.GetTypeSymbols(code);
		var name = PointerArgTypeBuilder.GetProjectedFullyQualifiedName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public struct Target { } 
				
				public static class Test 
				{ 
					public static unsafe void Foo(Target* t) { } 
				} 
			} 
		}
		""",
		"ArgumentEvaluationForOuter_Inner_TargetPointer")]
	[TestCase(
		"""
		namespace Outer 
		{ 
			namespace Inner 
			{ 
				public struct Target<T> { } 
				
				public static class Test 
				{ 
					public static unsafe void Foo(Target<string>* t) { } 
				} 
			} 
		}
		""",
		"ArgumentEvaluationForOuter_Inner_TargetOfstringPointer")]
	public static void GetProjectedEvaluationDelegateName(string code, string expectedValue)
	{
		var (type, compilation) = PointerArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.PointerArgProjectedEvaluationDelegateName, Is.EqualTo(expectedValue));
	}

	[TestCase(
		"""
		using System;

		namespace Mock 
		{ 
			public interface IMock 
			{ 
				unsafe void Foo(int* t);
			} 
		}
		""",
		"global::Mock.IMockCreateExpectations.Projections.ArgumentEvaluationForintPointer")]
	public static void GetProjectedEvaluationDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, model) = PointerArgTypeBuilderTests.GetTypeSymbols(code);
		var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }")); 
		var name = PointerArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(
			new TypeReferenceModel(type, compilation), MockModel.Create(invocation, typeToMock, model, BuildType.Create, true).Information!.Type.Type);
		Assert.That(name, Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol typeToMock, ITypeSymbol type, Compilation compilation, SemanticModel model) GetTypeSymbols(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
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
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}