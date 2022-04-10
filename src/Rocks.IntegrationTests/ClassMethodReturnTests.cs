using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public class ClassMethodReturn
{
	public virtual int NoParameters() => default;
	public virtual int OneParameter(int a) => default;
	public virtual int MultipleParameters(int a, string b) => default;
}

public static class ClassMethodReturnTests
{
	[Test]
	public static void CreateWithNoParameters()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance();
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithNoParameters()
	{
		var chunk = Rock.Make<ClassMethodReturn>().Instance();
		var value = chunk.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithNoParametersWithReturn()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters().Returns(3);

		var chunk = rock.Instance();
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersMultipleCalls()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters().CallCount(2);

		var chunk = rock.Instance();
		chunk.NoParameters();
		chunk.NoParameters();

		rock.Verify();
	}

	[Test]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters().CallCount(2);

		var chunk = rock.Instance();
		chunk.NoParameters();

		Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithNoParametersAndCallback()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters().Callback(() => 3);

		var chunk = rock.Instance();
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var rock = Rock.Create<ClassMethodReturn>();

		var chunk = rock.Instance();

		Assert.That(() => chunk.NoParameters(), Throws.Nothing);
	}

	[Test]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance();

		Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithOneParameter()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().OneParameter(3);

		var chunk = rock.Instance();
		var value = chunk.OneParameter(3);

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameter()
	{
		var chunk = Rock.Make<ClassMethodReturn>().Instance();
		var value = chunk.OneParameter(3);

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterWithReturn()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().OneParameter(3).Returns(3);

		var chunk = rock.Instance();
		var value = chunk.OneParameter(3);

		rock.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterWithCallback()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().OneParameter(3).Callback(_ => 3);

		var chunk = rock.Instance();
		var value = chunk.OneParameter(3);

		rock.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().OneParameter(3);

		var chunk = rock.Instance();

		Assert.That(() => chunk.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMultipleParameters()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().MultipleParameters(3, "b");

		var chunk = rock.Instance();
		var value = chunk.MultipleParameters(3, "b");

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParameters()
	{
		var chunk = Rock.Make<ClassMethodReturn>().Instance();
		var value = chunk.MultipleParameters(3, "b");

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersWithReturn()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().MultipleParameters(3, "b").Returns(3);

		var chunk = rock.Instance();
		var value = chunk.MultipleParameters(3, "b");

		rock.Verify();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().MultipleParameters(3, "b").Callback((a, b) =>
		{
			(aValue, bValue) = (a, b);
			return 3;
		});

		var chunk = rock.Instance();
		var value = chunk.MultipleParameters(3, "b");

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(aValue, Is.EqualTo(3));
			Assert.That(bValue, Is.EqualTo("b"));
			Assert.That(value, Is.EqualTo(3));
		});
	}

	[Test]
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var rock = Rock.Create<ClassMethodReturn>();
		rock.Methods().MultipleParameters(3, "b");

		var chunk = rock.Instance();

		Assert.That(() => chunk.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}