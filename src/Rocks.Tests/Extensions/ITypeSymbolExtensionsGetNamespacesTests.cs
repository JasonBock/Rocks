using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeSymbolExtensionsGetNamespacesTests
{
	[Test]
	public static void GetNamespacesForNonGenericType()
	{
		var @namespace = "TargetNamespace";
		var code =
$@"namespace {@namespace}
{{
	public class Target {{ }}
}}";
		var typeSymbol = ITypeSymbolExtensionsGetNamespacesTests.GetTypeSymbol(code, "Target");
		var namespaces = typeSymbol.GetNamespaces();

		Assert.Multiple(() =>
		{
			Assert.That(namespaces, Has.Count.EqualTo(1));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == @namespace), Is.True);
		});
	}

	[Test]
	public static void GetNamespacesForGenericType()
	{
		var @namespace = "TargetNamespace";
		var innerNamespace = "Inner";
		var middleNamespace = "Middle";
		var outerNamespace = "Outer";
		var targetType = "Target";
		var theField = "TheField";

		var code =
$@"using {innerNamespace};
using {middleNamespace};
using {outerNamespace};

namespace {innerNamespace}
{{
	public class InnerType {{ }}
}}

namespace {middleNamespace}
{{
	public class MiddleType<T> {{ }}
}}

namespace {outerNamespace}
{{
	public class OuterType<MiddleType<T>> {{ }}
}}

namespace {@namespace}
{{
	public class {targetType} 
	{{ 
		public OuterType<MiddleType<InnerType>> {theField} {{ get; }}
	}}
}}";
		var typeSymbol = ITypeSymbolExtensionsGetNamespacesTests.GetTypeSymbol(code, targetType);
		var propertySymbol = typeSymbol.GetMembers().Single(_ => _.Name == theField) as IPropertySymbol;
		var namespaces = propertySymbol!.Type.GetNamespaces();

		Assert.Multiple(() =>
		{
			Assert.That(namespaces, Has.Count.EqualTo(3));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == outerNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == middleNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == innerNamespace), Is.True);
		});
	}

	private static ITypeSymbol GetTypeSymbol(string source, string typeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == typeName);
		return model.GetDeclaredSymbol(typeSyntax)!;
	}
}