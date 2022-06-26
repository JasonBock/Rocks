using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassConstructorWithSpecialParameters
{
	public ClassConstructorWithSpecialParameters(int a, ref string b, out string c, params string[] d)
	{
		c = "42";
		(this.A, this.B, this.C, this.D) = (a, b, c, d);
	}

	public virtual void Foo() { }

	public int A { get; }

	public string B { get; }

	public string C { get; }

#pragma warning disable CA1819 // Properties should not return arrays
	public string[] D { get; }
#pragma warning restore CA1819 // Properties should not return arrays
}

public class ClassConstructor
{
	protected ClassConstructor(string stringData) =>
		this.StringData = stringData;
	public ClassConstructor(int intData) =>
		this.IntData = intData;

	public virtual int NoParameters() => default;

	public int IntData { get; }
	public string? StringData { get; }
}

public static class ClassConstructorTests
{
	[Test]
	public static void CreateSpecialConstructor()
	{
		var bValue = "b";
		var cValue = "c";
		var d1Value = "d1";
		var d2Value = "d2";

		var rock = Rock.Create<ClassConstructorWithSpecialParameters>();
		rock.Methods().Foo();

		var chunk = rock.Instance(2, ref bValue, out cValue, d1Value, d2Value);
		chunk.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(chunk.A, Is.EqualTo(2));
			Assert.That(chunk.B, Is.EqualTo("b"));
			Assert.That(chunk.C, Is.EqualTo("42"));
			Assert.That(cValue, Is.EqualTo("42"));
			Assert.That(chunk.D, Is.EquivalentTo(new [] { d1Value, d2Value }));
		});

		rock.Verify();
	}

	[Test]
	public static void MakeSpecialConstructor()
	{
		var bValue = "b";
		var cValue = "c";
		var d1Value = "d1";
		var d2Value = "d2";

		var chunk = Rock.Make<ClassConstructorWithSpecialParameters>().Instance(2, ref bValue, out cValue, d1Value, d2Value);

		Assert.Multiple(() =>
		{
			Assert.That(chunk.A, Is.EqualTo(2));
			Assert.That(chunk.B, Is.EqualTo("b"));
			Assert.That(chunk.C, Is.EqualTo("42"));
			Assert.That(cValue, Is.EqualTo("42"));
			Assert.That(chunk.D, Is.EquivalentTo(new[] { d1Value, d2Value }));
		});
	}

	[Test]
	public static void CreateWithNoParametersAndPublicConstructor()
	{
		var rock = Rock.Create<ClassConstructor>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance(3);
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(chunk.IntData, Is.EqualTo(3));
			Assert.That(chunk.StringData, Is.Null);
		});
	}

	[Test]
	public static void MakeWithNoParametersAndPublicConstructor()
	{
		var chunk = Rock.Make<ClassConstructor>().Instance(3);
		var value = chunk.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithNoParametersAndProtectedConstructor()
	{
		var rock = Rock.Create<ClassConstructor>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance("b");
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(chunk.IntData, Is.EqualTo(default(int)));
			Assert.That(chunk.StringData, Is.EqualTo("b"));
		});
	}

	[Test]
	public static void MakeWithNoParametersAndProtectedConstructor()
	{
		var chunk = Rock.Make<ClassConstructor>().Instance("b");
		var value = chunk.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}
}