using NUnit.Framework;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public abstract class AbstractClassGenericIndexer<T>
	{
		public abstract List<string> this[int a] { get; }
		public abstract int this[int a, T b] { get; }
		public abstract T this[string a] { get; }
	}

	public static class AbstractClassGenericIndexerTests
	{
		[Test]
		public static void MockUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
			rock.Indexers().Getters().This(4).Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk[4];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.SameAs(returnValue));
			});
		}

		[Test]
		public static void MockUsingGenericTypeParameter()
		{
			var returnValue = 3;
			var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
			rock.Indexers().Getters().This(4, 5).Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk[4, 5];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockUsingGenericTypeParameterAsReturn()
		{
			var returnValue = 3;
			var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
			rock.Indexers().Getters().This("b").Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk["b"];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}
	}
}