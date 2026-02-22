using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeQuality.Analyzers.QualityGuidelines;
using NUnit.Framework;

namespace Rocks.Analysis.Tests;

// Analyzer class was found here:
// https://github.com/dotnet/sdk/blob/e9d68d884cd44246e6d2d7fdb12714ff7618b90e/src/Microsoft.CodeAnalysis.NetAnalyzers/src/Microsoft.CodeAnalysis.NetAnalyzers/Microsoft.CodeQuality.Analyzers/QualityGuidelines/DoNotPassDisposablesIntoUnawaitedTasks.cs#L19
public static class IDisposableInstancesIntoTasksSuppressorTests
{
	private static readonly DiagnosticResult CA2025 =
		DiagnosticResult.CompilerWarning("CA2025").WithSeverity(DiagnosticSeverity.Warning);

	[Test]
	public static async Task SuppressWhenCallbackIsUsedAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Threading.Tasks;
			
			public sealed class Disposable
				: IDisposable
			{
				private bool disposedValue;

				private void Dispose(bool disposing)
				{
					if (!this.disposedValue)
					{
						if (disposing) { }

						this.disposedValue = true;
					}
				}

				public void Dispose()
				{
					this.Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}
			}

			public sealed class TestHandler
				: Handler<Func<Task<Disposable>>, Task<Disposable>>
			{ }

			public sealed class TestAdornments
				: Adornments<TestAdornments, TestHandler, Func<Task<Disposable>>, Task<Disposable>>
			{
				public TestAdornments(TestHandler handler)
					: base(handler) { }
			}

			public static class Tester
			{
				public static void Test()
				{
					using var disposable = new Disposable();
					var adornments = new TestAdornments(new());
					adornments.Callback(() => Task.FromResult({|#0:disposable|}));
				}
			}
			""";

		var expected = new[]
		{
			CA2025.WithLocation(0).WithIsSuppressed(true),
		};

		await TestAssistants.RunSuppressorAsync<IDisposableInstancesIntoTasksSuppressor, DoNotPassDisposablesIntoUnawaitedTasksAnalyzer>(code, expected);
	}

	[Test]
	public static async Task SuppressWhenReturnValueIsUsedAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Threading.Tasks;
			
			public sealed class Disposable
				: IDisposable
			{
				private bool disposedValue;

				private void Dispose(bool disposing)
				{
					if (!this.disposedValue)
					{
						if (disposing) { }

						this.disposedValue = true;
					}
				}

				public void Dispose()
				{
					this.Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}
			}

			public sealed class TestHandler
				: Handler<Func<Task<Disposable>>, Task<Disposable>>
			{ }

			public sealed class TestAdornments
				: Adornments<TestAdornments, TestHandler, Func<Task<Disposable>>, Task<Disposable>>
			{
				public TestAdornments(TestHandler handler)
					: base(handler) { }
			}

			public static class Tester
			{
				public static void Test()
				{
					using var disposable = new Disposable();
					var adornments = new TestAdornments(new());
					adornments.ReturnValue(Task.FromResult({|#0:disposable|}));
				}
			}
			""";

		var expected = new[]
		{
			CA2025.WithLocation(0).WithIsSuppressed(true),
		};

		await TestAssistants.RunSuppressorAsync<IDisposableInstancesIntoTasksSuppressor, DoNotPassDisposablesIntoUnawaitedTasksAnalyzer>(code, expected);
	}

	[Test]
	public static async Task SuppressWhenTaskIsReturnedFromArbitraryMethodAsync()
	{
		var code =
			"""
			using System;
			using System.Threading.Tasks;
			
			public sealed class Disposable
				: IDisposable
			{
				private bool disposedValue;

				private void Dispose(bool disposing)
				{
					if (!this.disposedValue)
					{
						if (disposing) { }

						this.disposedValue = true;
					}
				}

				public void Dispose()
				{
					this.Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}
			}

			public static class Tester
			{
				public static Task<Disposable> TestAsync()
				{
					using var disposable = new Disposable();				
					return Task.FromResult({|#0:disposable|});
				}
			}
			""";

		var expected = new[]
		{
			CA2025.WithLocation(0).WithIsSuppressed(false),
		};

		await TestAssistants.RunSuppressorAsync<IDisposableInstancesIntoTasksSuppressor, DoNotPassDisposablesIntoUnawaitedTasksAnalyzer>(code, expected);
	}
}