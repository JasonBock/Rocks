using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace Rocks.Analysis.IntegrationTests.AsynchronousTestTypes;

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

		using var context = new RockContext();
		var expectations = context.Create<AsyncEnumerationCreateExpectations>();
		expectations.Setups.GetRecordsAsync(Arg.IsDefault<CancellationToken>()).ReturnValue(ReturnsAsyncIterator());
		var mock = expectations.Instance();

		var values = new List<string>();

		await foreach (var value in mock.GetRecordsAsync())
		{
			values.Add(value);
		}

		using (Assert.EnterMultipleScope())
		{
			Assert.That(values, Has.Count.EqualTo(1));
			Assert.That(values[0], Is.EqualTo("x"));
		}
	}

	[Test]
	public static async Task CreateAsynchronousMethodsAsync()
	{
		const int returnValue = 3;
		using var context = new RockContext();
		var expectations = context.Create<IAmAsynchronousCreateExpectations>();
		expectations.Setups.FooAsync().ReturnValue(Task.CompletedTask);
		expectations.Setups.FooReturnAsync().ReturnValue(Task.FromResult(returnValue));
		expectations.Setups.ValueFooAsync().ReturnValue(new ValueTask());
		expectations.Setups.ValueFooReturnAsync().ReturnValue(new ValueTask<int>(returnValue));

		var mock = expectations.Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);
		await mock.ValueFooAsync().ConfigureAwait(false);
		var valueValue = await mock.ValueFooReturnAsync().ConfigureAwait(false);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.EqualTo(returnValue));
			Assert.That(valueValue, Is.EqualTo(returnValue));
		}
	}

	[Test]
	public static async Task CreateAsynchronousMethodsWithAsyncCallbackAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IAmAsynchronousCreateExpectations>();
		expectations.Setups.FooAsync().Callback(async () => await Task.Delay(10).ConfigureAwait(false));
		expectations.Setups.FooReturnAsync().Callback(async () =>
		{
			await Task.Delay(10).ConfigureAwait(false);
			return 3;
		});

		var mock = expectations.Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static async Task MakeAsynchronousMethodsAsync()
	{
		var mock = new IAmAsynchronousMakeExpectations().Instance();
		await mock.FooAsync().ConfigureAwait(false);
		var value = await mock.FooReturnAsync().ConfigureAwait(false);
		await mock.ValueFooAsync().ConfigureAwait(false);
		var valueValue = await mock.ValueFooReturnAsync().ConfigureAwait(false);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.Default);
			Assert.That(valueValue, Is.Default);
		}
	}
}