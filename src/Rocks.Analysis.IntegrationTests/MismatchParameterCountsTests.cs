using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.MismatchParameterCountsTestTypes;

public interface IMismatch
{
	void NoParameters<T>();
	void ExtraTypeParameter<T, TValue>(TValue value);
}

internal static class MismatchParameterCountsTests
{
	[Test]
	public static void CreateWithMultipleNoParametersSetups()
	{
		using var context = new RockContext();
		var expectations = context.Create<IMismatchCreateExpectations>();
		expectations.Setups.NoParameters<string>();
		expectations.Setups.NoParameters<int>();

		var mock = expectations.Instance();
		mock.NoParameters<string>();
		mock.NoParameters<int>();
	}

	[Test]
	public static void CreateWithUnexpectedNoParametersSetups()
	{
		using var context = new RockContext();
		var expectations = context.Create<IMismatchCreateExpectations>();
		expectations.Setups.NoParameters<string>();

		var mock = expectations.Instance();
		Assert.That(() => mock.NoParameters<Guid>(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMissingNoParametersSetups() => 
		Assert.That(
			() =>
			{
				using var context = new RockContext();
				var expectations = context.Create<IMismatchCreateExpectations>();
				expectations.Setups.NoParameters<string>();
				expectations.Setups.NoParameters<int>();

				var mock = expectations.Instance();
				mock.NoParameters<string>();
			}, 
			Throws.TypeOf<VerificationException>());

	[Test]
	public static void CreateWithExtraTypeParameterSetups()
	{
		using var context = new RockContext();
		var expectations = context.Create<IMismatchCreateExpectations>();
		expectations.Setups.ExtraTypeParameter<string, int>(3);
		expectations.Setups.ExtraTypeParameter<Guid, int>(3);

		var mock = expectations.Instance();
		mock.ExtraTypeParameter<string, int>(3);
		mock.ExtraTypeParameter<Guid, int>(3);
	}

	[Test]
	public static void CreateWithUnexpectedExtraTypeParameterSetups()
	{
		using var context = new RockContext();
		var expectations = context.Create<IMismatchCreateExpectations>();
		expectations.Setups.ExtraTypeParameter<string, int>(3);

		var mock = expectations.Instance();
		Assert.That(() => mock.ExtraTypeParameter<Guid, int>(3), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMissingExtraTypeParameterSetups() =>
		Assert.That(
			() =>
			{
				using var context = new RockContext();
				var expectations = context.Create<IMismatchCreateExpectations>();
				expectations.Setups.ExtraTypeParameter<string, int>(3);
				expectations.Setups.ExtraTypeParameter<Guid, int>(3);

				var mock = expectations.Instance();
				mock.ExtraTypeParameter<Guid, int>(3);
			},
			Throws.TypeOf<VerificationException>());
}