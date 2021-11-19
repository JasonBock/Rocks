using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassGenericProperty<T>
{
	public virtual List<string> Values => default!;
	public virtual T Data => default!;
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
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<ClassGenericProperty<int>>().Instance();
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
	public static void MakeUsingGenericTypeParameter()
	{
		var chunk = Rock.Make<ClassGenericProperty<int>>().Instance();
		var value = chunk.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}