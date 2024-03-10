using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

internal static class ITypeSymbolExtensionsGetNameTests
{
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", "int*")]
	[TestCase("public class Target { public void Foo(string a) { } }", "string")]
	[TestCase("public class Target { public void Foo(string? a) { } }", "string?")]
	[TestCase("public class Target<T> { public void Foo(T a) { } }", "T")]
	[TestCase("public class Target<T> { public void Foo(T? a) { } }", "T?")]
	[TestCase("using System; public class Target { public void Foo(Guid a) { } }", "global::System.Guid")]
	[TestCase("using System; public class Target { public void Foo(Guid? a) { } }", "global::System.Guid?")]
	[TestCase("public class Generic<T> { } public class Target<U> { public void Foo(Generic<U> a) { } }", "global::Generic<U>")]
	[TestCase("public class Outer { public struct Inner { public unsafe void Foo(Inner* a) { } } }", "global::Outer.Inner*")]
	[TestCase("public class Outer { public struct Inner { public void Foo(Inner a) { } } }", "global::Outer.Inner")]
	[TestCase("public class Outer<T> { public struct Inner { public void Foo(Inner a) { } } }", "global::Outer<T>.Inner")]
	[TestCase("public class Outer<T> { public struct Inner { public void Foo(Outer<object>.Inner a) { } } }", "global::Outer<object>.Inner")]
	[TestCase("using System.Collections.Generic; public static class Target { public static void Foo(List<string> a) { } }", "global::System.Collections.Generic.List<string>")]
	[TestCase("using System.Collections.Generic; using Stuff; namespace Stuff { public class Thing { } } public static class Target { public static void Foo(List<Thing> a) { } }", "global::System.Collections.Generic.List<global::Stuff.Thing>")]
	[TestCase("using System.Collections.Generic; using Stuff; namespace Stuff { public class Thing { } } public static class Target { public static void Foo(List<Thing?> a) { } }", "global::System.Collections.Generic.List<global::Stuff.Thing?>")]
	public static void GetReferenceableName(string code, string expectedName)
	{
		var (typeSymbol, compilation) = ITypeSymbolExtensionsGetNameTests.GetTypeSymbolFromParameter(code);
		var name = typeSymbol.GetFullyQualifiedName(compilation);

		Assert.That(name, Is.EqualTo(expectedName));
	}

	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", TypeNameOption.NoGenerics, "int*")]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", TypeNameOption.IncludeGenerics, "int*")]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", TypeNameOption.Flatten, "intPointer")]
	[TestCase("public class Target { public unsafe void Foo(delegate*<int, void> a) { } }", TypeNameOption.NoGenerics, "delegate*<int, void>")]
	[TestCase("public class Target { public unsafe void Foo(delegate*<int, void> a) { } }", TypeNameOption.IncludeGenerics, "delegate*<int, void>")]
	[TestCase("public class Target { public unsafe void Foo(delegate*<int, void> a) { } }", TypeNameOption.Flatten, "delegatePointerOfint__void")]
	[TestCase("public class Target { public unsafe void Foo(delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int> a) { } }", TypeNameOption.NoGenerics, "delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int>")]
	[TestCase("public class Target { public unsafe void Foo(delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int> a) { } }", TypeNameOption.IncludeGenerics, "delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int>")]
	[TestCase("public class Target { public unsafe void Foo(delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int> a) { } }", TypeNameOption.Flatten, "delegatePointer_unmanaged_Stdcall__SuppressGCTransition_Ofint__int")]
	public static void GetNameForEsotericType(string code, TypeNameOption option, string expectedName)
	{
		var (typeSymbol, _) = ITypeSymbolExtensionsGetNameTests.GetTypeSymbolFromParameter(code);
		var name = typeSymbol.GetName(option);

		Assert.That(name, Is.EqualTo(expectedName));
	}

	[TestCase("public class Target { }", TypeNameOption.NoGenerics, "Target")]
	[TestCase("public class Target<T, T2, TSomething> { }", TypeNameOption.NoGenerics, "Target")]
	[TestCase("public class Target { }", TypeNameOption.IncludeGenerics, "Target")]
	[TestCase("public class Target<T, T2, TSomething> { }", TypeNameOption.IncludeGenerics, "Target<T, T2, TSomething>")]
	[TestCase("public class Target { }", TypeNameOption.Flatten, "Target")]
	[TestCase("public class Target<T, T2, TSomething> : Base", TypeNameOption.Flatten, "TargetOfT_T2_TSomething")]
	public static void GetName(string code, TypeNameOption option, string expectedName)
	{
		var typeSymbol = ITypeSymbolExtensionsGetNameTests.GetTypeSymbol(code);
		var name = typeSymbol.GetName(option);

		Assert.That(name, Is.EqualTo(expectedName));
	}

	[TestCase("public class Base { } public class Target { Base Data { get; } }",
		TypeNameOption.NoGenerics, "Base")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
		TypeNameOption.NoGenerics, "Base")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
		TypeNameOption.NoGenerics, "Base")]
	[TestCase("public class Base { } public class Target { Base Data { get; } }",
		TypeNameOption.IncludeGenerics, "Base")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
		TypeNameOption.IncludeGenerics, "Base<int, string, Guid>")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
		TypeNameOption.IncludeGenerics, "Base<T, string, TSomething>")]
	[TestCase("public class Base { } public class Target { Base Data { get; } }",
		TypeNameOption.Flatten, "Base")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target { Base<int, string, Guid> Data { get; } }",
		TypeNameOption.Flatten, "BaseOfint_string_Guid")]
	[TestCase("public class Base<T, T2, TSomething> { } public class Target<T, TSomething> { Base<T, string, TSomething> Data { get; } }",
		TypeNameOption.Flatten, "BaseOfT_string_TSomething")]
	public static void GetNameFromDeclaredType(string code, TypeNameOption option, string expectedName)
	{
		var parameterSymbol = ITypeSymbolExtensionsGetNameTests.GetDeclaredTypeSymbol(code);
		var name = parameterSymbol.Type.GetName(option);

		Assert.That(name, Is.EqualTo(expectedName));
	}

	private static IPropertySymbol GetDeclaredTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<PropertyDeclarationSyntax>().Where(_ => _.Identifier.Text == "Data").Single();
		return model.GetDeclaredSymbol(propertySyntax)!;
	}

	private static ITypeSymbol GetTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Where(_ => _.Identifier.Text == "Target").Single();
		return model.GetDeclaredSymbol(typeSyntax)!;
	}

	private static (ITypeSymbol, Compilation) GetTypeSymbolFromParameter(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}