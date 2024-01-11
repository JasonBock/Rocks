using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Tests.Builders.Create;

public static class VariableNamingContextTests
{
	[Test]
	public static void ReretrieveVariableName()
	{
		var namingContext = new VariableNamingContext(["self", "expectations"]);
		var variable = namingContext["expectations"];
		var reretrieveVariable = namingContext["expectations"];

		Assert.Multiple(() =>
		{
			Assert.That(variable, Is.EqualTo("expectations1"), nameof(variable));
			Assert.That(reretrieveVariable, Is.EqualTo("expectations1"), nameof(reretrieveVariable));
		});
	}

	[Test]
	public static void AddWhenThereAreNoNames()
	{
		var namingContext = new VariableNamingContext(ImmutableHashSet<string>.Empty);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsNoMatchWithNames()
	{
		var namingContext = new VariableNamingContext(["a"]);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsMatchWithNames()
	{
		var namingContext = new VariableNamingContext(["a"]);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a1"));
	}

	[Test]
	public static void AddWhenThereAreNoParameters()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo() { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsNoMatchInParametersOrVariables()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsMatchInParameters()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a1"));
	}

	[Test]
	public static void AddWhenVariableCurrentlyExists()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		_ = namingContext["b"];
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereAreMultipleMatchesInParameters()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a, int a1) { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a2"));
	}

	[Test]
	public static void AddWhenThereAreMultipleMatchesInParametersAndVariables()
	{
		(var method, var compilation) = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a, int a1) { } }");
		var model = new MethodModel(method, new TypeReferenceModel(method.ContainingType, compilation), compilation,
			RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, 1);

		var namingContext = new VariableNamingContext(model);
		_ = namingContext["a2"];
		_ = namingContext["a3"];
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a4"));
	}

	private static (IMethodSymbol, Compilation) GetMethod(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		return (model.GetDeclaredSymbol(syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single())!, compilation);
	}
}