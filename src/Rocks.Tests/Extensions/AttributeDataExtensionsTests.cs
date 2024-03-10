﻿using ADETAttributes;
using ADETTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace ADETTypes
{
	public class TypeOfThis { }

	public class OpenGeneric<T1, T2> { }
}

namespace ADETAttributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MethodAttribute
		: Attribute
	{
		public MethodAttribute(Type value) =>
			this.Value = value;

		public Type Value { get; }
	}
}

namespace Rocks.Tests.Extensions
{
	public enum MyValue
	{
		ThisNegativeOne = -1, 
		ThisOne = 0, 
		ThatOne = 1, 
		AnotherOne = 2
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class MyTestAttribute
		: Attribute
	{
		public MyTestAttribute(string a, double b, int c, uint d, Type e, int[] f, MyValue g) =>
			(this.A, this.B, this.C, this.D, this.E, this.F, this.G) =
				(a, b, c, d, e, f, g);

		public string A { get; }
		public double B { get; }
		public int C { get; }
		public uint D { get; }
		public Type E { get; }
		public int[] F { get; }
		public MyValue G { get; }

		public int NamedA { get; set; }
	}

	public static class AttributeDataExtensionsTests
	{
		[Test]
		public static void GetNamespaces()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
$$"""
using {{typeof(TypeOfThis).Namespace}};
using {{typeof(MethodAttribute).Namespace}};

public interface IA
{
	[Method(typeof(TypeOfThis))]
	void Foo();
}
""");

			var namespaces = attributes[0].GetNamespaces();

			Assert.Multiple(() =>
			{
				Assert.That(namespaces, Has.Count.EqualTo(2));
				Assert.That(namespaces.Any(_ => _.Name == typeof(TypeOfThis).Namespace), Is.True);
				Assert.That(namespaces.Any(_ => _.Name == typeof(MethodAttribute).Namespace), Is.True);
			});
		}

		[Test]
		public static void GetDescriptionWithSpecialCharactersInString()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using Rocks.Tests.Extensions;
				using System;

				[AttributeUsage(AttributeTargets.Method)]
				public sealed class MessageAttribute
					: Attribute
				{
					public MessageAttribute(string message) =>
						this.Message = message;

					public string Message { get; }
				}

