using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IAssemblySymbolExtensionsTests
{
	// TODO: Should try to push a lot of this code into one private shared method.
	[Test]
	public static void CheckExposureWhenSourceAssemblyHasInternalsVisibleToWithTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("TargetAssembly")]
			""";

		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
		var sourceReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var sourceCompilation = CSharpCompilation.Create("SourceAssembly", new SyntaxTree[] { sourceSyntaxTree },
			sourceReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var targetSyntaxTree = CSharpSyntaxTree.ParseText("public class Target { }");
		var targetReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var targetCompilation = CSharpCompilation.Create("TargetAssembly", new SyntaxTree[] { targetSyntaxTree },
			targetReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.That(sourceCompilation.Assembly.ExposesInternalsTo(targetCompilation.Assembly), Is.True);
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

		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
		var sourceReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var sourceCompilation = CSharpCompilation.Create("SourceAssembly", new SyntaxTree[] { sourceSyntaxTree },
			sourceReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var targetSyntaxTree = CSharpSyntaxTree.ParseText("public class Target { }");
		var targetReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var targetCompilation = CSharpCompilation.Create("TargetAssembly", new SyntaxTree[] { targetSyntaxTree },
			targetReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.That(sourceCompilation.Assembly.ExposesInternalsTo(targetCompilation.Assembly), Is.True);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyHasInternalsVisibleToWithDifferentTargetAssemblyName()
	{
		var sourceCode =
			"""
			using System.Runtime.CompilerServices;

			[assembly: InternalsVisibleTo("DifferentTargetAssembly")]
			""";

		var sourceSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
		var sourceReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var sourceCompilation = CSharpCompilation.Create("SourceAssembly", new SyntaxTree[] { sourceSyntaxTree },
			sourceReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var targetSyntaxTree = CSharpSyntaxTree.ParseText("public class Target { }");
		var targetReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var targetCompilation = CSharpCompilation.Create("TargetAssembly", new SyntaxTree[] { targetSyntaxTree },
			targetReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.That(sourceCompilation.Assembly.ExposesInternalsTo(targetCompilation.Assembly), Is.False);
	}

	[Test]
	public static void CheckExposureWhenSourceAssemblyDoesNotHaveInternalsVisibleTo()
	{
		var sourceSyntaxTree = CSharpSyntaxTree.ParseText("public class Source { }");
		var sourceReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var sourceCompilation = CSharpCompilation.Create("SourceAssembly", new SyntaxTree[] { sourceSyntaxTree },
			sourceReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var targetSyntaxTree = CSharpSyntaxTree.ParseText("public class Target { }");
		var targetReferences = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ =>
			{
				var location = _.Location;
				return MetadataReference.CreateFromFile(location);
			});
		var targetCompilation = CSharpCompilation.Create("TargetAssembly", new SyntaxTree[] { targetSyntaxTree },
			targetReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		Assert.That(sourceCompilation.Assembly.ExposesInternalsTo(targetCompilation.Assembly), Is.False);
	}
}