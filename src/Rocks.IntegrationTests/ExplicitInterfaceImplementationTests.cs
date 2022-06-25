using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IExplicitInterfaceImplementationOne
{
	void A();
	int B { get; set; }
	int this[int x] { get; set; }
	event EventHandler C;
	int D { get; init; }
}

public interface IExplicitInterfaceImplementationTwo
{
	void A();
	int B { get; set; }
	int this[int x] { get; set; }
	event EventHandler C;
	int D { get; init; }
}

public interface IExplicitInterfaceImplementation
	: IExplicitInterfaceImplementationOne, IExplicitInterfaceImplementationTwo
{ }

public interface IIterator
{
	void Iterate();
}

public interface IIterator<out T>
	: IIterator
{
	new T Iterate();
}

public interface IIterable
{
	IIterator GetIterator();
}

public interface IIterable<out T>
	: IIterable
{
	new IIterator<T> GetIterator();
}

public static class ExplicitInterfaceImplementationTests
{
	[Test]
	public static void CreateDifferByReturnTypeOnly()
	{
		var rock = Rock.Create<IIterable<string>>();
		rock.Methods().GetIterator();
		rock.ExplicitMethodsForIIterable().GetIterator();

		var chunk = rock.Instance();
		chunk.GetIterator();
		((IIterable)chunk).GetIterator();

		rock.Verify();
	}

	[Test]
	public static void CreateMethod()
	{
		var rock = Rock.Create<IExplicitInterfaceImplementation>();
		rock.ExplicitMethodsForIExplicitInterfaceImplementationOne().A();
		rock.ExplicitMethodsForIExplicitInterfaceImplementationTwo().A();

		var chunk = rock.Instance();
		((IExplicitInterfaceImplementationOne)chunk).A();
		((IExplicitInterfaceImplementationTwo)chunk).A();

		rock.Verify();
	}

	[Test]
	public static void MakeMethod()
	{
		var chunk = Rock.Make<IExplicitInterfaceImplementation>().Instance();

		Assert.Multiple(() =>
		{
			Assert.That(() => ((IExplicitInterfaceImplementationOne)chunk).A(), Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)chunk).A(), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateProperty()
	{
		var rock = Rock.Create<IExplicitInterfaceImplementation>();
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationOne().Getters().B();
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationOne().Setters().B(Arg.Any<int>());
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationTwo().Getters().B();
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationTwo().Setters().B(Arg.Any<int>());

		var chunk = rock.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)chunk).B;
		((IExplicitInterfaceImplementationOne)chunk).B = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)chunk).B;
		((IExplicitInterfaceImplementationTwo)chunk).B = twoValue;

		rock.Verify();
	}

	[Test]
	public static void CreatePropertyWithInit()
	{
		var rock = Rock.Create<IExplicitInterfaceImplementation>();
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationOne().Getters().D();
		rock.ExplicitPropertiesForIExplicitInterfaceImplementationTwo().Getters().D();

		var chunk = rock.Instance();
		_ = ((IExplicitInterfaceImplementationOne)chunk).D;
		_ = ((IExplicitInterfaceImplementationTwo)chunk).D;

		rock.Verify();
	}

	[Test]
	public static void MakeProperty()
	{
		var chunk = Rock.Make<IExplicitInterfaceImplementation>().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)chunk).B;
		var twoValue = ((IExplicitInterfaceImplementationTwo)chunk).B;

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
			Assert.That(() => ((IExplicitInterfaceImplementationOne)chunk).B = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)chunk).B = twoValue, Throws.Nothing);
		});
	}

	[Test]
	public static void MakePropertyWithInit()
	{
		var chunk = Rock.Make<IExplicitInterfaceImplementation>().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)chunk).D;
		var twoValue = ((IExplicitInterfaceImplementationTwo)chunk).D;

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
		});
	}

	[Test]
	public static void CreateIndexer()
	{
		var rock = Rock.Create<IExplicitInterfaceImplementation>();
		rock.ExplicitIndexersForIExplicitInterfaceImplementationOne().Getters().This(Arg.Any<int>());
		rock.ExplicitIndexersForIExplicitInterfaceImplementationOne().Setters().This(Arg.Any<int>(), Arg.Any<int>());
		rock.ExplicitIndexersForIExplicitInterfaceImplementationTwo().Getters().This(Arg.Any<int>());
		rock.ExplicitIndexersForIExplicitInterfaceImplementationTwo().Setters().This(Arg.Any<int>(), Arg.Any<int>());

		var chunk = rock.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)chunk)[3];
		((IExplicitInterfaceImplementationOne)chunk)[3] = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)chunk)[3];
		((IExplicitInterfaceImplementationTwo)chunk)[3] = twoValue;

		rock.Verify();
	}

	[Test]
	public static void MakeIndexer()
	{
		var chunk = Rock.Make<IExplicitInterfaceImplementation>().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)chunk)[3];
		var twoValue = ((IExplicitInterfaceImplementationTwo)chunk)[3];

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
			Assert.That(() => ((IExplicitInterfaceImplementationOne)chunk)[3] = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)chunk)[3] = twoValue, Throws.Nothing);
		});
	}
}