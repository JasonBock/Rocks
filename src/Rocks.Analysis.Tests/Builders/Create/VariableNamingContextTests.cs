using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Builders.Create;
using Rocks.Analysis.Models;
using System.Collections.Immutable;

namespace Rocks.Analysis.Tests.Builders.Create;

public static class VariableNamingContextTests
{
	[Test]
	public static void ReretrieveVariableName()
	{
		var namingContext = new VariablesNamingContext(["self", "expectations"]);
		var variable = namingContext["expectations"];
		var reretrieveVariable = namingContext["expectations"];

		using (Assert.EnterMultipleScope())
		{
			Assert.That(variable, Is.EqualTo("expectations1"), nameof(variable));
			Assert.That(reretrieveVariable, Is.EqualTo("expectations1"), nameof(reretrieveVariable));
		}
	}

	[Test]
	public static void AddWhenThereAreNoNames()
	{
		var namingContext = new VariablesNamingContext(ImmutableHashSet<string>.Empty);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsNoMatchWithNames()
	{
		var namingContext = new VariablesNamingContext(["a"]);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsMatchWithNames()
	{
		var namingContext = new VariablesNamingContext(["a"]);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a1"));
	}

	[Test]
	public static async Task AddWhenThereAreNoParametersAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo() { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static async Task AddWhenThereIsNoMatchInParametersOrVariablesAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static async Task AddWhenThereIsMatchInParametersAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a1"));
	}

	[Test]
	public static async Task AddWhenVariableCurrentlyExistsAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		_ = namingContext["b"];
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static async Task AddWhenThereAreMultipleMatchesInParametersAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo(int a, int a1) { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a2"));
	}

	[Test]
	public static async Task AddWhenThereAreMultipleMatchesInParametersAndVariablesAsync()
	{
		(var method, var modelContext) = await VariableNamingContextTests.GetMethodAsync("public class Method { public void Foo(int a, int a1) { } }");
		var model = new MethodModel(method, modelContext.CreateTypeReference(method.ContainingType), modelContext,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariablesNamingContext(model);
		_ = namingContext["a2"];
		_ = namingContext["a3"];
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a4"));
	}

	private static async Task<(IMethodSymbol, ModelContext)> GetMethodAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return (model.GetDeclaredSymbol((await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!, new(model));
	}
}