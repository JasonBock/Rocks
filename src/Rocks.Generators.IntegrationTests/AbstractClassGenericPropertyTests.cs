using NUnit.Framework;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public abstract class AbstractClassGenericProperty<T>
	{
		public abstract List<string> Values { get; }
		public abstract T Data { get; }
	}

	public static class AbstractClassGenericPropertyTests
	{
		[Test]
		public static void MockUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<AbstractClassGenericProperty<int>>();
			rock.Properties().Getters().Values().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.Values;

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
			var rock = Rock.Create<AbstractClassGenericProperty<int>>();
			rock.Properties().Getters().Data().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.Data;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}
	}
}