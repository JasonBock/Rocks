using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.SuppressCA2025TestTypes;

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

public interface IReturnDisposableTask
{
	Task<Disposable> DoTaskAsync();
	ValueTask<Disposable> DoValueTaskAsync();
}

internal static class SuppressCA2025Tests
{
	[Test]
	public static async Task CreateTaskUsingReturnValueAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnDisposableTaskCreateExpectations>();
		using var disposable = new Disposable();
		expectations.Setups.DoTaskAsync().ReturnValue(Task.FromResult(disposable));

		var mock = expectations.Instance();
		var mockDisposable = await mock.DoTaskAsync();
		Assert.That(mockDisposable, Is.SameAs(disposable));
	}

	[Test]
	public static async Task CreateTaskUsingCallbackAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnDisposableTaskCreateExpectations>();
		using var disposable = new Disposable();
		expectations.Setups.DoTaskAsync().Callback(() => Task.FromResult(disposable));

		var mock = expectations.Instance();
		var mockDisposable = await mock.DoTaskAsync();
		Assert.That(mockDisposable, Is.SameAs(disposable));
	}

	[Test]
	public static async Task CreateValueTaskUsingReturnValueAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnDisposableTaskCreateExpectations>();
		using var disposable = new Disposable();
		expectations.Setups.DoValueTaskAsync().ReturnValue(ValueTask.FromResult(disposable));

		var mock = expectations.Instance();
		var mockDisposable = await mock.DoValueTaskAsync();
		Assert.That(mockDisposable, Is.SameAs(disposable));
	}

	[Test]
	public static async Task CreateValueTaskUsingCallbackAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnDisposableTaskCreateExpectations>();
		using var disposable = new Disposable();
		expectations.Setups.DoValueTaskAsync().Callback(() => ValueTask.FromResult(disposable));

		var mock = expectations.Instance();
		var mockDisposable = await mock.DoValueTaskAsync();
		Assert.That(mockDisposable, Is.SameAs(disposable));
	}
}