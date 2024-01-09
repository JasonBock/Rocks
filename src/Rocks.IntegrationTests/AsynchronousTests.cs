using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace Rocks.IntegrationTests;

public class AsyncEnumeration
{
	public virtual async IAsyncEnumerable<string> GetRecordsAsync(
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
		await Task.CompletedTask;
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
		yield return "y";
	}
}

public interface IAmAsynchronous
{
	Task FooAsync();
	ValueTask ValueFooAsync();
	Task<int> FooReturnAsync();
	ValueTask<int> ValueFooReturnAsync();
}

public static class AsynchronousTests
{
	[Test]
	[RockCreate<AsyncEnumeration>]
	public static async Task CreateAsyncInteratorAsync()
	{
		static async IAsyncEnumerable<string> ReturnsAsyncIterator()
		{
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
			await Task.CompletedTask;
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
			yield return "x";
		}

		var expectations = new AsyncEnumerationCreateExpectations();
		expectations.Methods.GetRecordsAsync(Arg.IsDefault<CancellationToken>()).ReturnValue(ReturnsAsyncIterator());
		var mock = expectations.Instance();

		var values = new List<string>();

		await foreach (var value in mock.GetRecordsAsync())
		{
			values.Add(value);
		}

		Assert.Multiple(() =>
		{
			Assert.That(values, Has.Count.EqualTo(1));
			Assert.That(values[0], Is.EqualTo("x"));
		});

		expectations.Verify();
	}

	[Test]
	[RockCreate<IAmAsynchronous>]
	public static async Task CreateAsynchronousMethodsAsync()
	{
		const int returnValue = 3;
		var expectations = new IAmAsynchronousCreateExpectations();
		expectations.Methods.FooAsync().ReturnValue(Task.CompletedTask);
		expectations.Methods.FooReturnAsync().ReturnValue(Task.FromResult(returnValue));
		expectations.Methods.ValueFooAsync().ReturnValue(new ValueTask());
		expectations.Methods.ValueFooReturnAsync().ReturnValue(new ValueTask<int>(returnValue));

		var mock = expectations.Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);
		await mock.ValueFooAsync().ConfigureAwait(false);
		var valueValue = await mock.ValueFooReturnAsync().ConfigureAwait(false);

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(returnValue));
			Assert.That(valueValue, Is.EqualTo(returnValue));
		});
	}

	[Test]
	[RockCreate<IAmAsynchronous>]
	public static async Task CreateAsynchronousMethodsWithAsyncCallbackAsync()
	{
		var expectations = new IAmAsynchronousCreateExpectations();
		expectations.Methods.FooAsync().Callback(async () => await Task.Delay(10).ConfigureAwait(false));
		expectations.Methods.FooReturnAsync().Callback(async () =>
		{
			await Task.Delay(10).ConfigureAwait(false);
			return 3;
		});

		var mock = expectations.Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	[RockMake<IAmAsynchronous>]
	public static async Task MakeAsynchronousMethodsAsync()
	{
		var mock = new IAmAsynchronousMakeExpectations().Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);
		await mock.ValueFooAsync().ConfigureAwait(false);
		var valueValue = await mock.ValueFooReturnAsync().ConfigureAwait(false);

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(valueValue, Is.EqualTo(default(int)));
		});
	}
}