using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockablePropertyDiscoveryTests
{
	[Test]
	public static async Task GetMockablePropertiesFromInterfaceWithStaticNonVirtualPropertiesAsync()
	{
		const string targetTypeName = "ITest";

		var code =
			$$"""
			public interface {{targetTypeName}}
			{
				static int StaticData { get; init; }
				int Data { get; init; }
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockablePropertyDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, ref memberIdentifier, compilation).Properties;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var properties = result.Results;
			Assert.That(properties, Has.Length.EqualTo(1));

			var dataProperty = properties.Single(_ => _.Value.Name == "Data");
			Assert.That(dataProperty.Accessors, Is.EqualTo(PropertyAccessor.GetAndInit));
		}
	}

	[Test]
	public static async Task GetMockablePropertiesWithInitAsync()
	{
		const string targetTypeName = "Test";

		var code =
			$$"""
			public class {{targetTypeName}}
			{
				public virtual int GetAndInit { get; init; }
				public virtual int Init { init; }
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockablePropertyDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, ref memberIdentifier, compilation).Properties;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var properties = result.Results;
			Assert.That(properties, Has.Length.EqualTo(2));

			var getAndInitProperty = properties.Single(_ => _.Value.Name == "GetAndInit");
			Assert.That(getAndInitProperty.Accessors, Is.EqualTo(PropertyAccessor.GetAndInit));

			var initProperty = properties.Single(_ => _.Value.Name == "Init");
			Assert.That(initProperty.Accessors, Is.EqualTo(PropertyAccessor.Init));
		}
	}

	[Test]
	public static async Task GetMockablePropertiesWithMultipleIndexersAsync()
	{
		const string targetTypeName = "Test";

		var code =
			$$"""
			public abstract class {{targetTypeName}}
			{
				public abstract int this[int a, string b] { get; }
				public abstract int this[string a] { get; }
				public abstract int this[int a] { get; }
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockablePropertyDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, ref memberIdentifier, compilation).Properties;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var properties = result.Results;
			Assert.That(properties, Has.Length.EqualTo(3));

			var thisIntProperty = properties.Single(_ => _.Value.Parameters.Length == 1 &&
				 _.Value.Parameters[0].Type.SpecialType == SpecialType.System_Int32);
			Assert.That(thisIntProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(thisIntProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var thisStringProperty = properties.Single(_ => _.Value.Parameters.Length == 1 &&
				 _.Value.Parameters[0].Type.SpecialType == SpecialType.System_String);
			Assert.That(thisStringProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(thisStringProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));

			var thisIntStringProperty = properties.Single(_ => _.Value.Parameters.Length == 2);
			Assert.That(thisIntStringProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(thisIntStringProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static async Task GetMockablePropertiesWithNewAbstractPropertyWithOverrideAsync()
	{
		const string targetPropertyName = "Data";
		const string targetTypeName = "Test";

		var code =
			$$"""
			public abstract class CoreClass
			{
				public virtual int {{targetPropertyName}} => 3;
			}

			public abstract class BaseClass
				: CoreClass
			{
				public new abstract int {{targetPropertyName}} { get; }
			}

			public class {{targetTypeName}}
				: BaseClass
			{
				public override int {{targetPropertyName}} => 3;
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockablePropertyDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, ref memberIdentifier, compilation).Properties;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var properties = result.Results;
			Assert.That(properties, Has.Length.EqualTo(1));

			var dataProperty = properties.Single(_ => _.Value.Name == targetPropertyName);
			Assert.That(dataProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static async Task GetMockablePropertiesWithMultipleAbstractPropertiesWithOverrideAsync()
	{
		const string targetPropertyName = "Data";
		const string targetTypeName = "Test";

		var code =
			$$"""
			public abstract class CoreClass
			{
				public abstract int {{targetPropertyName}} { get; }
			}

			public abstract class BaseClass
				: CoreClass
			{
				public override int {{targetPropertyName}} => 3;
			}

			public class {{targetTypeName}}
				: BaseClass
			{
				public override int {{targetPropertyName}} => base.{{targetPropertyName}};
			}
			""";

		var (typeSymbol, compilation) = await GetTypeSymbolAsync(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockablePropertyDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, ref memberIdentifier, compilation).Properties;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var properties = result.Results;
			Assert.That(properties, Has.Length.EqualTo(1));

			var dataProperty = properties.Single(_ => _.Value.Name == targetPropertyName);
			Assert.That(dataProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	private static async Task<(ITypeSymbol, Compilation)> GetTypeSymbolAsync(string source, string targetTypeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
		return (model.GetDeclaredSymbol(typeSyntax)!, compilation);
	}
}