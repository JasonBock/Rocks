using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Diagnostics;
using System.Globalization;

namespace Rocks.Tests.Diagnostics;

public static class TypeIsClosedGenericDiagnosticTests
{
	[Test]
	public static void Create()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText("public class X { }");
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();

		var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression("public static void Foo() { }"));

		var descriptor = TypeIsClosedGenericDiagnostic.Create(invocation, model.GetDeclaredSymbol(typeSyntax)!);

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.GetMessage(CultureInfo.InvariantCulture), Is.EqualTo("The type X is a closed generic"));
			Assert.That(descriptor.Descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(TypeIsClosedGenericDescriptor.Title));
			Assert.That(descriptor.Id, Is.EqualTo(TypeIsClosedGenericDescriptor.Id));
			Assert.That(descriptor.Severity, Is.EqualTo(DiagnosticSeverity.Error));
		});
	}
}