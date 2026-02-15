using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

internal static class SyntaxNodeExtensionsTests
{
	[Test]
	public static async Task FindParentWhenParentExistsAndIsTargetAsync()
	{
		var code =
			"""
			public static class Tester
			{
				public static void Test() =>
					Tester.Do("5");

				public static void Do(string value) { }
			}
			""";

		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("test", [syntaxTree],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var argumentNode = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true).OfType<ArgumentSyntax>().Single();
		var parent = argumentNode.FindParent<InvocationExpressionSyntax>(_ => true);
		var model = compilation.GetSemanticModel(syntaxTree);
		var invocationMethod = model.GetSymbolInfo(parent!).Symbol as IMethodSymbol;
		Assert.That(invocationMethod!.Name, Is.EqualTo("Do"));
	}

	[Test]
	public static async Task FindParentWhenParentExistsAndIsNotTargetAsync()
	{
		var code =
			"""
			public static class Tester
			{
				public static void Test() =>
					Tester.Do("5");

				public static void Do(string value) { }
			}
			""";

		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("test", [syntaxTree],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var argumentNode = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true).OfType<ArgumentSyntax>().Single();
		var parent = argumentNode.FindParent<InvocationExpressionSyntax>(_ => false);
		Assert.That(parent, Is.Null);
	}

	[Test]
	public static async Task FindParentWhenParentDoesNotExistAsync()
	{
		var code =
			"""
			public static class Tester
			{
				public static void Test() =>
					Tester.Do("5");

				public static void Do(string value) { }
			}
			""";

		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("test", [syntaxTree],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var argumentNode = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true).OfType<ArgumentSyntax>().Single();
		var parent = argumentNode.FindParent<AttributeListSyntax>(_ => true);
		Assert.That(parent, Is.Null);
	}
}