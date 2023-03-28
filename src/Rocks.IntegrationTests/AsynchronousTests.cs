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
	public static async Task CreateAsyncInteratorAsync()
	{
		static async IAsyncEnumerable<string> ReturnsAsyncIterator()
		{
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
		 await Task.CompletedTask;
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
		 yield return "x";
		}

		var expectations = Rock.Create<AsyncEnumeration>();
		expectations.Methods().GetRecordsAsync(Arg.IsDefault<CancellationToken>()).Returns(ReturnsAsyncIterator());
		var mock = expectations.Instance();

		var values = new List<string>();

		await foreach(var value in mock.GetRecordsAsync())
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
	public static async Task CreateAsynchronousMethodsAsync()
	{
		const int returnValue = 3;
		var expectations = Rock.Create<IAmAsynchronous>();
		expectations.Methods().FooAsync().Returns(Task.CompletedTask);
		expectations.Methods().FooReturnAsync().Returns(Task.FromResult(returnValue));
		expectations.Methods().ValueFooAsync().Returns(new ValueTask());
		expectations.Methods().ValueFooReturnAsync().Returns(new ValueTask<int>(returnValue));

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
	public static async Task CreateAsynchronousMethodsWithAsyncCallbackAsync()
	{
		var expectations = Rock.Create<IAmAsynchronous>();
		expectations.Methods().FooAsync().Callback(async () => await Task.Delay(10).ConfigureAwait(false));
		expectations.Methods().FooReturnAsync().Callback(async () =>
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
	public static async Task MakeAsynchronousMethodsAsync()
	{
		var mock = Rock.Make<IAmAsynchronous>().Instance();
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