using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders.Create;

namespace Rocks.Tests.Builders.Create;

public static class VariableNamingContextTests
{
	[Test]
	public static void AddWhenThereAreNoParameters()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo() { } }");
		var namingContext = new VariableNamingContext(method);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsNoMatchInParametersOrVariables()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var namingContext = new VariableNamingContext(method);
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereIsMatchInParameters()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var namingContext = new VariableNamingContext(method);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a1"));
	}

	[Test]
	public static void AddWhenVariableCurrentlyExists()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a) { } }");
		var namingContext = new VariableNamingContext(method);
		_ = namingContext["b"];
		var variable = namingContext["b"];
		Assert.That(variable, Is.EqualTo("b"));
	}

	[Test]
	public static void AddWhenThereAreMultipleMatchesInParameters()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a, int a1) { } }");
		var namingContext = new VariableNamingContext(method);
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a2"));
	}

	[Test]
	public static void AddWhenThereAreMultipleMatchesInParametersAndVariaables()
	{
		var method = VariableNamingContextTests.GetMethod("public class Method { public void Foo(int a, int a1) { } }");
		var namingContext = new VariableNamingContext(method);
		_ = namingContext["a2"];
		_ = namingContext["a3"];
		var variable = namingContext["a"];
		Assert.That(variable, Is.EqualTo("a4"));
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