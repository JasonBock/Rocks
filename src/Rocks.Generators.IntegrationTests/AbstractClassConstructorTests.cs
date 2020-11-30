using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public abstract class AbstractClassConstructor
	{
		protected AbstractClassConstructor(string stringData) => 
			this.StringData = stringData;
		public AbstractClassConstructor(int intData) => 
			this.IntData = intData;
		
		public abstract int NoParameters();

		public int IntData { get;  }
		public string? StringData { get; }
	}

	public static class AbstractClassConstructorTests
	{
		[Test]
		public static void MockWithNoParametersAndPublicConstructor()
		{
			var rock = Rock.Create<AbstractClassConstructor>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance(3);
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(chunk.IntData, Is.EqualTo(3));
				Assert.That(chunk.StringData, Is.Null);
			});
		}

		[Test]
		public static void MockWithNoParametersAndProtectedConstructor()
		{
			var rock = Rock.Create<AbstractClassConstructor>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance("b");
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(chunk.IntData, Is.EqualTo(default(int)));
				Assert.That(chunk.StringData, Is.EqualTo("b"));
			});
		}
	}
}