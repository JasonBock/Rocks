using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ExplicitInterfaceImplementationTestTypes;

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
		using var context = new RockContext(); 
		var expectations = context.Create<IIterableCreateExpectations<string>>();
		expectations.Setups.GetIterator();
		expectations.Setups.ExplicitForIIterable.GetIterator();

		var mock = expectations.Instance();
		mock.GetIterator();
		_ = ((IIterable)mock).GetIterator();
	}

	[Test]
	public static void CreateMethod()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IExplicitInterfaceImplementationCreateExpectations>();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne.A();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo.A();

		var mock = expectations.Instance();
		((IExplicitInterfaceImplementationOne)mock).A();
		((IExplicitInterfaceImplementationTwo)mock).A();
	}

	[Test]
	public static void MakeMethod()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock).A(), Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock).A(), Throws.Nothing);
		}
	}

	[Test]
	public static void CreateProperty()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IExplicitInterfaceImplementationCreateExpectations>();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne.B.Gets();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne.B.Sets(Arg.Any<int>());
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo.B.Gets();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo.B.Sets(Arg.Any<int>());

		var mock = expectations.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).B;
		((IExplicitInterfaceImplementationOne)mock).B = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).B;
		((IExplicitInterfaceImplementationTwo)mock).B = twoValue;
	}

	[Test]
	public static void CreatePropertyWithInit()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IExplicitInterfaceImplementationCreateExpectations>();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne.D.Gets();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo.D.Gets();

		var mock = expectations.Instance();
		_ = ((IExplicitInterfaceImplementationOne)mock).D;
		_ = ((IExplicitInterfaceImplementationTwo)mock).D;
	}

	[Test]
	public static void MakeProperty()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).B;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).B;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(oneValue, Is.Default);
			Assert.That(twoValue, Is.Default);
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock).B = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock).B = twoValue, Throws.Nothing);
		}
	}

	[Test]
	public static void MakePropertyWithInit()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock).D;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock).D;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(oneValue, Is.Default);
			Assert.That(twoValue, Is.Default);
		}
	}

	[Test]
	public static void CreateIndexer()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IExplicitInterfaceImplementationCreateExpectations>();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne[Arg.Any<int>()].Gets();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationOne[Arg.Any<int>()].Sets(Arg.Any<int>());
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo[Arg.Any<int>()].Gets();
		expectations.Setups.ExplicitForIExplicitInterfaceImplementationTwo[Arg.Any<int>()].Sets(Arg.Any<int>());

		var mock = expectations.Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock)[3];
		((IExplicitInterfaceImplementationOne)mock)[3] = oneValue;
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock)[3];
		((IExplicitInterfaceImplementationTwo)mock)[3] = twoValue;
	}

	[Test]
	public static void MakeIndexer()
	{
		var mock = new IExplicitInterfaceImplementationMakeExpectations().Instance();
		var oneValue = ((IExplicitInterfaceImplementationOne)mock)[3];
		var twoValue = ((IExplicitInterfaceImplementationTwo)mock)[3];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(oneValue, Is.Default);
			Assert.That(twoValue, Is.Default);
			Assert.That(() => ((IExplicitInterfaceImplementationOne)mock)[3] = oneValue, Throws.Nothing);
			Assert.That(() => ((IExplicitInterfaceImplementationTwo)mock)[3] = twoValue, Throws.Nothing);
		}
	}
}