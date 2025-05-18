using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.VerificationTestTypes;

public interface IData
{
	int CalculateValue();
	void Calculate();
	string Value { get; set; }
}

public static class VerificationTests
{
	[Test]
	public static void VerifyWhenVoidCallbackThrowsException()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDataCreateExpectations>();
		expectations.Methods.Calculate().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(data.Calculate, Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void VerifyWhenIntCallbackThrowsException()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDataCreateExpectations>();
		expectations.Methods.CalculateValue().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(data.CalculateValue, Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void VerifyWhenGetterCallbackThrowsException()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDataCreateExpectations>();
		expectations.Properties.Getters.Value().Callback(() => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.Value, Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void VerifyWhenSetterCallbackThrowsException()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDataCreateExpectations>();
		expectations.Properties.Setters.Value("x").Callback(_ => throw new NotSupportedException());

		var data = expectations.Instance();
		Assert.That(() => data.Value = "x", Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void VerifyWhenPropertyExpectationIsNotMet()
	{
		var expectations = new IDataCreateExpectations();
		expectations.Properties.Setters.Value("3");

		_ = expectations.Instance();

		Assert.That(expectations.Verify, 
			Throws.TypeOf<VerificationException>()
				.And.Message.EqualTo(
					"The following verification failure(s) occurred: Mock type: Rocks.Analysis.IntegrationTests.VerificationTestTypes.IDataCreateExpectations+Mock, member: Void set_Value(System.String), message: The expected call count is incorrect. Expected: 1, received: 0."));
	}
}