using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.NetCore.Analyzers.Tasks;
using NUnit.Framework;

namespace Rocks.Analysis.Tests;

public static class ValueTypeInReturnValueSuppressorTests
{
	private static readonly DiagnosticResult CA2012 = 
		DiagnosticResult.CompilerWarning("CA2012").WithSeverity(DiagnosticSeverity.Info);

	[Test]
	[Ignore("Currently having an issue with the testing library - https://github.com/dotnet/roslyn-sdk/issues/1175")]
	public static async Task SuppressWhenValueTaskIsPassedToReturnValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Threading.Tasks;
			
			public sealed class TestHandler
				: Handler<Func<ValueTask<string>>, ValueTask<string>>
			{ }

			public sealed class TestAdornments
				: Adornments<TestAdornments, TestHandler, Func<ValueTask<string>>, ValueTask<string>>
			{
				public TestAdornments(TestHandler handler) 
					: base(handler) { }
			}

			public static class Tester
			{
				public static void Test()
				{
					var adornments = new TestAdornments(new());
					adornments.ReturnValue({|#0:ValueTask.FromResult("hi")|});
				}
			}
			""";

		var expected = new[]
		{
			CA2012.WithLocation(0).WithIsSuppressed(true),
		};

		await TestAssistants.RunSuppressorAsync<ValueTaskInReturnValueSuppressor, UseValueTasksCorrectlyAnalyzer>(code, expected);
	}
}