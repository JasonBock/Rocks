using NUnit.Framework;
using System.Threading.Tasks;

namespace Rocks.IntegrationTests
{
	public interface IAmAsynchronous
	{
		Task FooAsync();
		Task<int> FooReturnAsync();
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

			var chunk = rock.Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		[Ignore("Need to return Task.CompletedTask or Task.FromResult")]
		public static async Task MakeAsynchronousMethods()
		{
			var chunk = Rock.Make<IAmAsynchronous>().Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}
	}
}