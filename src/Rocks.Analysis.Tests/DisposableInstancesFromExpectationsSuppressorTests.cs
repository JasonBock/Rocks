using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Microsoft.NetCore.Analyzers.Runtime;

namespace Rocks.Analysis.Tests;

public static class DisposableInstancesFromExpectationsSuppressorTests
{
	private static readonly DiagnosticResult CA2000 =
		DiagnosticResult.CompilerWarning("CA2000").WithSeverity(DiagnosticSeverity.Warning);

	[Test]
	public static async Task SuppressWhenDisposableIsReturnedFromInstanceInvocationAsync()
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

			public class MyExpectations
				: Expectations
			{
				public override void Verify() { }

				public Disposable Instance() => new Disposable();
			}

			public static class Tester
			{
				public static void Test()
				{
					var expectations = new MyExpectations();
					_ = {|#0:expectations.Instance()|};
				}
			}
			""";

		var expected = new[]
		{
			CA2000.WithLocation(0).WithIsSuppressed(true),
		};

		await TestAssistants.RunSuppressorAsync<DisposableInstancesFromExpectationsSuppressor, DisposeObjectsBeforeLosingScope>(code, expected);
	}

	[Test]
	public static async Task SuppressWhenDisposableIsReturnedFromInstanceInvocationNotOnExpectationAsync()
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

			public class MyExpectations
			{
				public Disposable Instance() => new Disposable();
			}

			public static class Tester
			{
				public static void Test()
				{
					var expectations = new MyExpectations();
					_ = {|#0:expectations.Instance()|};
				}
			}
			""";

		var expected = new[]
		{
			CA2000.WithLocation(0).WithIsSuppressed(false),
		};

		await TestAssistants.RunSuppressorAsync<DisposableInstancesFromExpectationsSuppressor, DisposeObjectsBeforeLosingScope>(code, expected);
	}
}