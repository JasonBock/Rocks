using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeSymbolExtensionsGetMockablePropertiesTests
{
	[Test]
	public static void GetMockablePropertiesWithInit()
	{
		const string targetTypeName = "Test";

		var code =
$@"public class {targetTypeName}
{{
	public virtual int GetAndInit {{ get; init; }}
	public virtual int Init {{ init; }}
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockablePropertiesTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var properties = typeSymbol.GetMockableProperties(typeSymbol.ContainingAssembly, shims, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(properties.Length, Is.EqualTo(2));
			var getAndInitProperty = properties.Single(_ => _.Value.Name == "GetAndInit");
			Assert.That(getAndInitProperty.Accessors, Is.EqualTo(PropertyAccessor.GetAndInit));
			var initProperty = properties.Single(_ => _.Value.Name == "Init");
			Assert.That(initProperty.Accessors, Is.EqualTo(PropertyAccessor.Init));
		});
	}

	[Test]
	public static void GetMockablePropertiesWithMultipleIndexers()
	{
		const string targetTypeName = "Test";

		var code =
$@"public abstract class {targetTypeName}
{{
	public abstract int this[int a, string b] {{ get; }}
	public abstract int this[string a] {{ get; }}
	public abstract int this[int a] {{ get; }}
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockablePropertiesTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var properties = typeSymbol.GetMockableProperties(typeSymbol.ContainingAssembly, shims, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(properties.Length, Is.EqualTo(3));
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
		});
	}

	[Test]
	public static void GetMockablePropertiesWithNewAbstractPropertyWithOverride()
	{
		const string targetPropertyName = "Data";
		const string targetTypeName = "Test";

		var code =
$@"public abstract class CoreClass
{{
	public virtual int {targetPropertyName} => 3;
}}

public abstract class BaseClass
	: CoreClass
{{
	public new abstract int {targetPropertyName} {{ get; }}
}}

public class {targetTypeName}
	: BaseClass
{{
	public override int {targetPropertyName} => 3;
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockablePropertiesTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var properties = typeSymbol.GetMockableProperties(typeSymbol.ContainingAssembly, shims, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(properties.Length, Is.EqualTo(1));
			var dataProperty = properties.Single(_ => _.Value.Name == targetPropertyName);
			Assert.That(dataProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
		});
	}

	[Test]
	public static void GetMockablePropertiesWithMultipleAbstractPropertiesWithOverride()
	{
		const string targetPropertyName = "Data";
		const string targetTypeName = "Test";

		var code =
$@"public abstract class CoreClass
{{
	public abstract int {targetPropertyName} {{ get; }}
}}

public abstract class BaseClass
	: CoreClass
{{
	public override int {targetPropertyName} => 3;
}}

public class {targetTypeName}
	: BaseClass
{{
	public override int {targetPropertyName} => base.{targetPropertyName};
}}";

		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetMockablePropertiesTests.GetTypeSymbol(code, targetTypeName);
		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>();
		var properties = typeSymbol.GetMockableProperties(typeSymbol.ContainingAssembly, shims, ref memberIdentifier);

		Assert.Multiple(() =>
		{
			Assert.That(properties.Length, Is.EqualTo(1));
			var dataProperty = properties.Single(_ => _.Value.Name == targetPropertyName);
			Assert.That(dataProperty.RequiresExplicitInterfaceImplementation, Is.EqualTo(RequiresExplicitInterfaceImplementation.No));
			Assert.That(dataProperty.RequiresOverride, Is.EqualTo(RequiresOverride.Yes));
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