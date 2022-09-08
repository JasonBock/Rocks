using NUnit.Framework;

namespace Rocks.IntegrationTests;

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