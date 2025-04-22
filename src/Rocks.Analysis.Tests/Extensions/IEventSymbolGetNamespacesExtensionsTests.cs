using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IEventSymbolGetNamespacesExtensionsTests
{
	[Test]
	public static void GetNamespaces()
	{
		var customEventArgNamespace = "CustomEventArgs";
		var attributeNamespace = "IEventSymbolExtensionsTestsNamespace";
		var typeName = "EventClass";

		var code =
			$$"""
			using System;
			using {{customEventArgNamespace}};
			using {{attributeNamespace}};

			namespace {{attributeNamespace}}
			{
				[AttributeUsage(AttributeTargets.Event)]
				public sealed class EventAttribute
					: Attribute
				{ }
			}

			namespace {{customEventArgNamespace}}
			{
				public sealed class ACustomEventArgs
					: EventArgs
				{ }
			}

			public class {{typeName}}
			{
				[Event]
				public event EventHandler<ACustomEventArgs> CustomEvent;
			}
			""";

		Assert.Multiple(() =>
		{
			var @event = IEventSymbolGetNamespacesExtensionsTests.GetEvent(code, typeName);
			var namespaces = @event.GetNamespaces();

			Assert.That(namespaces, Has.Count.EqualTo(3));
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == typeof(EventHandler).Namespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == attributeNamespace), Is.True);
			Assert.That(namespaces.Any(_ => _.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) == customEventArgNamespace), Is.True);
		});
	}

	private static IEventSymbol GetEvent(string source, string typeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == typeName);
		var typeSymbol = (ITypeSymbol)model.GetDeclaredSymbol(typeSyntax)!;

		return typeSymbol.GetMembers().OfType<IEventSymbol>().Single();
	}
}