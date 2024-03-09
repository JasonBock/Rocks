using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public interface IData
{
	int CalculateValue();
	void Calculate();
	string Value { get; set; }
}

public static class VerificationTests
{
	[Test]
	[RockCreate<IData>]
	public static void VerifyWhenVoidCallbackThrowsException()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Methods.Calculate().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.Calculate(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	[RockCreate<IData>]
	public static void VerifyWhenIntCallbackThrowsException()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Methods.CalculateValue().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.CalculateValue(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	[RockCreate<IData>]
	public static void VerifyWhenGetterCallbackThrowsException()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Properties.Getters.Value().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.Value, Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	[RockCreate<IData>]
	public static void VerifyWhenSetterCallbackThrowsException()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Properties.Setters.Value("x").Callback(_ => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.Value = "x", Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	[RockCreate<IData>]
	public static void VerifyWhenPropertyExpectationIsNotMet()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Properties.Setters.Value("3");

		_ = expectations.Instance();

		Assert.That(expectations.Verify, 
			Throws.TypeOf<VerificationException>()
				.And.Message.EqualTo(
					"The following verification failure(s) occured: Mock type: Rocks.IntegrationTests.IDataCreateExpectations+Mock, member: Void set_Value(System.String), messsage: The expected call count is incorrect. Expected: 1, received: 0."));
	}
}