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
	[RockCreate<IIterable<string>>]
	public static void CreateDifferByReturnTypeOnly()
	{
		var expectations = new IIterableOfstringCreateExpectations();
		expectations.Methods.GetIterator();
		expectations.ExplicitMethodsForIIterable.GetIterator();

		var mock = expectations.Instance();
		mock.GetIterator();
		((IIterable)mock).GetIterator();

		expectations.Verify();
	}

	[Test]
	[RockCreate<IExplicitInterfaceImplementation>]
	public static void CreateMethod()
	{
		var expectations = new IExplicitInterfaceImplementationCreateExpectations();
		expectations.ExplicitMethodsForIExplicitInterfaceImplementationOne.A();
		expectations.ExplicitMethodsForIExplicitInterfaceImplementationTwo.A();

		var mock = expectations.Instance();
		((IExplicitInterfaceImplementationOne)mock).A();
		((IExplicitInterfaceImplementationTwo)mock).A();

		expectations.Verify();
	}

	[Test]
	[RockMake<IExplicitInterfaceImplementation>]
	public static void MakeMethod()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();

		Assert.Multiple(() =>
		{
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock).A(), Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock).A(), Throws.Nothing);
		});
	}

	[Test]
	[RockCreate<IExplicitInterfaceImplementation>]
	public static void CreateProperty()
	{
		var expectations = new IExplicitInterfaceImplementationCreateExpectations();
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationOne.Getters.B();
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationOne.Setters.B(Arg.Any<int>());
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationTwo.Getters.B();
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationTwo.Setters.B(Arg.Any<int>());

		var mock = expectations.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).B;
		((IExplicitInterfaceImplementationOne)mock).B = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).B;
		((IExplicitInterfaceImplementationTwo)mock).B = twoValue;

		expectations.Verify();
	}

	[Test]
	[RockCreate<IExplicitInterfaceImplementation>]
	public static void CreatePropertyWithInit()
	{
		var expectations = new IExplicitInterfaceImplementationCreateExpectations();
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationOne.Getters.D();
		expectations.ExplicitPropertiesForIExplicitInterfaceImplementationTwo.Getters.D();

		var mock = expectations.Instance();
		_ = ((IExplicitInterfaceImplementationOne)mock).D;
		_ = ((IExplicitInterfaceImplementationTwo)mock).D;

		expectations.Verify();
	}

	[Test]
	[RockMake<IExplicitInterfaceImplementation>]
	public static void MakeProperty()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).B;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).B;

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock).B = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock).B = twoValue, Throws.Nothing);
		});
	}

	[Test]
	[RockMake<IExplicitInterfaceImplementation>]
	public static void MakePropertyWithInit()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).D;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).D;

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
		});
	}

	[Test]
	[RockCreate<IExplicitInterfaceImplementation>]
	public static void CreateIndexer()
	{
		var expectations = new IExplicitInterfaceImplementationCreateExpectations();
		expectations.ExplicitIndexersForIExplicitInterfaceImplementationOne.Getters.This(Arg.Any<int>());
		expectations.ExplicitIndexersForIExplicitInterfaceImplementationOne.Setters.This(Arg.Any<int>(), Arg.Any<int>());
		expectations.ExplicitIndexersForIExplicitInterfaceImplementationTwo.Getters.This(Arg.Any<int>());
		expectations.ExplicitIndexersForIExplicitInterfaceImplementationTwo.Setters.This(Arg.Any<int>(), Arg.Any<int>());

		var mock = expectations.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock)[3];
		((IExplicitInterfaceImplementationOne)mock)[3] = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock)[3];
		((IExplicitInterfaceImplementationTwo)mock)[3] = twoValue;

		expectations.Verify();
	}

	[Test]
	[RockMake<IExplicitInterfaceImplementation>]
	public static void MakeIndexer()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock)[3];
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock)[3];

		Assert.Multiple(() =>
		{
			Assert.That(oneValue, Is.EqualTo(default(int)));
			Assert.That(twoValue, Is.EqualTo(default(int)));
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock)[3] = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock)[3] = twoValue, Throws.Nothing);
		});
	}
}