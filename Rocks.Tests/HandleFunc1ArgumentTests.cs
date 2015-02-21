using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc1ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(default(int)));
			rock.Handle(_ => _.ValueTarget(default(int)));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1);
			chunk.ValueTarget(10);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc<int, string>(_ => _.ReferenceTarget(default(int)),
				a => { argumentA = a; return stringReturnValue; });
			rock.HandleFunc<int, int>(_ => _.ValueTarget(default(int)),
				a => { argumentA = a; return intReturnValue; });

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(default(int)), 2);
			rock.Handle(_ => _.ValueTarget(default(int)), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1);
			chunk.ReferenceTarget(1);
			chunk.ValueTarget(10);
			chunk.ValueTarget(10);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc<int, string>(_ => _.ReferenceTarget(default(int)),
				a => { argumentA = a; return stringReturnValue; }, 2);
			rock.HandleFunc<int, int>(_ => _.ValueTarget(default(int)),
				a => { argumentA = a; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(100), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(100, argumentA, nameof(argumentA));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(1000), nameof(chunk.ValueTarget));
			Assert.AreEqual(1000, argumentA, nameof(argumentA));

			rock.Verify();
		}
	}

	public interface IHandleFunc1ArgumentTests
	{
		string ReferenceTarget(int a);
		int ValueTarget(int a);
	}
}