using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeSymbolExtensionsIsObsoleteTests
{
	[TestCase("public class Target { } public class Usage { public void Foo(Target t) { } }", false)]
	[TestCase("[System.Obsolete(\"obsolete\")] public class Target { } public class Usage { public void Foo(Target t) { } }", false)]
	[TestCase("[System.Obsolete(\"obsolete\", true)] public class Target { } public class Usage { public void Foo(Target t) { } }", true)]
	[TestCase("public class GenericTarget<T> { } public class Usage { public void Foo(GenericTarget<string> t) { } }", false)]
	[TestCase("[System.Obsolete(\"obsolete\")] public class Target { } public class SubTarget { } public class GenericTarget<T> where T : Target { } public class Usage { public void Foo(GenericTarget<SubTarget> t) { } }", false)]
	[TestCase("[System.Obsolete(\"obsolete\", true)] public class Target { } public class SubTarget { } public class GenericTarget<T> where T : Target { } public class Usage { public void Foo(GenericTarget<SubTarget> t) { } }", true)]
	public static void IsTypeObsolete(string code, bool expectedValue)
	{
		(var type, var obsoleteAttribute) = ITypeSymbolExtensionsIsObsoleteTests.GetSymbol(code);

		Assert.That(type.IsObsolete(obsoleteAttribute), Is.EqualTo(expectedValue));
	}

	private static (ITypeSymbol type, INamedTypeSymbol obsoleteAttribute) GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var obsoleteAttribute = model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName!)!;

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, obsoleteAttribute);
	}
}