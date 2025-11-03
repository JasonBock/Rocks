using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.ClassMethodVoidTestTypes;

public class MethodVoidTests
{
	public virtual void NoParameters() { }
	public virtual void OneParameter(int a) { }
	public virtual void MultipleParameters(int a, string b) { }
}

internal static class ClassMethodVoidTests
{
	[Test]
	public static void CreateWithNoParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.NoParameters();

		var mock = expectations.Instance();
		mock.NoParameters();
	}

	[Test]
	public static void MakeWithNoParameters()
	{
		var mock = new MethodVoidTestsMakeExpectations().Instance();

		Assert.That(mock.NoParameters, Throws.Nothing);
	}

	[Test]
	public static void CreateWithNoParametersMultipleCalls()
	{
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();
		mock.NoParameters();
	}

	[Test]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var expectations = new MethodVoidTestsCreateExpectations();
		expectations.Setups.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithNoParametersAndCallback()
	{
		var wasCallbackInvoked = false;

		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.NoParameters().Callback(() => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var expectations = new MethodVoidTestsCreateExpectations();

		var mock = expectations.Instance();

		Assert.That(mock.NoParameters, Throws.Nothing);
	}

	[Test]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var expectations = new MethodVoidTestsCreateExpectations();
		expectations.Setups.NoParameters();

		_ = expectations.Instance();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithOneParameter()
	{
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.OneParameter(3);

		var mock = expectations.Instance();
		mock.OneParameter(3);
	}

	[Test]
	public static void MakeWithOneParameter()
	{
		var mock = new MethodVoidTestsMakeExpectations().Instance();

		Assert.That(() => mock.OneParameter(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterWithCallback()
	{
		var aValue = 0;
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.OneParameter(3).Callback(a => aValue = a);

		var mock = expectations.Instance();
		mock.OneParameter(3);

		Assert.That(aValue, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var expectations = new MethodVoidTestsCreateExpectations();
		expectations.Setups.OneParameter(3);

		var mock = expectations.Instance();

		Assert.That(() => mock.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMultipleParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.MultipleParameters(3, "b");

		var mock = expectations.Instance();
		mock.MultipleParameters(3, "b");
	}

	[Test]
	public static void MakeWithMultipleParameters()
	{
		var mock = new MethodVoidTestsMakeExpectations().Instance();

		Assert.That(() => mock.MultipleParameters(3, "b"), Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		using var context = new RockContext();
		var expectations = context.Create<MethodVoidTestsCreateExpectations>();
		expectations.Setups.MultipleParameters(3, "b").Callback((a, b) => (aValue, bValue) = (a, b));

		var mock = expectations.Instance();
		mock.MultipleParameters(3, "b");

		using (Assert.EnterMultipleScope())
		{
			Assert.That(aValue, Is.EqualTo(3));
			Assert.That(bValue, Is.EqualTo("b"));
		}
	}

	[Test]
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var expectations = new MethodVoidTestsCreateExpectations();
		expectations.Setups.MultipleParameters(3, "b");

		var mock = expectations.Instance();

		Assert.That(() => mock.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}