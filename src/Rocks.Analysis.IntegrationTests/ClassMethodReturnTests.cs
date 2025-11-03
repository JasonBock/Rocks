using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.ClassMethodReturnTestTypes;

public class ClassMethodReturn
{
	public virtual int NoParameters() => default;
	public virtual int OneParameter(int a) => default;
	public virtual int MultipleParameters(int a, string b) => default;
}

internal static class ClassMethodReturnTests
{
	[Test]
	public static void CreateWithNoParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.NoParameters();

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithNoParameters()
	{
		var mock = new ClassMethodReturnMakeExpectations().Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithNoParametersWithReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.NoParameters().ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersMultipleCalls()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();
		mock.NoParameters();
	}

	[Test]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var expectations = new ClassMethodReturnCreateExpectations();
		expectations.Setups.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithNoParametersAndCallback()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.NoParameters().Callback(() => 3);

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var expectations = new ClassMethodReturnCreateExpectations();

		var mock = expectations.Instance();

		Assert.That(mock.NoParameters, Throws.Nothing);
	}

	[Test]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var expectations = new ClassMethodReturnCreateExpectations();
		expectations.Setups.NoParameters();

		_ = expectations.Instance();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithOneParameter()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.OneParameter(3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameter()
	{
		var mock = new ClassMethodReturnMakeExpectations().Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterWithReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.OneParameter(3).ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterWithCallback()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.OneParameter(3).Callback(_ => 3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var expectations = new ClassMethodReturnCreateExpectations();
		expectations.Setups.OneParameter(3);

		var mock = expectations.Instance();

		Assert.That(() => mock.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMultipleParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.MultipleParameters(3, "b");

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParameters()
	{
		var mock = new ClassMethodReturnMakeExpectations().Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersWithReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.MultipleParameters(3, "b").ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnCreateExpectations>();
		expectations.Setups.MultipleParameters(3, "b").Callback((a, b) =>
		{
			(aValue, bValue) = (a, b);
			return 3;
		});

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		using (Assert.EnterMultipleScope())
		{
			Assert.That(aValue, Is.EqualTo(3));
			Assert.That(bValue, Is.EqualTo("b"));
			Assert.That(value, Is.EqualTo(3));
		}
	}

	[Test]
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var expectations = new ClassMethodReturnCreateExpectations();
		expectations.Setups.MultipleParameters(3, "b");

		var mock = expectations.Instance();

		Assert.That(() => mock.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}