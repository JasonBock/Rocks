using NUnit.Framework;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassGenericProperty<T>
{
	public abstract List<string> Values { get; }
	public abstract T Data { get; }
}

public abstract class AbstractClassGenericPropertyGetAndInit<T>
{
	public abstract List<string> Values { get; init; }
	public abstract T Data { get; init; }
}

public static class AbstractClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<AbstractClassGenericProperty<int>>();
		rock.Properties().Getters().Values().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.Values;

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<AbstractClassGenericPropertyGetAndInit<int>>();
		rock.Properties().Getters().Values().Returns(returnValue);

		var chunk = rock.Instance(null);
		var value = chunk.Values;

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<AbstractClassGenericProperty<int>>().Instance();
		var value = chunk.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var chunk = Rock.Make<AbstractClassGenericPropertyGetAndInit<int>>().Instance();
		var value = chunk.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericProperty<int>>();
		rock.Properties().Getters().Data().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.Data;

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericPropertyGetAndInit<int>>();
		rock.Properties().Getters().Data().Returns(returnValue);

		var chunk = rock.Instance(null);
		var value = chunk.Data;

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var chunk = Rock.Make<AbstractClassGenericProperty<int>>().Instance();
		var value = chunk.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var chunk = Rock.Make<AbstractClassGenericPropertyGetAndInit<int>>().Instance();
		var value = chunk.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}