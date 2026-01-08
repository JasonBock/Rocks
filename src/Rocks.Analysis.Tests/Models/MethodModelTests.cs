using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class MethodModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			public class Target
			{
				public int Go(string value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.AttributesDescription, Is.Empty);
			Assert.That(model.Constraints, Is.Empty);
			Assert.That(model.ContainingType.FullyQualifiedName, Is.EqualTo("global::Target"));
			Assert.That(model.DefaultConstraints, Is.Empty);
			Assert.That(model.IsAbstract, Is.False);
			Assert.That(model.IsGenericMethod, Is.False);
			Assert.That(model.IsMarkedWithDoesNotReturn, Is.False);
			Assert.That(model.IsUnsafe, Is.False);
			Assert.That(model.IsVirtual, Is.False);
			Assert.That(model.MemberIdentifier, Is.EqualTo(memberIdentifier));
			Assert.That(model.MethodKind, Is.EqualTo(MethodKind.Ordinary));
			Assert.That(model.MockType, Is.SameAs(mockType));
			Assert.That(model.Name, Is.EqualTo("Go"));
			Assert.That(model.OverridingCodeValue, Is.EqualTo("public"));
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("value"));
			Assert.That(model.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(model.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(model.RequiresProjectedDelegate, Is.False);
			Assert.That(model.ReturnType.FullyQualifiedName, Is.EqualTo("int"));
			Assert.That(model.ReturnTypeAttributesDescription, Is.Empty);
			Assert.That(model.ReturnTypeIsTaskOfTType, Is.False);
			Assert.That(model.ReturnTypeIsTaskOfTTypeAndIsNullForgiving, Is.False);
			Assert.That(model.ReturnTypeIsTaskType, Is.False);
			Assert.That(model.ReturnTypeIsValueTaskOfTType, Is.False);
			Assert.That(model.ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving, Is.False);
			Assert.That(model.ReturnTypeIsValueTaskType, Is.False);
			Assert.That(model.ReturnsVoid, Is.False);
			Assert.That(model.ReturnsByRef, Is.False);
			Assert.That(model.ReturnsByRefReadOnly, Is.False);
			Assert.That(model.ShouldThrowDoesNotReturnException, Is.False);
			Assert.That(model.ReturnTypeTypeArguments, Is.Empty);
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
				public int Go(string value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.AttributesDescription, Is.EqualTo("[global::System.CLSCompliantAttribute(true)]"));
	}

	[Test]
	public static async Task CreateWithReturnTypeAttributesAsync()
	{
		var code =
			"""
			using System;

			public class Target
			{
				[return: CLSCompliant(true)]
				public int Go(string value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeAttributesDescription, Is.EqualTo("[return: global::System.CLSCompliantAttribute(true)]"));
	}

	[Test]
	public static async Task CreateWithReturnTypeWithTypeArgumentsAsync()
	{
		var code =
			"""
			using System;

			public class Target
			{
				public EventArgs<T> Go<T>() => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.ReturnTypeTypeArguments, Has.Length.EqualTo(1));
			Assert.That(model.ReturnTypeTypeArguments[0].FullyQualifiedName, Is.EqualTo("T"));
		}
	}

	[Test]
	public static async Task CreateWithExplicitInterfaceImplementationAsync()
	{
		var code =
			"""
			public class Target
			{
				public int Go(string value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.OverridingCodeValue, Is.Null);
	}

	[Test]
	public static async Task CreateWithGenericsAsync()
	{
		var code =
			"""
			public class Target
			{
				public int Go<T>(T value) where T : class => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.Constraints, Has.Length.EqualTo(1));
			Assert.That(model.Constraints[0].ToString(), Is.EqualTo("where T : class"));
			Assert.That(model.DefaultConstraints, Has.Length.EqualTo(1));
			Assert.That(model.DefaultConstraints[0].ToString(), Is.EqualTo("where T : class"));
			Assert.That(model.IsGenericMethod, Is.True);
		}
	}

	[Test]
	public static async Task CreateWithAbstractAsync()
	{
		var code =
			"""
			public abstract class Target
			{
				public abstract int Go(string value);
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.IsAbstract, Is.True);
	}

	[Test]
	public static async Task CreateWithVoidReturnAsync()
	{
		var code =
			"""
			public class Target
			{
				public void Go(string value) { }
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnsVoid, Is.True);
	}

	[Test]
	public static async Task CreateWithRefReturnAsync()
	{
		var code =
			"""
			public class Target
			{
				private ref int data;

				public ref int Go(string value) => ref this.data;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnsByRef, Is.True);
	}

	[Test]
	public static async Task CreateWithRefReadOnlyReturnAsync()
	{
		var code =
			"""
			public class Target
			{
				private ref int data;

				public ref readonly int Go(string value) => ref this.data;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnsByRefReadOnly, Is.True);
	}

	[Test]
	public static async Task CreateWithTaskReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public Task Go(string value) => Task.CompletedTask;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsTaskType, Is.True);
	}

	[Test]
	public static async Task CreateWithTaskOfTReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public Task<int> Go(string value) => Task.FromResult(1);
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsTaskOfTType, Is.True);
	}

	[Test]
	public static async Task CreateWithTaskOfTAndIsNullForgivingReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public Task<string?> Go(string value) => Task.FromResult("value");
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsTaskOfTTypeAndIsNullForgiving, Is.True);
	}

	[Test]
	public static async Task CreateWithValueTaskReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public ValueTask Go(string value) => ValueTask.CompletedTask;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsValueTaskType, Is.True);
	}

	[Test]
	public static async Task CreateWithValueTaskOfTReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public ValueTask<int> Go(string value) => Task.FromResult(1);
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsValueTaskOfTType, Is.True);
	}

	[Test]
	public static async Task CreateWithValueTaskOfTAndIsNullForgivingReturnAsync()
	{
		var code =
			"""
			using System.Threading.Tasks;

			public class Target
			{
				public ValueTask<int?> Go(string value) => Task.FromResult(1);
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving, Is.True);
	}

	[Test]
	public static async Task CreateWithVirtualAsync()
	{
		var code =
			"""
			public class Target
			{
				public virtual int Go(string value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.IsVirtual, Is.True);
	}

	[Test]
	public static async Task CreateWithDoesNotReturnAsync()
	{
		var code =
			"""
			using System;
			using System.Diagnostics.CodeAnalysis;

			public class Target
			{
				[DoesNotReturn]
				public virtual int Go(string value) => throw new NotSupportedException();
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.IsMarkedWithDoesNotReturn, Is.True);
			Assert.That(model.ShouldThrowDoesNotReturnException, Is.True);
		}
	}

	[Test]
	public static async Task CreateWithUnsafeAsync()
	{
		var code =
			"""
			public class Target
			{
				public unsafe int Go(int* value) => default;
			}
			""";

		const uint memberIdentifier = 1;

		(var method, var type, var modelContext) = await GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new MethodModel(method, mockType, modelContext,
			 RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, RequiresHiding.No, memberIdentifier);

		Assert.That(model.IsUnsafe, Is.True);
	}

	private static async Task<(IMethodSymbol, ITypeSymbol, ModelContext)> GetSymbolsCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);
		var root = await syntaxTree.GetRootAsync();
		var typeSyntax = root.DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = root.DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!, model.GetDeclaredSymbol(typeSyntax)!, new(model));
	}
}