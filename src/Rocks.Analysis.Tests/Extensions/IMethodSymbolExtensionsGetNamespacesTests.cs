using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsGetNamespacesTests
{
	[Test]
	public static void GetNamespaces()
	{
		var methodAttributeNamespace = "NSMethod";
		var parameterTypeNamespace = "NS.Multiple.Values";
		var parameterAttributeNamespace = "NSParameter";
		var returnAttribute = "NSReturn";

		var code =
			$$"""
			using System;
			using {{methodAttributeNamespace}};
			using {{returnAttribute}};
			using {{parameterTypeNamespace}};
			using {{parameterAttributeNamespace}};

			namespace {{parameterTypeNamespace}}
			{
				public sealed class ParameterType { }
			}

			namespace {{parameterAttributeNamespace}}
			{
				[AttributeUsage(AttributeTargets.Parameter)]
				public sealed class ParameterAttribute
					: Attribute
				{ }
			}

			namespace {{methodAttributeNamespace}}
			{
				[AttributeUsage(AttributeTargets.Method)]
				public sealed class MethodAttribute
					: Attribute
				{ }
			}

			namespace {{returnAttribute}}
			{
				[AttributeUsage(AttributeTargets.ReturnValue)]
				public sealed class ReturnAttribute
					: Attribute
				{ }
			}

			public class NamespaceClass
			{
				[Method]
				[return: Return]
				public string Foo([Parameter] ParameterType p) => string.Empty;
			}
			""";

		Assert.Multiple(() =>
		{
			var method = IMethodSymbolExtensionsGetNamespacesTests.GetMethod(code);
			var namespaces = method.GetNamespaces();

			Assert.That(namespaces, Has.Count.EqualTo(5));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(string).Namespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == methodAttributeNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == returnAttribute), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == parameterTypeNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == parameterAttributeNamespace), Is.True);
		});
	}

	private static IMethodSymbol GetMethod(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return model.GetDeclaredSymbol(syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!;
	}
}