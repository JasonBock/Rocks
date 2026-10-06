using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class PropertyModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			public class Target
			{
				public string Value { get; set; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.GetAndSet, false, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.Accessors, Is.EqualTo(PropertyAccessor.GetAndSet));
			Assert.That(model.AllAttributesDescription, Is.Empty);
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.ContainingType.FullyQualifiedName, Is.EqualTo("global::Target"));
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.True);
			Assert.That(model.GetMethod, Is.Not.Null);
			Assert.That(model.IsInaccessibleAbstractMember, Is.False);
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
	public static async Task CreateWithInaccessibleAbstractPropertyGetSetAsync()
	{
		var source =
			"""
			public abstract class Holder
			{
				protected Holder() { }

				protected abstract Data Value { get; set; }

				protected internal class Data { }
			}
			""";

		var sourceReferences = Shared.References.Value
			.Cast<MetadataReference>();
		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source);
		var sourceCompilation = CSharpCompilation.Create("Source", [sourceSyntaxTree],
			sourceReferences,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var sourceReference = sourceCompilation.ToMetadataReference()!;

		var code =
			"""
			public abstract class DerivingHolder
				: Holder { }
			""";

		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, [sourceCompilation.ToMetadataReference()]);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.GetAndSet, true, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.IsInaccessibleAbstractMember, Is.True);
			Assert.That(model.GetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.GetMethod, Is.Not.Null);
			Assert.That(model.InitCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetCanBeSeenByContainingAssembly, Is.False);
			Assert.That(model.SetMethod, Is.Not.Null);
		}
	}

	[Test]
	public static async Task CreateWithGetOnlyAsync()
	{
		var code =
			"""
			public class Target
			{
				public string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

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
	public static async Task CreateWithInitOnlyAsync()
	{
		var code =
			"""
			public class Target
			{
				public string Value { init; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Init, false, memberIdentifier);

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
	public static async Task CreateWithSetOnlyAsync()
	{
		var code =
			"""
			public class Target
			{
				public string Value { set; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Set, false, memberIdentifier);

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
	public static async Task CreateWithVirtualAsync()
	{
		var code =
			"""
			public class Target
			{
				public virtual string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.IsVirtual, Is.True);
	}

	[Test]
	public static async Task CreateWithAbstractAsync()
	{
		var code =
			"""
			public abstract class Target
			{
				public abstract string Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.IsAbstract, Is.True);
	}

	[Test]
	public static async Task CreateWithIndexerAsync()
	{
		var code =
			"""
			public abstract class Target
			{
				public abstract string this[string data] { get; }
			}
			""";
		(var indexer, var type, var modelContext) = await GetSymbolsForIndexerCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(indexer, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.IsIndexer, Is.True);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("data"));
		}
	}

	[Test]
	public static async Task CreateWithUnsafeAsync()
	{
		var code =
			"""
			public class Target
			{
				public unsafe int* Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.IsUnsafe, Is.True);
	}

	[Test]
	public static async Task CreateWithReturnsByRefAsync()
	{
		var code =
			"""
			public class Target
			{
				public ref int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.ReturnsByRef, Is.True);
	}

	[Test]
	public static async Task CreateWithReturnsByRefReadOnlyAsync()
	{
		var code =
			"""
			public class Target
			{
				public ref readonly int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.ReturnsByRefReadOnly, Is.True);
	}

	[Test]
	public static async Task CreateWithExplicitInterfaceImplementationAsync()
	{
		var code =
			"""
			public class Target
			{
				public int Value { get; }
			}
			""";
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No,
			 PropertyAccessor.Get, false, memberIdentifier);

		Assert.That(model.OverridingCodeValue, Is.Null);
	}

	[Test]
	public static async Task CreateWithAttributesAsync()
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
		(var property, var type, var modelContext) = await GetSymbolsCompilationAsync(code, []);
		var mockType = modelContext.CreateTypeReference(type);
		const uint memberIdentifier = 1;
		var model = new PropertyModel(property, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No,
			 PropertyAccessor.GetAndSet, false, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true), global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]"));
			Assert.That(model.AllAttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true), global::System.Diagnostics.CodeAnalysis.AllowNullAttribute]"));
		}
	}

	private static async Task<(IPropertySymbol, ITypeSymbol, ModelContext)> GetSymbolsCompilationAsync(
		string code, IEnumerable<MetadataReference> additionalReferences)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value.Concat(additionalReferences), new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);
		var root = await syntaxTree.GetRootAsync();
		var typeSyntax = root.DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
		var typeProperties = typeSymbol.GetMembers().OfType<IPropertySymbol>().ToArray();
		var propertySymbol = 
			typeProperties.Length > 0 ? 
				typeProperties[0] :
				typeSymbol.BaseType!.GetMembers().OfType<IPropertySymbol>().ToArray()[0];
		return (propertySymbol, typeSymbol, new(model));
	}

	private static async Task<(IPropertySymbol, ITypeSymbol, ModelContext)> GetSymbolsForIndexerCompilationAsync(
		string code, IEnumerable<MetadataReference> additionalReferences)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value.Concat(additionalReferences), new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);
		var root = await syntaxTree.GetRootAsync();
		var typeSyntax = root.DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var typeSymbol = model.GetDeclaredSymbol(typeSyntax)!;
		var typeProperties = typeSymbol.GetMembers().OfType<IPropertySymbol>().ToArray();
		var indexerSymbol =
			typeProperties.Length > 0 ?
				typeProperties[0] :
				typeSymbol.BaseType!.GetMembers().OfType<IPropertySymbol>().ToArray()[0];
		return (indexerSymbol, typeSymbol, new(model));
	}
}