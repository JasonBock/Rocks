using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NS.Multiple.Values;
using NSMethod;
using NSParameter;
using NSReturn;
using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Linq;

namespace NS.Multiple.Values
{
	public sealed class ParameterType { }
}

namespace NSParameter
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class ParameterAttribute
		: Attribute
	{ }
}

namespace NSMethod
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MethodAttribute
		: Attribute
	{ }
}

namespace NSReturn
{
	[AttributeUsage(AttributeTargets.ReturnValue)]
	public sealed class ReturnAttribute
		: Attribute
	{ }
}

namespace Rocks.Tests.Extensions
{
	public static class IMethodSymbolExtensionsGetNamespacesTests
	{
		[Test]
		public static void GetNamespaces()
		{
			var code =
$@"using System;
using {typeof(MethodAttribute).Namespace};
using {typeof(ReturnAttribute).Namespace};
using {typeof(ParameterType).Namespace};
using {typeof(ParameterAttribute).Namespace};

public class NamespaceClass
{{
	[Method]
	[return: Return]
	public string Foo([Parameter] ParameterType p) => string.Empty;
}}";

			Assert.Multiple(() =>
			{
				var method = IMethodSymbolExtensionsGetNamespacesTests.GetMethod(code);
				var namespaces = method.GetNamespaces();

				Assert.That(namespaces.Count, Is.EqualTo(5));
				Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(string).Namespace), Is.True);
				Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(MethodAttribute).Namespace), Is.True);
				Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(ReturnAttribute).Namespace), Is.True);
				Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(ParameterType).Namespace), Is.True);
				Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(ParameterAttribute).Namespace), Is.True);
			});
		}

		private static IMethodSymbol GetMethod(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location));
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			return model.GetDeclaredSymbol(syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single())!;
		}
	}
}