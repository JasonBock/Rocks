﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IEventSymbolCanBeSeenByContainingAssemblyExtensionsTests
{
	[Test]
	public static void CallWhenEventCanBeSeen()
	{
		var code =
			"""
			using System;

			public class Source
			{
				public event EventHandler CustomEvent;
			}
			""";
		var (symbol, compilation) = IEventSymbolCanBeSeenByContainingAssemblyExtensionsTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static void CallWhenEventCannotSeen()
	{
		var code =
			"""
			using System;
			
			public class Source
			{
				private event EventHandler CustomEvent;
			}
			""";
		var (symbol, compilation) = IEventSymbolCanBeSeenByContainingAssemblyExtensionsTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	private static (IEventSymbol, Compilation) GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var typeSymbol = (ITypeSymbol)model.GetDeclaredSymbol(typeSyntax)!;

		return (typeSymbol.GetMembers().OfType<IEventSymbol>().Single(), compilation);
	}
}