using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockableEventDiscoveryTests
{
	[Test]
	public static async Task GetMockableMethodsWithInaccessibleAbstractMethodsAsync()
	{
		const string targetTypeName = "DerivingHolder";

		var source =
			"""
			using System;

			public abstract class Holder
			{
				protected Holder() { }

				protected abstract event EventHandler<DataEventArgs> Test;

				protected internal class DataEventArgs 
					: EventArgs { }
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

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, [sourceCompilation.ToMetadataReference()]);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.EqualTo(1));
			var inaccessibleEvent = result.InaccessibleAbstractMembers[0];
			Assert.That(inaccessibleEvent.Value.Name, Is.EqualTo("Test"));
			Assert.That(inaccessibleEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(inaccessibleEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static async Task GetMockableMethodsWithInaccessibleAbstractMethodsWithOverrideAsync()
	{
		const string targetTypeName = "DerivingHolder";

		var source =
			"""
			using System;

			public abstract class Holder
			{
				protected Holder() { }

				protected abstract event EventHandler<DataEventArgs> Test;

				protected internal class DataEventArgs 
					: EventArgs { }
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
			public class DerivingHolder
				: Holder 
			{ 
				protected override sealed event EventHandler<DataEventArgs> Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, [sourceCompilation.ToMetadataReference()]);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
	}

	[Test]
	public static async Task GetMockableEventsFromAbstractClassAsync()
	{
		const string targetTypeName = "TestClass";

		var code =
			$$"""
			using System;

			public abstract class {{targetTypeName}}
			{
				public abstract event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromClassWithVirtualEventAsync()
	{
		const string targetTypeName = "TestClass";

		var code =
			$$"""
			using System;

			public class {{targetTypeName}}
			{
				public virtual event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromClassWithNonVirtualAndStaticEventsAsync()
	{
		const string targetTypeName = "TestClass";

		var code =
			$$"""
			using System;

			public class {{targetTypeName}}
			{
				public static event EventHandler StaticTest;
				public event EventHandler NormalTest;
				public virtual event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromInterfaceAsync()
	{
		const string targetTypeName = "ITest";

		var code =
			$$"""
			using System;

			public interface {{targetTypeName}}
			{
				event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromInterfaceWithStaticMemberAsync()
	{
		const string targetTypeName = "ITest";

		var code =
			$$"""
			using System;

			public interface {{targetTypeName}}
			{
				static event EventHandler StaticTest;
				event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromInterfaceWithBaseInterfaceAsync()
	{
		const string targetTypeName = "ITest";

		var code =
			$$"""
			using System;

			public interface IBaseTest
			{
				event EventHandler BaseTest;
			}

			public interface {{targetTypeName}}
				: IBaseTest
			{
				event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(2));

			var baseTestEvent = events.Single(_ => _.Value.Name == "BaseTest");
			Assert.That(baseTestEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTestEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static async Task GetMockableEventsFromInterfaceWithBaseInterfacesThatDuplicateNamesAsync()
	{
		const string targetTypeName = "ITest";

		var code =
			$$"""
			using System;

			public interface IBaseTestOne
			{
				event EventHandler BaseTest;
			}

			public interface IBaseTestTwo
			{
				event EventHandler BaseTest;
			}
			
			public interface {{targetTypeName}}
				: IBaseTestOne, IBaseTestTwo
			{
				event EventHandler Test;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName, []);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.InaccessibleAbstractMembers, Has.Length.Zero);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(3));

			var baseTestOneEvent = events.Single(_ => _.Value.Name == "BaseTest" && _.Value.ContainingType.Name == "IBaseTestOne");
			Assert.That(baseTestOneEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTestOneEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));

			var baseTestTwoEvent = events.Single(_ => _.Value.Name == "BaseTest" && _.Value.ContainingType.Name == "IBaseTestTwo");
			Assert.That(baseTestTwoEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTestTwoEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	private static async Task<(ITypeSymbol, Compilation)> GetTypeSymbolAsync(
		string source, string targetTypeName,
		IEnumerable<MetadataReference> additionalReferences)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value.Concat(additionalReferences), new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
		return (model.GetDeclaredSymbol(typeSyntax)!, compilation);
	}
}