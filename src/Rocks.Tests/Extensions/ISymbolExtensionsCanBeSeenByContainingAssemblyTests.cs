using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ISymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static void CallWhenSymbolIsPublic()
	{
		var code =
			"""
			public class Source
			{
				public void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenSymbolIsProtected()
	{
		var code =
			"""
			public class Source
			{
				protected void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenSymbolIsProtectedInternal()
	{
		var code =
			"""
			public class Source
			{
				protected internal void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
		});
	}

	[Test]
	public static void CallWhenSymbolIsInternalAndContainingAssemblyEqualsInvocationAssembly()
	{
		var code =
			"""
			public class Source
			{
				internal void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
		});
	}

	[Test]
	public static void CallWhenSymbolIsInternalAndContainingAssemblyExposesToInvocationAssembly()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("{{ContainingAssembly}}")]

			public class Source
			{
				internal void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly), Is.True);
		});
	}

	[Test]
	public static void CallWhenSymbolIsInternalAndContainingAssemblyDoesNotExposeToInvocationAssembly()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			public class Source
			{
				internal void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly), Is.False);
		});
	}

	[Test]
	public static void CallWhenSymbolIsPrivateProtectedAndContainingAssemblyEqualsInvocationAssembly()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			public class Source
			{
				private protected void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
		});
	}

	[Test]
	public static void CallWhenSymbolIsPrivateProtectedAndContainingAssemblyExposesToInvocationAssembly()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("{{ContainingAssembly}}")]

			public class Source
			{
				private protected void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly), Is.True);
		});
	}

	[Test]
	public static void CallWhenSymbolIsPrivateProtectedAndContainingAssemblyDoesNotExposeToInvocationAssembly()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			"""
			public class Source
			{
				private protected void Foo() { }
			}
			""";
		var symbol = ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.Multiple(() =>
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly), Is.False);
		});
	}

	private static ISymbol GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}