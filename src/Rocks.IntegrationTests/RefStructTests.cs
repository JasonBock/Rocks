using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IHaveInAndOutSpan
{
	Span<int> Foo(Span<int> values);
	Span<byte> Values { get; set; }
}

public interface IReturnSpan
{
	Span<int> GetRandomData();
	Span<byte> Values { get; }
}

public interface IHaveSpan
{
	void Foo(Span<int> values);
	void Bar<T>(Span<T> values);
}

public static class RefStructTests
{
	[Test]
	public static void CreateInAndOut()
	{
		var expectations = Rock.Create<IHaveInAndOutSpan>();
		expectations.Methods().Foo(new()).Returns(() => new[] { 1, 2 }.AsSpan());
		expectations.Properties().Getters().Values().Returns(() => new byte[] { 3, 4, 5 }.AsSpan());

		var mock = expectations.Instance();

		Assert.Multiple(() =>
		{
			var data = mock.Foo(default);
			Assert.That(data.Length, Is.EqualTo(2));
			Assert.That(data[0], Is.EqualTo(1));
			Assert.That(data[1], Is.EqualTo(2));
			var values = mock.Values;
			Assert.That(values.Length, Is.EqualTo(3));
			Assert.That(values[0], Is.EqualTo(3));
			Assert.That(values[1], Is.EqualTo(4));
			Assert.That(values[2], Is.EqualTo(5));
		});

		expectations.Verify();
	}

	[Test]
	public static void MakeInAndOut()
	{
		var mock = Rock.Make<IHaveInAndOutSpan>().Instance();

#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
		Assert.Multiple(() =>
		{
			Assert.That(mock.Foo(default) == default, Is.True);
			Assert.That(mock.Values == default, Is.True);
		});
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
	}

	[Test]
	public static void CreateWithReturnValues()
	{
		var expectations = Rock.Create<IReturnSpan>();
		expectations.Methods().GetRandomData().Returns(() => new[] { 1, 2 }.AsSpan());
		expectations.Properties().Getters().Values().Returns(() => new byte[] { 3, 4, 5 }.AsSpan());

		var mock = expectations.Instance();

		Assert.Multiple(() =>
		{
			var data = mock.GetRandomData();
			Assert.That(data.Length, Is.EqualTo(2));
			Assert.That(data[0], Is.EqualTo(1));
			Assert.That(data[1], Is.EqualTo(2));
			var values = mock.Values;
			Assert.That(values.Length, Is.EqualTo(3));
			Assert.That(values[0], Is.EqualTo(3));
			Assert.That(values[1], Is.EqualTo(4));
			Assert.That(values[2], Is.EqualTo(5));
		});

		expectations.Verify();
	}

	[Test]
	public static void MakeWithReturnValues()
	{
		var mock = Rock.Make<IReturnSpan>().Instance();
#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
		Assert.Multiple(() =>
		{
			Assert.That(mock.GetRandomData() == default, Is.True);
			Assert.That(mock.Values == default, Is.True);
		});
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
	}

	[Test]
	public static void CreateWithSpanOfInt()
	{
		var expectations = Rock.Create<IHaveSpan>();
		expectations.Methods().Foo(new());

		var mock = expectations.Instance();
		var buffer = new int[] { 3 };

		mock.Foo(new Span<int>(buffer));

		expectations.Verify();
	}

	[Test]
	public static void MakeWithSpanOfInt()
	{
		var mock = Rock.Make<IHaveSpan>().Instance();
		var buffer = new int[] { 3 };
		mock.Foo(new Span<int>(buffer));
	}

	[Test]
	public static void CreateWithSpanOfIntAndValidation()
	{
		static bool FooEvaluation(Span<int> value) =>
			value.Length == 1 && value[0] == 3;

		var expectations = Rock.Create<IHaveSpan>();
		expectations.Methods().Foo(new(FooEvaluation));

		var mock = expectations.Instance();
		var buffer = new int[] { 3 };

		mock.Foo(new Span<int>(buffer));

		expectations.Verify();
	}

	[Test]
	public static void CreateWithSpanOfT()
	{
		var expectations = Rock.Create<IHaveSpan>();
		expectations.Methods().Bar<int>(new());

		var mock = expectations.Instance();
		var buffer = new int[] { 3 };

		mock.Bar(new Span<int>(buffer));

		expectations.Verify();
	}

	[Test]
	public static void MakeWithSpanOfT()
	{
		var mock = Rock.Make<IHaveSpan>().Instance();
		var buffer = new int[] { 3 };
		mock.Bar(new Span<int>(buffer));
	}

	[Test]
	public static void CreateWithSpanOfTAndValidation()
	{
		static bool BarEvaluation(Span<int> value) =>
			value.Length == 1 && value[0] == 3;

		var expectations = Rock.Create<IHaveSpan>();
		expectations.Methods().Bar<int>(new(BarEvaluation));

		var mock = expectations.Instance();
		var buffer = new int[] { 3 };

		mock.Bar(new Span<int>(buffer));

		expectations.Verify();
	}
}