//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using NUnit.Framework;
//using Rocks.Extensions;
//using System.ComponentModel;

//namespace Rocks.Tests.Extensions;

//public static class AccessibilityExtensionsTests
//{
//	[TestCase(Accessibility.Public, "public")]
//	[TestCase(Accessibility.Private, "private")]
//	[TestCase(Accessibility.Protected, "protected")]
//	[TestCase(Accessibility.Internal, "internal")]
//	[TestCase(Accessibility.ProtectedOrInternal, "protected internal")]
//	[TestCase(Accessibility.ProtectedAndInternal, "private protected")]
//	public static void GetOverridingCodeValue(Accessibility accessibility, string codeValue) =>
//		Assert.That(accessibility.GetOverridingCodeValue(), Is.EqualTo(codeValue));

//	[TestCase(Accessibility.Public, true, "public")]
//	[TestCase(Accessibility.Private, true, "private")]
//	[TestCase(Accessibility.Protected, true, "protected")]
//	[TestCase(Accessibility.Internal, true, "internal")]
//	[TestCase(Accessibility.ProtectedOrInternal, true, "protected internal")]
//	[TestCase(Accessibility.ProtectedOrInternal, false, "protected")]
//	[TestCase(Accessibility.ProtectedAndInternal, true, "private protected")]
//	public static void GetOverridingCodeValueTest(Accessibility accessibility, bool areAssembliesIdentical, string codeValue)
//	{
//		var source = "public static class Stuff { }";
//		var syntaxTree = CSharpSyntaxTree.ParseText(source);
//		var references = AppDomain.CurrentDomain.GetAssemblies()
//			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
//			.Select(_ => MetadataReference.CreateFromFile(_.Location));
//		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
//			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

//		IAssemblySymbol? selfAssembly = null;

//		if(areAssembliesIdentical) 
//		{
//			selfAssembly = compilation.Assembly;
//		}
//		else
//		{
//			var selfSource = "public static class Stuff { }";
//			var selfSyntaxTree = CSharpSyntaxTree.ParseText(selfSource);
//			var selfReferences = AppDomain.CurrentDomain.GetAssemblies()
//				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
//				.Select(_ => MetadataReference.CreateFromFile(_.Location));
//			var selfCompilation = CSharpCompilation.Create("generator", new SyntaxTree[] { selfSyntaxTree },
//				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
//			selfAssembly = selfCompilation.Assembly;
//		}

//		Assert.That(accessibility.GetOverridingCodeValueTest(selfAssembly, compilation.Assembly), Is.EqualTo(codeValue));
//	}

//	[Test]
//	public static void GetOverridingCodeValueWithInvalidAccesibility() =>
//		Assert.That(() => Accessibility.NotApplicable.GetOverridingCodeValue(), Throws.TypeOf<InvalidEnumArgumentException>());
//}