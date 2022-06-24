using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Performance.Tests;

public static class PointersGenerationTests
{
	[Test]
	public static void Run()
	{
		var benchmarks = new PointersGeneration();
		var compilation = benchmarks.RunGenerator();

		Assert.Multiple(() =>
		{
			Assert.That(compilation.SyntaxTrees.Count, Is.EqualTo(2));

			var x = compilation.GetDiagnostics().ToArray();
			Assert.That(compilation.GetDiagnostics().Count(_ => _.Severity == DiagnosticSeverity.Error), 
				Is.EqualTo(0));
		});
	}
}