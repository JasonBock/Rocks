using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.NetCore.Analyzers.Tasks;
using NUnit.Framework;

namespace Rocks.Analysis.Tests;

// Analyzer class was found here:
// https://github.com/dotnet/sdk/blob/e9d68d884cd44246e6d2d7fdb12714ff7618b90e/src/Microsoft.CodeAnalysis.NetAnalyzers/src/Microsoft.CodeAnalysis.NetAnalyzers/Microsoft.NetCore.Analyzers/Tasks/UseValueTasksCorrectly.cs#L25
public static class ValueTaskInReturnValueSuppressorTests
{
	private static readonly DiagnosticResult CA2012 =
		DiagnosticResult.CompilerWarning("CA2012").WithSeverity(DiagnosticSeverity.Info);

	[Test]
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

	[Test]
	public static async Task SuppressWhenValueTaskIsPassedToCallbackAsync()
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
					adornments.Callback(() => {|#0:ValueTask.FromResult("hi")|});
				}
			}
			""";

		await TestAssistants.RunSuppressorAsync<ValueTaskInReturnValueSuppressor, UseValueTasksCorrectlyAnalyzer>(code, []);
	}

	[Test]
	public static async Task SuppressWhenValueTaskIsPassedToArbitraryMethodAsync()
	{
		var code =
			"""
			using System;
			using System.Threading.Tasks;
			
			public static class Tester
			{
				public static async ValueTask TestAsync() =>
					{|#0:ValueTask.FromResult("hi")|};
			}
			""";

		var expected = new[]
		{
			CA2012.WithLocation(0).WithIsSuppressed(false),
		};

		await TestAssistants.RunSuppressorAsync<ValueTaskInReturnValueSuppressor, UseValueTasksCorrectlyAnalyzer>(code, expected);
	}
}