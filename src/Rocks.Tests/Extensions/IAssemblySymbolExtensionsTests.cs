using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IAssemblySymbolExtensionsTests
{
	[Test]
	public static void CheckExposureWhenSourceAssemblyHasInternalsVisibleToWithTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("TargetAssembly")]
			""";

		IAssemblySymbolExtensionsTests.CheckExposesInternalsTo(sourceCode, true);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyHasMultipleInternalsVisibleToWithOneTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("TargetAssembly")]
			[assembly: InternalsVisibleTo("NotTargetAssembly")]
			""";

		IAssemblySymbolExtensionsTests.CheckExposesInternalsTo(sourceCode, true);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyHasInternalsVisibleToWithDifferentTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("DifferentTargetAssembly")]
			""";

		IAssemblySymbolExtensionsTests.CheckExposesInternalsTo(sourceCode, false);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyHasMultipleInternalsVisibleToWithDifferentTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("DifferentTargetAssembly")]
			[assembly: InternalsVisibleTo("NotTargetAssembly")]
			""";

		IAssemblySymbolExtensionsTests.CheckExposesInternalsTo(sourceCode, false);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyDoesNotHaveInternalsVisibleTo() => 
		IAssemblySymbolExtensionsTests.CheckExposesInternalsTo("public class Source { }", false);

	private static void CheckExposesInternalsTo(string sourceCode, bool expectedResult)
	{
		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
		var sourceCompilation = CSharpCompilation.Create("SourceAssembly", new SyntaxTree[] { sourceSyntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var targetSyntaxTree = CSharpSyntaxTree.ParseText("public class Target { }");
		var targetCompilation = CSharpCompilation.Create("TargetAssembly", new SyntaxTree[] { targetSyntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.That(sourceCompilation.Assembly.ExposesInternalsTo(targetCompilation.Assembly), Is.EqualTo(expectedResult));
	}
}