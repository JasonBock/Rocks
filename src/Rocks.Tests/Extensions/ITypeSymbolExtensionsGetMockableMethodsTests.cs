using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeSymbolExtensionsGetMockableMethodsTests
{
	[Test]
	public static void GetMockableMethodsWithNewAbstractMethodWithOverride()
	{
		const string targetMethodName = "GetData";
		const string targetTypeName = "Test";

		var code =
$@"public abstract class CoreClass
{{
	public virtual int {targetMethodName}() => 3;
}}

public abstract class BaseClass
	: CoreClass
{{
	public new abstract int {targetMethodName}();
}}

public class {targetTypeName}
	: BaseClass
{{
	public override int {targetMethodName}() => 3;
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var dataMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(dataMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}

	[Test]
	public static void GetMockableMethodsWithMultipleAbstractMethodsWithOverride()
	{
		const string targetMethodName = "Data";
		const string targetTypeName = "Test";

		var code =
$@"public abstract class CoreClass
{{
	public abstract int {targetMethodName}();
}}

public abstract class BaseClass
	: CoreClass
{{
	public override int {targetMethodName}() => 3;
}}

public class {targetTypeName}
	: BaseClass
{{
	public override int {targetMethodName}() => base.{targetMethodName}();
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var dataMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(dataMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}

	[Test]
	public static void GetMockableMethodsWithInterfaceMethods()
	{
		const string targetMethodName = "Foo";
		const string targetTypeName = "ITest";

		var code =
$@"public interface {targetTypeName}
{{
	void {targetMethodName}();
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(1));
			var fooMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		});
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasBaseInterface()
	{
		const string baseMethodName = "Foo";
		const string targetMethodName = "Bar";
		const string targetTypeName = "Target";
		var code =
$@"public interface Base
{{
	void {baseMethodName}();
}}

public interface {targetTypeName} 
	: Base
{{ 
	void {targetMethodName}();
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(2));
			var baseMethod = methods.Single(_ => _.Value.Name == baseMethodName);
			Assert.That(baseMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		});
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasBaseInterfaceWithMatchingMethod()
	{
		const string baseTypeName = "Base";
		const string targetMethodName = "Bar";
		const string targetTypeName = "Target";
		var code =
$@"public interface {baseTypeName}
{{
	void {targetMethodName}();
}}

public interface {targetTypeName} 
	: {baseTypeName}
{{ 
	void {targetMethodName}();
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(1));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName && _.Value.ContainingType.Name == targetTypeName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
		});
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
$@"public interface {baseOneTypeName}
{{
	void {baseMethodName}();
}}

public interface {baseTwoTypeName}
{{
	void {baseMethodName}();
}}

public interface {targetTypeName} 
	: {baseOneTypeName}, {baseTwoTypeName}
{{ 
	void {targetMethodName}();
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(3));
			var baseOneMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseOneTypeName);
			Assert.That(baseOneMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseOneMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var baseTwoMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseTwoTypeName);
			Assert.That(baseTwoMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTwoMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var targetMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(targetMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(targetMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
		});
	}

	[Test]
	public static void GetMockableMethodsWhenInterfaceHasExplicitInterfaceImplementation()
	{
		const string baseOneTypeName = "BaseOne";
		const string baseTwoTypeName = "BaseTwo";
		const string baseMethodName = "Foo";
		const string targetTypeName = "Target";

		var code =
$@"public interface {baseOneTypeName}
{{
	void {baseMethodName}();
}}

public interface {baseTwoTypeName}
{{
	void {baseMethodName}();
}}

public interface {targetTypeName} 
	: {baseOneTypeName}, {baseTwoTypeName}
{{ }}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(2));
			var baseOneMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseOneTypeName);
			Assert.That(baseOneMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseOneMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
			var baseTwoMethod = methods.Single(_ => _.Value.Name == baseMethodName && _.Value.ContainingType.Name == baseTwoTypeName);
			Assert.That(baseTwoMethod.RequiresOverride, Is.EqualTo(RequiresOverride.No));
			Assert.That(baseTwoMethod.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.Yes));
		});
	}

	[Test]
	public static void GetMockableMethodsWithClassMethod()
	{
		const string targetTypeName = "Test";
		const string targetMethodName = "Foo";

		var code =
$@"public class {targetTypeName}
{{
	public virtual void {targetMethodName}() {{ }}
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(4));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var fooMethod = methods.Single(_ => _.Value.Name == targetMethodName);
			Assert.That(fooMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}

	[Test]
	public static void GetMockableMethodsWithClassNoMethods()
	{
		const string targetTypeName = "Test";

		var code = $"public class {targetTypeName} {{ }}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockableMethodsTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var methods = typeSymbol.GetMockableMethods(typeSymbol.ContainingAssembly, shims, compilation, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(methods.Length, Is.EqualTo(3));
			var getHashCodeMethod = methods.Single(_ => _.Value.Name == nameof(object.GetHashCode));
			Assert.That(getHashCodeMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var equalsMethod = methods.Single(_ => _.Value.Name == nameof(object.Equals));
			Assert.That(equalsMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
			var toStringMethod = methods.Single(_ => _.Value.Name == nameof(object.ToString));
			Assert.That(toStringMethod.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}

	private static (ITypeSymbol, Compilation) GetTypeSymbol(string source, string targetTypeName)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single(_ => _.Identifier.Text == targetTypeName);
		return (model.GetDeclaredSymbol(typeSyntax)!, compilation);
	}
}