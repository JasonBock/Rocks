using NUnit.Framework;
using System.Threading.Tasks;

namespace Rocks.IntegrationTests
{
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
		public static async Task CreateAsynchronousMethods()
		{
			const int returnValue = 3;
			var rock = Rock.Create<IAmAsynchronous>();
			rock.Methods().FooAsync().Returns(Task.CompletedTask);
			rock.Methods().FooReturnAsync().Returns(Task.FromResult(returnValue));
			rock.Methods().ValueFooAsync().Returns(new ValueTask());
			rock.Methods().ValueFooReturnAsync().Returns(new ValueTask<int>(returnValue));

			var chunk = rock.Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);
			await chunk.ValueFooAsync().ConfigureAwait(false);
			var valueValue = await chunk.ValueFooReturnAsync().ConfigureAwait(false);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
				Assert.That(valueValue, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static async Task CreateAsynchronousMethodsWithAsyncCallback()
		{
			var rock = Rock.Create<IAmAsynchronous>();
			rock.Methods().FooAsync().Callback(async () => await Task.Delay(10));
			rock.Methods().FooReturnAsync().Callback(async () =>
			{
				await Task.Delay(10);
				return 3;
			});

			var chunk = rock.Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static async Task MakeAsynchronousMethods()
		{
			var chunk = Rock.Make<IAmAsynchronous>().Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);
			await chunk.ValueFooAsync().ConfigureAwait(false);
			var valueValue = await chunk.ValueFooReturnAsync().ConfigureAwait(false);

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
				Assert.That(valueValue, Is.EqualTo(default(int)));
			});
		}
	}
}