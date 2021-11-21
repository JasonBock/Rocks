using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitMakeGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public interface ITest
	{
		int Bar { get; init; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ITest>();
		}
	}
}";

		var generatedCode = string.Empty;

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}