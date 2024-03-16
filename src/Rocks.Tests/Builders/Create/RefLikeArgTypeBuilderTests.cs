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
		Assert.That(model.RefLikeArgProjectedName, Is.EqualTo("ArgumentForSpanOfint"));
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
		Assert.That(name, Is.EqualTo("global::Mock.IMockCreateExpectations.Projections.ArgumentForSpanOfint"));
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
		Assert.That(model.RefLikeArgProjectedEvaluationDelegateName, Is.EqualTo("ArgumentEvaluationForSpan<TSource>"));
	}

	private static (ITypeSymbol typeToMock, ITypeSymbol type, Compilation compilation, SemanticModel model) GetTypeSymbols(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
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
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}