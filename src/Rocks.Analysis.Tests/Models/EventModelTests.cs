using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class EventModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			public class Target
			{
				public event EventHandler? Test;
			}
			""";

		(var @event, var modelContext) = await EventModelTests.GetSymbolsCompilationAsync(code);
		var model = new EventModel(@event, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.ArgsType, Is.EqualTo("global::System.EventArgs"));
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.ContainingType.FullyQualifiedName, Is.EqualTo("global::Target"));
			Assert.That(model.Name, Is.EqualTo("Test"));
			Assert.That(model.OverridingCodeValue, Is.EqualTo("public"));
			Assert.That(model.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(model.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(model.Type.FullyQualifiedName, Is.EqualTo("EventHandler?"));
		}
	}

	[Test]
	public static async Task CreateWithRequiresExplicitInterfaceImplementationAsync()
	{
		var code =
			"""
			public class Target
			{
				public event EventHandler? Test;
			}
			""";

		(var @event, var modelContext) = await EventModelTests.GetSymbolsCompilationAsync(code);
		var model = new EventModel(@event, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.OverridingCodeValue, Is.Null);
			Assert.That(model.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
		}
	}

	[Test]
	public static async Task CreateWithAttributesAsync()
	{
		var code =
			"""
			using System;

			public class Target
			{
				[CLSCompliant(true)]
				public event EventHandler? Test;
			}
			""";

		(var @event, var modelContext) = await EventModelTests.GetSymbolsCompilationAsync(code);
		var model = new EventModel(@event, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No);

		Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true)]"));
	}

	[Test]
	public static async Task CreateWithArgsTypeAsync()
	{
		var code =
			"""
			using System;

			public class CustomArgs
				: EventArgs { }

			public class Target
			{
				event EventHandler<CustomArgs> Test;
			}			
			""";

		(var @event, var modelContext) = await EventModelTests.GetSymbolsCompilationAsync(code);
		var model = new EventModel(@event, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No);

		Assert.That(model.ArgsType, Is.EqualTo("global::CustomArgs"));
	}

	private static async Task<(IEventSymbol, ModelContext)> GetSymbolsCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == "Target");
		var type = model.GetDeclaredSymbol(typeSyntax)!;
		return (type.GetMembers().OfType<IEventSymbol>().Single(), new(model));
	}
}