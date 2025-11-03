using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Discovery;

namespace Rocks.Analysis.Tests.Discovery;

public static class MockableMethodDiscoveryTests
{
	[Test]
	public static void GetMockableMethodsWithNewAbstractMethodWithOverride()
	{
		const string targetMethodName = "GetData";
		const string targetTypeName = "Test";

		var code =
			$$"""
			public abstract class CoreClass
			{
				public virtual int {{targetMethodName}}() => 3;
			}

			public abstract class BaseClass
				: CoreClass
			{
				public new abstract int {{targetMethodName}}();
			}

			public class {{targetTypeName}}
				: BaseClass
			{
				public override int {{targetMethodName}}() => 3;
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var dataMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(dataMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static void GetMockableMethodsWithMultipleAbstractMethodsWithOverride()
	{
		const string targetMethodName = "Data";
		const string targetTypeName = "Test";

		var code =
			$$"""
			public abstract class CoreClass
			{
				public abstract int {{targetMethodName}}();
			}

			public abstract class BaseClass
				: CoreClass
			{
				public override int {{targetMethodName}}() => 3;
			}

			public class {{targetTypeName}}
				: BaseClass
			{
				public override int {{targetMethodName}}() => base.{{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var dataMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(dataMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static void GetMockableMethodsWithInterfaceMethods()
	{
		const string targetMethodName = "Foo";
		const string targetTypeName = "ITest";

		var code =
			$$"""
			public interface {{targetTypeName}}
			{
				void {{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(1));
			var fooMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		}
	}

	[Test]
	public static void GetMockableMethodsWithStaticNonVirtualMethodsOnInterface()
	{
		const string targetMethodName = "Foo";
		const string targetTypeName = "ITest";

		var code =
			$$"""
			public interface {{targetTypeName}}
			{
				public static string ToString(IRequest request) => "";
				public static string ToMethodCallString(IRequest request) => "";
				void {{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(1));
			var fooMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		}
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasBaseInterface()
	{
		const string baseMethodName = "Foo";
		const string targetMethodName = "Bar";
		const string targetTypeName = "Target";
		var code =
			$$"""
			public interface Base
			{
				void {{baseMethodName}}();
			}

			public interface {{targetTypeName}} 
				: Base
			{ 
				void {{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(2));
			var baseMethod = methods.Single(_ => _.Value.Name == baseMethodName);
			Assert.That(baseMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		}
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasBaseInterfaceWithMatchingMethod()
	{
		const string baseTypeName = "Base";
		const string targetMethodName = "Bar";
		const string targetTypeName = "Target";
		var code =
			$$"""
			public interface {{baseTypeName}}
			{
				void {{targetMethodName}}();
			}

			public interface {{targetTypeName}} 
				: {{baseTypeName}}
			{ 
				void {{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(1));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName && _.Value.ContainingType.Name == targetTypeName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		}
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasBaseInterfacesWithMatchingMethods()
	{
		const string baseOneTypeName = "BaseOne";
		const string baseTwoTypeName = "BaseTwo";
		const string baseMethodName = "Foo";
		const string targetMethodName = "Bar";
		const string targetTypeName = "Target";

		var code =
			$$"""
			public interface {{baseOneTypeName}}
			{
				void {{baseMethodName}}();
			}

			public interface {{baseTwoTypeName}}
			{
				void {{baseMethodName}}();
			}

			public interface {{targetTypeName}}
				: {{baseOneTypeName}}, {{baseTwoTypeName}}
			{ 
				void {{targetMethodName}}();
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(3));
			var baseOneMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseOneTypeName);
			Assert.That(baseOneMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseOneMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var baseTwoMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseTwoTypeName);
			Assert.That(baseTwoMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTwoMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(targetMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		}
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasExplicitInterfaceImplementation()
	{
		const string baseOneTypeName = "BaseOne";
		const string baseTwoTypeName = "BaseTwo";
		const string baseMethodName = "Foo";
		const string targetTypeName = "Target";

		var code =
			$$"""
			public interface {{baseOneTypeName}}
			{
				void {{baseMethodName}}();
			}

			public interface {{baseTwoTypeName}}
			{
				void {{baseMethodName}}();
			}

			public interface {{targetTypeName}} 
				: {{baseOneTypeName}}, {{baseTwoTypeName}}
			{ }
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(2));
			var baseOneMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseOneTypeName);
			Assert.That(baseOneMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseOneMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var baseTwoMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseTwoTypeName);
			Assert.That(baseTwoMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTwoMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
		}
	}

	[Test]
	public static void GetMockableMethodsWithClassMethod()
	{
		const string targetTypeName = "Test";
		const string targetMethodName = "Foo";

		var code =
			$$"""
			public class {{targetTypeName}}
			{
				public virtual void {{targetMethodName}}() { }
			}
			""";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var fooMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		}
	}

	[Test]
	public static void GetMockableMethodsWithClassNoMethods()
	{
		const string targetTypeName = "Test";

		var code = $"public class {targetTypeName} {{ }}";

		var (typeSymbol, compilation) = GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var result = new MockableMethodDiscovery(typeSymbol, typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier).Methods;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(result.HasInaccessibleAbstractMembers, Is.False);
			var methods = result.Results;
			Assert.That(methods, Has.Length.EqualTo(3));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
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