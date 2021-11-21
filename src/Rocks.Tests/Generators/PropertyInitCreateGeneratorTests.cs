using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitCreateGeneratorTests
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

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}