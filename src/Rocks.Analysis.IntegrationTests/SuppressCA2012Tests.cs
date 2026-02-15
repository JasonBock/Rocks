using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.SuppressCA2012TestTypes;

public interface IReturnValueTask
{
	ValueTask<string> DoAsync();
}

internal static class SuppressCA2012Tests
{
	[Test]
	public static async Task CreateUsingReturnValueAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnValueTaskCreateExpectations>();
		expectations.Setups.DoAsync().ReturnValue(ValueTask.FromResult("hi"));

		var mock = expectations.Instance();
		Assert.That(await mock.DoAsync(), Is.EqualTo("hi"));
	}

	[Test]
	public static async Task CreateUsingCallbackAsync()
	{
		using var context = new RockContext();
		var expectations = context.Create<IReturnValueTaskCreateExpectations>();
		expectations.Setups.DoAsync().Callback(() => ValueTask.FromResult("hi"));

		var mock = expectations.Instance();
		Assert.That(await mock.DoAsync(), Is.EqualTo("hi"));
	}
}