using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Performance.Tests;

public static class SimpleGenerationTests
{
	[Test]
	public static void Run()
	{
		var benchmarks = new SimpleGeneration();
		var compilation = benchmarks.RunGenerator();

		Assert.Multiple(() =>
		{
			Assert.That(compilation.SyntaxTrees.Count, Is.EqualTo(2));
			Assert.That(compilation.GetDiagnostics().Count(
				_ => _.Severity == DiagnosticSeverity.Warning || _.Severity == DiagnosticSeverity.Error), 
				Is.EqualTo(0));
		});
	}
}