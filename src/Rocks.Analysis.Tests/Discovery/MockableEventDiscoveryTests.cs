using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockableEventDiscoveryTests
{
	[Test]
	public static void GetMockableEventsFromAbstractClass()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableEventsFromClassWithVirtualEvent()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableEventsFromClassWithNonVirtualAndStaticEvents()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableEventsFromInterface()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableEventsFromInterfaceWithStaticMember()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var events = result.Results;
			Assert.That(events, Has.Length.EqualTo(1));

			var testEvent = events.Single(_ => _.Value.Name == "Test");
			Assert.That(testEvent.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(testEvent.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableEventsFromInterfaceWithBaseInterface()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
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
	public static void GetMockableEventsFromInterfaceWithBaseInterfacesThatDuplicateNames()
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

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var result = new MockableEventDiscovery(typeSymbol, typeSymbol.ContainingAssembly, compilation).Events;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
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

	private static (ITypeSymbol, Compilation) GetTypeSymbol(string source, string targetTypeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
		return (model.GetDeclaredSymbol(typeSyntax)!, compilation);
	}
}