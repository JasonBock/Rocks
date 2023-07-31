using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Performance.Tests;

public static class LargeInterfaceGenerationTests
{
	[Test]
	public static void Run()
	{
		var benchmarks = new LargeInterfaceGeneration();
		var compilation = benchmarks.RunGenerator();

		Assert.Multiple(() =>
		{
			Assert.That(compilation.SyntaxTrees.Count, Is.EqualTo(2));
			Assert.That(compilation.GetDiagnostics().Count(_ => _.Severity == DiagnosticSeverity.Error), 
				Is.EqualTo(0));
		});
	}
}