using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassGenericProperty<T>
{
	public virtual List<string> Values => default!;
	public virtual T Data => default!;
}

public class ClassGenericPropertyGetAndInit<T>
{
	public virtual List<string> Values { get => default!; init { } }
	public virtual T Data { get => default!; init { } }
}

public static class ClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<ClassGenericProperty<int>>();
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
		var rock = Rock.Create<ClassGenericPropertyGetAndInit<int>>();
		rock.Properties().Getters().Values().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.Values;

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<ClassGenericProperty<int>>().Instance();
		var value = chunk.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var chunk = Rock.Make<ClassGenericPropertyGetAndInit<int>>().Instance();
		var value = chunk.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var rock = Rock.Create<ClassGenericProperty<int>>();
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
		var rock = Rock.Create<ClassGenericPropertyGetAndInit<int>>();
		rock.Properties().Getters().Data().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.Data;

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var chunk = Rock.Make<ClassGenericProperty<int>>().Instance();
		var value = chunk.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var chunk = Rock.Make<ClassGenericPropertyGetAndInit<int>>().Instance();
		var value = chunk.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}