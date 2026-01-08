using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeSymbolExtensionsGetNamespacesTests
{
	[Test]
	public static async Task GetNamespacesForNonGenericTypeAsync()
	{
		var @namespace = "TargetNamespace";
		var code =
			$$"""
			namespace {{@namespace}}
			{
				public class Target { }
			}
			""";
		var typeSymbol = await GetTypeSymbolAsync(code, "Target");
		var namespaces = typeSymbol.GetNamespaces();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(namespaces, Has.Count.EqualTo(1));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == @namespace), Is.True);
		}
	}

	[Test]
	public static async Task GetNamespacesForGenericTypeAsync()
	{
		var @namespace = "TargetNamespace";
		var innerNamespace = "Inner";
		var middleNamespace = "Middle";
		var outerNamespace = "Outer";
		var targetType = "Target";
		var theField = "TheField";

		var code =
			$$"""
			using {{innerNamespace}};
			using {{middleNamespace}};
			using {{outerNamespace}};

			namespace {{innerNamespace}}
			{
				public class InnerType { }
			}

			namespace {{middleNamespace}}
			{
				public class MiddleType<T> { }
			}

			namespace {{outerNamespace}}
			{
				public class OuterType<MiddleType<T>> { }
			}

			namespace {{@namespace}}
			{
				public class {{targetType}}
				{ 
					public OuterType<MiddleType<InnerType>> {{theField}} { get; }
				}
			}
			""";
		var typeSymbol = await GetTypeSymbolAsync(code, targetType);
		var propertySymbol = typeSymbol.GetMembers().Single(_ => _.Name == theField) as IPropertySymbol;
		var namespaces = propertySymbol!.Type.GetNamespaces();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(namespaces, Has.Count.EqualTo(3));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == outerNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == middleNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == innerNamespace), Is.True);
		}
	}

	private static async Task<ITypeSymbol> GetTypeSymbolAsync(string source, string typeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == typeName);
		return model.GetDeclaredSymbol(typeSyntax)!;
	}
}