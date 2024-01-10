using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public interface IInterfaceMethodVoid
{
	void NoParameters();
	void OneParameter(int a);
	void MultipleParameters(int a, string b);
}

public static class InterfaceMethodVoidTests
{
	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParameters()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance();
		mock.NoParameters();

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceMethodVoid>]
	public static void MakeWithNoParameters()
	{
		var mock = new IInterfaceMethodVoidMakeExpectations().Instance();

		Assert.That(mock.NoParameters, Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParametersMultipleCalls()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();
		mock.NoParameters();

		expectations.Verify();
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParametersAndCallback()
	{
		var wasCallbackInvoked = false;

		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.NoParameters().Callback(() => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(wasCallbackInvoked, Is.True);
		expectations.Verify();
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();

		var mock = expectations.Instance();

		Assert.That(mock.NoParameters, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithOneParameter()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();
		mock.OneParameter(3);

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceMethodVoid>]
	public static void MakeWithOneParameter()
	{
		var mock = new IInterfaceMethodVoidMakeExpectations().Instance();

		Assert.That(() => mock.OneParameter(3), Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithOneParameterWithCallback()
	{
		var aValue = 0;
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.OneParameter(3).Callback(a => aValue = a);

		var mock = expectations.Instance();
		mock.OneParameter(3);

		expectations.Verify();

		Assert.That(aValue, Is.EqualTo(3));
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();

		Assert.That(() => mock.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithMultipleParameters()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();
		mock.MultipleParameters(3, "b");

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceMethodVoid>]
	public static void MakeWithMultipleParameters()
	{
		var mock = new IInterfaceMethodVoidMakeExpectations().Instance();

		Assert.That(() => mock.MultipleParameters(3, "b"), Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b").Callback((a, b) => (aValue, bValue) = (a, b));

		var mock = expectations.Instance();
		mock.MultipleParameters(3, "b");

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(aValue, Is.EqualTo(3));
			Assert.That(bValue, Is.EqualTo("b"));
		});
	}

	[Test]
	[RockCreate<IInterfaceMethodVoid>]
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var expectations = new IInterfaceMethodVoidCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();

		Assert.That(() => mock.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}