using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class TypeReferenceModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target { }
			""";
		(var type, var modelContext) = await GetSymbolAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.FlattenedName, Is.EqualTo("Target"));
			Assert.That(model.FullyQualifiedName, Is.EqualTo("global::TargetNamespace.Target"));
			Assert.That(model.IsBasedOnTypeParameter, Is.False);
			Assert.That(model.IsPointer, Is.False);
			Assert.That(model.IsRecord, Is.False);
			Assert.That(model.IsReferenceType, Is.True);
			Assert.That(model.IsRefLikeType, Is.False);
			Assert.That(model.Namespace, Is.EqualTo("TargetNamespace"));
			Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.None));
			Assert.That(model.TypeKind, Is.EqualTo(TypeKind.Class));
		}
	}

	[Test]
	public static async Task CreateWithAttributesNotObsoleteAsync()
	{
		var code =
			"""
			using System;
			
			namespace TargetNamespace; 
			
			[Serializable]
			public class Target { }
			""";
		(var type, var modelContext) = await GetSymbolAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.AttributesDescription, Is.Empty);
	}

	[Test]
	public static async Task CreateWithObsoleteAttributesAsync()
	{
		var code =
			"""
			using System;
			
			namespace TargetNamespace; 
			
			[Obsolete("old")]
			public class Target { }
			""";
		(var type, var modelContext) = await GetSymbolAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.AttributesDescription, Is.EqualTo("""[type: global::System.ObsoleteAttribute("old")]"""));
	}

	[Test]
	public static async Task CreateWithGenericsAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target<T> { }
			""";
		(var type, var modelContext) = await GetSymbolAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.FlattenedName, Is.EqualTo("Target"));
		}
	}

	[Test]
	public static async Task CreateBasedOnTypeParameterAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class TargetUsage
			{
				public void Go<T>(T target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.IsBasedOnTypeParameter, Is.True);
			Assert.That(model.RequiresProjectedArgument, Is.False);
		}
	}

	[Test]
	public static async Task CreateWithNullableAnnotationsAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target { }

			public class TargetUsage
			{
				public void Go(Target? target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.Annotated));
			Assert.That(model.RequiresProjectedArgument, Is.False);
		}
	}

	[Test]
	public static async Task CreateWithTypeThatDoesNotNeedProjectionAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target
			{
				public unsafe void Go(string target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.RequiresProjectedArgument, Is.False);
	}

	[Test]
	public static async Task CreateWithPointerAsync()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target
			{
				public unsafe void Go(int* target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.RequiresProjectedArgument, Is.True);
			Assert.That(model.IsPointer, Is.True);
		}
	}

	public static async Task CreateWithArgIteratorAsync()
	{
		var code =
			"""
			using System;

			namespace TargetNamespace; 
			
			public class Target
			{
				public void Go(ArgIterator target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.RequiresProjectedArgument, Is.True);
	}

	public static async Task CreateWithRuntimeArgumentHandleAsync()
	{
		var code =
			"""
			using System;

			namespace TargetNamespace; 
			
			public class Target
			{
				public void Go(RuntimeArgumentHandle target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.RequiresProjectedArgument, Is.True);
	}

	public static async Task CreateWithTypedReferenceAsync()
	{
		var code =
			"""
			using System;

			namespace TargetNamespace; 
			
			public class Target
			{
				public void Go(TypedReference target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.RequiresProjectedArgument, Is.True);
	}

	[Test]
	public static async Task CreateWithRefLikeTypeAsync()
	{
		var code =
			"""
			using System;

			namespace TargetNamespace; 
			
			public class Target
			{
				public void Go(Span<int> target) { }
			}
			""";
		(var type, var modelContext) = await GetSymbolReferenceAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.RequiresProjectedArgument, Is.False);
			Assert.That(model.IsRefLikeType, Is.True);
		}
	}

	[Test]
	public static async Task CreateWithRecordAsync()
	{
		var code =
			"""
			public record Target;
			""";
		(var type, var modelContext) = await GetSymbolAndCompilationAsync(code);
		var model = modelContext.CreateTypeReference(type);

		Assert.That(model.IsRecord, Is.True);
	}

	private static async Task<(ITypeSymbol, ModelContext)> GetSymbolAndCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, new(model));
	}

	private static async Task<(ITypeSymbol, ModelContext)> GetSymbolReferenceAndCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, new(model));
	}
}