				public interface IA
				{
					[Message("a \' a \" a \a a \b a \f a \n a \r a \t a \b")]
					void Foo();
				}
				""");

			Assert.That(attributes[0].GetDescription(compilation), 
				Is.EqualTo("""global::MessageAttribute("a \' a \" a \a a \b a \f a \n a \r a \t a \b")"""));
		}

		[Test]
		public static void GetDescription()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using Rocks.Tests.Extensions;
				using System;

				public interface IA
				{
					[MyTest("a value", 12.34, 22, 44, typeof(Guid), new[] { 6, 7 }, MyValue.ThisOne, NamedA = 44)]
					void Foo();
				}
				""");

			Assert.That(attributes[0].GetDescription(compilation), 
				Is.EqualTo("""global::Rocks.Tests.Extensions.MyTestAttribute("a value", 12.34, 22, 44, typeof(global::System.Guid), new[] { 6, 7 }, (global::Rocks.Tests.Extensions.MyValue)(0), NamedA = 44)"""));
		}

		[Test]
		public static void GetDescriptionWithNegativeEnumValue()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using Rocks.Tests.Extensions;
				using System;

				public interface IA
				{
					[MyTest("a value", 12.34, 22, 44, typeof(Guid), new[] { 6, 7 }, MyValue.ThisNegativeOne, NamedA = 44)]
					void Foo();
				}
				""");

			Assert.That(attributes[0].GetDescription(compilation),
				Is.EqualTo("""global::Rocks.Tests.Extensions.MyTestAttribute("a value", 12.34, 22, 44, typeof(global::System.Guid), new[] { 6, 7 }, (global::Rocks.Tests.Extensions.MyValue)(-1), NamedA = 44)"""));
		}

		[Test]
		public static void GetDescriptionWithOpenGeneric()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using ADETTypes;
				using Rocks.Tests.Extensions;
				using System;

				public interface IA
				{
					[MyTest("a value", 12.34, 22, 44, typeof(OpenGeneric<,>), new[] { 6, 7 }, MyValue.ThisOne, NamedA = 44)]
					void Foo();
				}
				""");

			Assert.That(attributes[0].GetDescription(compilation), 
				Is.EqualTo("""global::Rocks.Tests.Extensions.MyTestAttribute("a value", 12.34, 22, 44, typeof(global::ADETTypes.OpenGeneric<,>), new[] { 6, 7 }, (global::Rocks.Tests.Extensions.MyValue)(0), NamedA = 44)"""));
		}

		[Test]
		public static void GetDescriptionForArrayOfAttributes()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using Rocks.Tests.Extensions;
				using System;

				public interface IA
				{
					[MyTest("a value", 12.34, 22, 44, typeof(Guid), new[] { 6, 7 }, MyValue.ThisOne)]
					[MyTest("b value", 22.34, 33, 55, typeof(string), new[] { 8, 9 }, MyValue.ThatOne)]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo("""[global::Rocks.Tests.Extensions.MyTestAttribute("a value", 12.34, 22, 44, typeof(global::System.Guid), new[] { 6, 7 }, (global::Rocks.Tests.Extensions.MyValue)(0)), global::Rocks.Tests.Extensions.MyTestAttribute("b value", 22.34, 33, 55, typeof(string), new[] { 8, 9 }, (global::Rocks.Tests.Extensions.MyValue)(1))]"""));
		}

		[Test]
		public static void GetDescriptionForArrayWithTarget()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using Rocks.Tests.Extensions;
				using System;

				public interface IA
				{
					[MyTest("a value", 12.34, 22, 44, typeof(Guid), new[] { 6, 7 }, MyValue.ThisOne)]
					[MyTest("b value", 22.34, 33, 55, typeof(string), new[] { 8, 9 }, MyValue.ThatOne)]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation, AttributeTargets.Method), Is.EqualTo("""[method: global::Rocks.Tests.Extensions.MyTestAttribute("a value", 12.34, 22, 44, typeof(global::System.Guid), new[] { 6, 7 }, (global::Rocks.Tests.Extensions.MyValue)(0)), global::Rocks.Tests.Extensions.MyTestAttribute("b value", 22.34, 33, 55, typeof(string), new[] { 8, 9 }, (global::Rocks.Tests.Extensions.MyValue)(1))]"""));
		}

		[Test]
		public static void GetDescriptionWhenDynamicAttributeIsPresent()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetReturnAttributes(
				"""
				using System;
				using System.Runtime.CompilerServices;

				public interface IA
				{
					[return: Dynamic]
					dynamic Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetDescriptionWhenCompilerGeneratedAttributeIsPresent()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using System;
				using System.Runtime.CompilerServices;

				public interface IA
				{
					[CompilerGeneratedAttribute]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetDescriptionWhenIteratorStateMachineAttributeIsPresent()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using System;
				using System.Runtime.CompilerServices;

				public interface IA
				{
					[IteratorStateMachine(typeof(object))]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetDescriptionWhenAsyncIteratorStateMachineAttributeIsPresent()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using System;
				using System.Runtime.CompilerServices;

				public interface IA
				{
					[AsyncIteratorStateMachine(typeof(object))]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetDescriptionWhenAsyncStateMachineAttributeIsPresent()
		{
			var (attributes, compilation) = AttributeDataExtensionsTests.GetAttributes(
				"""
				using System;
				using System.Runtime.CompilerServices;

				public interface IA
				{
					[AsyncStateMachine(typeof(object))]
					void Foo();
				}
				""");

			Assert.That(attributes.GetDescription(compilation), Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetDescriptionWhenAttributeCannotBeSeen()
		{
			var internalCode =
				"""
				using System;

				internal sealed class YouCannotSeeThisAttribute : Attribute { }

				public static class Test
				{
					[YouCannotSeeThis]
					public static void Run() { }
				}
				""";

			var internalSyntaxTree = CSharpSyntaxTree.ParseText(internalCode);
			var internalCompilation = CSharpCompilation.Create("internal", [internalSyntaxTree],
				Shared.References.Value,
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			var externalCode =
				"""
				public static class UseTest
				{
					public static void CallRun() => Test.Run();
				}
				""";

			var externalSyntaxTree = CSharpSyntaxTree.ParseText(externalCode);
			var externalCompilation = CSharpCompilation.Create("external", [externalSyntaxTree],
				Shared.References.Value.Concat([internalCompilation.ToMetadataReference() as MetadataReference]),
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			var externalModel = externalCompilation.GetSemanticModel(externalSyntaxTree, true);

			var internalInvocation = externalSyntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<InvocationExpressionSyntax>().Single();
			var internalMethodSymbol = externalModel.GetSymbolInfo(internalInvocation).Symbol!;

			Assert.That(internalMethodSymbol.GetAttributes().GetDescription(externalCompilation), Is.EqualTo(string.Empty));
		}

		private static (ImmutableArray<AttributeData>, Compilation) GetAttributes(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var compilation = CSharpCompilation.Create("generator", [syntaxTree],
				Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single();
			var methodSymbol = model.GetDeclaredSymbol(methodSyntax)!;

			return (methodSymbol.GetAttributes(), compilation);
		}

		private static (ImmutableArray<AttributeData>, Compilation) GetReturnAttributes(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var compilation = CSharpCompilation.Create("generator", [syntaxTree],
				Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var model = compilation.GetSemanticModel(syntaxTree, true);

			var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
				.OfType<MethodDeclarationSyntax>().Single();
			var methodSymbol = model.GetDeclaredSymbol(methodSyntax)!;

			return (methodSymbol.GetReturnTypeAttributes(), compilation);
		}
	}
}