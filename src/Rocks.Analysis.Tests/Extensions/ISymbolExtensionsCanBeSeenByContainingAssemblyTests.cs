using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ISymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static async Task CallWhenSymbolIsPublicAsync()
	{
		var code =
			"""
			public class Source
			{
				public void Foo() { }
			}
			""";
		var (symbol, compilation) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenSymbolIsProtectedAsync()
	{
		var code =
			"""
			public class Source
			{
				protected void Foo() { }
			}
			""";
		var (symbol, compilation) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenSymbolIsProtectedInternalAsync()
	{
		var code =
			"""
			public class Source
			{
				protected internal void Foo() { }
			}
			""";
		var (symbol, compilation) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsInternalAndContainingAssemblyEqualsInvocationAssemblyAsync()
	{
		var code =
			"""
			public class Source
			{
				internal void Foo() { }
			}
			""";
		var (symbol, compilation) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsInternalAndContainingAssemblyExposesToInvocationAssemblyAsync()
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
		var (symbol, _) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly, containingCompilation), Is.True);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsInternalAndContainingAssemblyDoesNotExposeToInvocationAssemblyAsync()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			public class Source
			{
				internal void Foo() { }
			}
			""";
		var (symbol, _) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly, containingCompilation), Is.False);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsPrivateProtectedAndContainingAssemblyEqualsInvocationAssemblyAsync()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			$$"""
			public class Source
			{
				private protected void Foo() { }
			}
			""";
		var (symbol, compilation) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsPrivateProtectedAndContainingAssemblyExposesToInvocationAssemblyAsync()
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
		var (symbol, _) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly, containingCompilation), Is.True);
		}
	}

	[Test]
	public static async Task CallWhenSymbolIsPrivateProtectedAndContainingAssemblyDoesNotExposeToInvocationAssemblyAsync()
	{
		const string ContainingAssembly = nameof(ContainingAssembly);
		var code =
			"""
			public class Source
			{
				private protected void Foo() { }
			}
			""";
		var (symbol, _) = await ISymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		var containingSyntaxTree = CSharpSyntaxTree.ParseText("public class Containing { }");
		var containingCompilation = CSharpCompilation.Create(ContainingAssembly, [containingSyntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.CanBeSeenByContainingAssembly(containingCompilation.Assembly, containingCompilation), Is.False);
		}
	}

	private static async Task<(ISymbol, Compilation)> GetSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}