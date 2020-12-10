using NUnit.Framework;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceGenericProperty<T>
	{
		List<string> Values { get; }
		T Data { get; }
	}

	public static class InterfaceGenericPropertyTests
	{
		[Test]
		public static void CreateUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<IInterfaceGenericProperty<int>>();
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
		public static void MakeUsingGenericType()
		{
			var chunk = Rock.Make<IInterfaceGenericProperty<int>>().Instance();
			var value = chunk.Values;

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.SameAs(default(List<string>)));
			});
		}

		[Test]
		public static void CreateUsingGenericTypeParameter()
		{
			var returnValue = 3;
			var rock = Rock.Create<IInterfaceGenericProperty<int>>();
			rock.Properties().Getters().Data().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.Data;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MakeUsingGenericTypeParameter()
		{
			var chunk = Rock.Make<IInterfaceGenericProperty<int>>().Instance();
			var value = chunk.Data;

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}
	}
}