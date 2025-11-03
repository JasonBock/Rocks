using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class PropertyModelTests
{
	[Test]
	public static void Create()
	{
		var code =
			"""
			public class Target
			{
				public string Value { get; set; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.GetAndSet, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.Accessors, Is.EqualTo(PropertyAccessor.GetAndSet));
			Assert.That(model.AllAttributesDescription, Is.Empty);
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.ContainingType.FullyQualifiedName, Is.EqualTo("global::Target"));
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.GetMethod, Is.Not.Null);
			Assert.That(model.InitCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.IsAbstract, Is.False);
			Assert.That(model.IsIndexer, Is.False);
			Assert.That(model.IsUnsafe, Is.False);
			Assert.That(model.IsVirtual, Is.False);
			Assert.That(model.MemberIdentifier, Is.EqualTo(memberIdentifier));
			Assert.That(model.MockType, Is.SameAs(mockType));
			Assert.That(model.Name, Is.EqualTo("Value"));
			Assert.That(model.OverridingCodeValue, Is.EqualTo("public"));
			Assert.That(model.Parameters, Is.Empty);
			Assert.That(model.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(model.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(model.ReturnsByRef, Is.False);
			Assert.That(model.ReturnsByRefReadOnly, Is.False);
			Assert.That(model.SetCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.SetMethod, Is.Not.Null);
			Assert.That(model.Type.FullyQualifiedName, Is.EqualTo("string"));
		}
	}

	[Test]
	public static void CreateWithGetOnly()
	{
		var code =
			"""
			public class Target
			{
				public string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.GetMethod, Is.Not.Null);
			Assert.That(model.InitCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetMethod, Is.Null);
		}
	}

	[Test]
	public static void CreateWithInitOnly()
	{
		var code =
			"""
			public class Target
			{
				public string Value { init; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Init, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.GetMethod, Is.Null);
			Assert.That(model.InitCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.SetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetMethod, Is.Not.Null);
		}
	}

	[Test]
	public static void CreateWithSetOnly()
	{
		var code =
			"""
			public class Target
			{
				public string Value { set; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Set, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.GetMethod, Is.Null);
			Assert.That(model.InitCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.SetMethod, Is.Not.Null);
		}
	}

	[Test]
	public static void CreateWithVirtual()
	{
		var code =
			"""
			public class Target
			{
				public virtual string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.IsVirtual, Is.True);
	}

	[Test]
	public static void CreateWithAbstract()
	{
		var code =
			"""
			public abstract class Target
			{
				public abstract string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.IsAbstract, Is.True);
	}

	[Test]
	public static void CreateWithIndexer()
	{
		var code =
			"""
			public abstract class Target
			{
				public abstract string this[string data] { get; }
			}
			""";
		(var indexer, var type, var modelContext) = PropertyModelTests.GetSymbolsForIndexerCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(indexer, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.IsIndexer, Is.True);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("data"));
		}
	}

	[Test]
	public static void CreateWithUnsafe()
	{
		var code =
			"""
			public class Target
			{
				public unsafe int* Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.IsUnsafe, Is.True);
	}

	[Test]
	public static void CreateWithReturnsByRef()
	{
		var code =
			"""
			public class Target
			{
				public ref int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.ReturnsByRef, Is.True);
	}

	[Test]
	public static void CreateWithReturnsByRefReadOnly()
	{
		var code =
			"""
			public class Target
			{
				public ref readonly int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.ReturnsByRefReadOnly, Is.True);
	}

	[Test]
	public static void CreateWithExplicitInterfaceImplementation()
	{
		var code =
			"""
			public class Target
			{
				public int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No,
			 PropertyAccessor.Get, memberIdentifier);

		Assert.That(model.OverridingCodeValue, Is.Null);
	}

	[Test]
	public static void CreateWithAttributes()
	{
		var code =
			"""
			using System;
			using System.Diagnostics.CodeAnalysis;
			
			public class Target
			{
				[CLSCompliant(true)]
				[AllowNull]
				public string Value { get; set; }
			}
			""";
		(var property, var type, var modelContext) = PropertyModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.GetAndSet, memberIdentifier);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true), global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]"));
			Assert.That(model.AllAttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true), global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]"));
		}
	}

	private static (IPropertySymbol, ITypeSymbol, ModelContext) GetSymbolsCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<PropertyDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(propertySyntax)!, model.GetDeclaredSymbol(typeSyntax)!, new(model));
	}

	private static (IPropertySymbol, ITypeSymbol, ModelContext) GetSymbolsForIndexerCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var indexerSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<IndexerDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(indexerSyntax)!, model.GetDeclaredSymbol(typeSyntax)!, new(model));
	}
}