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
		var rock = Rock.Create<IHaveInAndOutSpan>();
		rock.Methods().Foo(new()).Returns(() => new[] { 1, 2 }.AsSpan());
		rock.Properties().Getters().Values().Returns(() => new byte[] { 3, 4, 5 }.AsSpan());

		var chunk = rock.Instance();

		Assert.Multiple(() =>
		{
			var data = chunk.Foo(default);
			Assert.That(data.Length, Is.EqualTo(2));
			Assert.That(data[0], Is.EqualTo(1));
			Assert.That(data[1], Is.EqualTo(2));
			var values = chunk.Values;
			Assert.That(values.Length, Is.EqualTo(3));
			Assert.That(values[0], Is.EqualTo(3));
			Assert.That(values[1], Is.EqualTo(4));
			Assert.That(values[2], Is.EqualTo(5));
		});

		rock.Verify();
	}

	[Test]
	public static void MakeInAndOut()
	{

	}

	[Test]
	public static void CreateWithReturnValues()
	{
		var rock = Rock.Create<IReturnSpan>();
		rock.Methods().GetRandomData().Returns(() => new[] { 1, 2 }.AsSpan());
		rock.Properties().Getters().Values().Returns(() => new byte[] { 3, 4, 5 }.AsSpan());

		var chunk = rock.Instance();

		Assert.Multiple(() =>
		{
			var data = chunk.GetRandomData();
			Assert.That(data.Length, Is.EqualTo(2));
			Assert.That(data[0], Is.EqualTo(1));
			Assert.That(data[1], Is.EqualTo(2));
			var values = chunk.Values;
			Assert.That(values.Length, Is.EqualTo(3));
			Assert.That(values[0], Is.EqualTo(3));
			Assert.That(values[1], Is.EqualTo(4));
			Assert.That(values[2], Is.EqualTo(5));
		});

		rock.Verify();
	}

	[Test]
	public static void MakeWithReturnValues()
	{
		var chunk = Rock.Make<IReturnSpan>().Instance();
#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
		Assert.That(chunk.GetRandomData() == default(Span<int>), Is.True);
		Assert.That(chunk.Values == default(Span<byte>), Is.True);
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
	}

	[Test]
	public static void CreateWithSpanOfInt()
	{
		var rock = Rock.Create<IHaveSpan>();
		rock.Methods().Foo(new());

		var chunk = rock.Instance();
		var buffer = new int[] { 3 };

		chunk.Foo(new Span<int>(buffer));

		rock.Verify();
	}

	[Test]
	public static void MakeWithSpanOfInt()
	{
		var chunk = Rock.Make<IHaveSpan>().Instance();
		var buffer = new int[] { 3 };
		chunk.Foo(new Span<int>(buffer));
	}

	[Test]
	public static void CreateWithSpanOfIntAndValidation()
	{
		static bool FooEvaluation(Span<int> value) =>
			value.Length == 1 && value[0] == 3;

		var rock = Rock.Create<IHaveSpan>();
		rock.Methods().Foo(new(FooEvaluation));

		var chunk = rock.Instance();
		var buffer = new int[] { 3 };

		chunk.Foo(new Span<int>(buffer));

		rock.Verify();
	}

	[Test]
	public static void CreateWithSpanOfT()
	{
		var rock = Rock.Create<IHaveSpan>();
		rock.Methods().Bar<int>(new());

		var chunk = rock.Instance();
		var buffer = new int[] { 3 };

		chunk.Bar(new Span<int>(buffer));

		rock.Verify();
	}

	[Test]
	public static void MakeWithSpanOfT()
	{
		var chunk = Rock.Make<IHaveSpan>().Instance();
		var buffer = new int[] { 3 };
		chunk.Bar(new Span<int>(buffer));
	}

	[Test]
	public static void CreateWithSpanOfTAndValidation()
	{
		static bool BarEvaluation(Span<int> value) =>
			value.Length == 1 && value[0] == 3;

		var rock = Rock.Create<IHaveSpan>();
		rock.Methods().Bar<int>(new(BarEvaluation));

		var chunk = rock.Instance();
		var buffer = new int[] { 3 };

		chunk.Bar(new Span<int>(buffer));

		rock.Verify();
	}
}