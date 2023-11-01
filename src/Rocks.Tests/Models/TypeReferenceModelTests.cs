using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

public static class TypeReferenceModelTests
{
	[Test]
	public static void Create()
	{
		var code = 
			"""
			namespace TargetNamespace; 
			
			public class Target { }
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.FlattenedName, Is.EqualTo("Target"));
			Assert.That(model.FullyQualifiedName, Is.EqualTo("global::TargetNamespace.Target"));
			Assert.That(model.IncludeGenericsName, Is.EqualTo("Target"));
			Assert.That(model.IsEsoteric, Is.False);
			Assert.That(model.IsPointer, Is.False);
			Assert.That(model.IsRecord, Is.False);
			Assert.That(model.IsReferenceType, Is.True);
			Assert.That(model.IsRefLikeType, Is.False);
			Assert.That(model.Kind, Is.EqualTo(SymbolKind.NamedType));
			Assert.That(model.Namespace, Is.EqualTo("TargetNamespace"));
			Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.None));
			Assert.That(model.PointerArgProjectedEvaluationDelegateName, Is.Null);
			Assert.That(model.PointerArgProjectedName, Is.Null);
			Assert.That(model.RefLikeArgProjectedEvaluationDelegateName, Is.Null);
			Assert.That(model.RefLikeArgProjectedName, Is.Null);
			Assert.That(model.RefLikeArgConstructorProjectedName, Is.Null);
			Assert.That(model.TypeKind, Is.EqualTo(TypeKind.Class));
		});
	}

	[Test]
	public static void CreateWithAttributesNotObsolete()
	{
		var code =
			"""
			using System;
			
			namespace TargetNamespace; 
			
			[Serializable]
			public class Target { }
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.That(model.AttributesDescription, Is.Empty);
	}

	[Test]
	public static void CreateWithObsoleteAttributes()
	{
		var code =
			"""
			using System;
			
			namespace TargetNamespace; 
			
			[Obsolete("old")]
			public class Target { }
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.That(model.AttributesDescription, Is.EqualTo("""[type: global::System.ObsoleteAttribute("old")]"""));
	}

	[Test]
	public static void CreateWithGenerics()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target<T> { }
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.FlattenedName, Is.EqualTo("TargetOfT"));
			Assert.That(model.IncludeGenericsName, Is.EqualTo("Target<T>"));
			Assert.That(model.NoGenericsName, Is.EqualTo("Target"));
		});
	}

	[Test]
	public static void CreateWithNullableAnnotations()
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
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolReferenceAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.That(model.NullableAnnotation, Is.EqualTo(NullableAnnotation.Annotated));
	}

	[Test]
	public static void CreateWithPointer()
	{
		var code =
			"""
			namespace TargetNamespace; 
			
			public class Target
			{
				public unsafe void Go(int* target) { }
			}
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolReferenceAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.IsEsoteric, Is.True);
			Assert.That(model.IsPointer, Is.True);
			Assert.That(model.PointerArgProjectedEvaluationDelegateName, Is.EqualTo("ArgumentEvaluationForintPointer"));
			Assert.That(model.PointerArgProjectedName, Is.EqualTo("ArgumentForintPointer"));
			Assert.That(model.RefLikeArgProjectedEvaluationDelegateName, Is.EqualTo("ArgEvaluationForintPointer"));
			Assert.That(model.RefLikeArgProjectedName, Is.EqualTo("ArgForintPointer"));
			Assert.That(model.RefLikeArgConstructorProjectedName, Is.EqualTo("ArgForintPointer"));
		});
	}

	[Test]
	public static void CreateWithRefLikeType()
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
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolReferenceAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.That(model.IsRefLikeType, Is.True);
	}

	[Test]
	public static void CreateWithRecord()
	{
		var code =
			"""
			public record Target;
			""";
		(var type, var compilation) = TypeReferenceModelTests.GetSymbolAndCompilation(code);
		var model = new TypeReferenceModel(type, compilation);

		Assert.That(model.IsRecord, Is.True);
	}

	private static (ITypeSymbol, Compilation) GetSymbolAndCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, compilation);
	}

	private static (ITypeSymbol, Compilation) GetSymbolReferenceAndCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}