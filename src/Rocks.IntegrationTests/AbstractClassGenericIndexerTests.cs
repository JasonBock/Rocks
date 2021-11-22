using NUnit.Framework;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassGenericIndexer<T>
{
	public abstract List<string> this[int a] { get; }
	public abstract int this[int a, T b] { get; }
	public abstract T this[string a] { get; }
}

public abstract class AbstractClassGenericIndexerGetAndInit<T>
{
	public abstract List<string> this[int a] { get; init; }
	public abstract int this[int a, T b] { get; init; }
	public abstract T this[string a] { get; init; }
}

public static class AbstractClassGenericIndexerTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
		rock.Indexers().Getters().This(4).Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk[4];

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		rock.Indexers().Getters().This(4).Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk[4];

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = chunk[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = chunk[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
		rock.Indexers().Getters().This(4, 5).Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk[4, 5];

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		rock.Indexers().Getters().This(4, 5).Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk[4, 5];

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = chunk[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = chunk[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericIndexer<int>>();
		rock.Indexers().Getters().This("b").Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk["b"];

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var rock = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		rock.Indexers().Getters().This("b").Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk["b"];

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = chunk["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var chunk = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = chunk["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}