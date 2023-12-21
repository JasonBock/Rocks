using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;

namespace Rocks.Tests.Builders.Create;

public static class RefLikeArgTypeBuilderTests
{
	[Test]
	public static void GetProjectedName()
	{
		var code =
			"""
			using System;

			namespace Outer 
			{ 
				namespace Inner 
				{ 
					public class Target { } 
				
					public static class Test 
					{ 
						public static void Foo(Span<int> t) { } 
					}
				}
			}
			""";
		var (type, compilation) = RefLikeArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.RefLikeArgProjectedName, Is.EqualTo("ArgForSpanOfint"));
	}

	[Test]
	public static void GetProjectedFullyQualifiedName()
	{
		var code =
			"""
			using System;

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
						public static void Foo(Span<int> t) { } 
					} 
				} 
			}
			""";
		var (typeToMock, type, compilation, _) = RefLikeArgTypeBuilderTests.GetTypeSymbols(code);
		var name = RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(
			new TypeReferenceModel(type, compilation), new TypeReferenceModel(typeToMock, compilation));
		Assert.That(name, Is.EqualTo("global::Mock.ProjectionsForIMock.ArgForSpanOfint"));
	}

	[Test]
	public static void GetProjectedEvaluationDelegateName()
	{
		var code =
			"""
			public static class Test 
			{ 
				public static void Foo<TSource>(System.Span<TSource> t) { } 
			}
			""";
		var (type, compilation) = RefLikeArgTypeBuilderTests.GetTypeSymbolFromParameter(code);
		var model = new TypeReferenceModel(type, compilation);
		Assert.That(model.RefLikeArgProjectedEvaluationDelegateName, Is.EqualTo("ArgEvaluationForSpan<TSource>"));
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
		"global::Mock.ProjectionsForIMock.ArgEvaluationForintPointer")]
	[TestCase(
		"""
		using System;
		
		namespace Mock 
		{ 
			public interface IMock 
			{ 
				void Foo(Span<int> t);
			} 
		}
		""",
		"global::Mock.ProjectionsForIMock.ArgEvaluationForSpanOfint")]
	public static void GetProjectedEvaluationDelegateFullyQualifiedName(string code, string expectedValue)
	{
		var (typeToMock, type, compilation, model) = RefLikeArgTypeBuilderTests.GetTypeSymbols(code);
		var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }"));
		var name = RefLikeArgTypeBuilder.GetProjectedEvaluationDelegateFullyQualifiedName(
			new TypeReferenceModel(type, compilation), MockModel.Create(invocation, typeToMock, model, BuildType.Create, true)!.Type!);
		Assert.That(name, Is.EqualTo(expectedValue));
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