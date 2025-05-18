using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.RockContextTestTypes;

public interface IContextural
{
	void DoStuff();
	void DoOtherStuff();
}

public static class RockContextTests
{
	[Test]
	public static void CauseExpectationAndVerificationExceptions() =>
		Assert.That(() =>
		{
			using var context = new RockContext();
			var expectations = context.Create<IContexturalCreateExpectations>();
			expectations.Methods.DoOtherStuff();

			var mock = expectations.Instance();
			mock.DoStuff();
		}, Throws.TypeOf<ExpectationException>());

	[Test]
	public static void CausesVerificationExceptionAndContextIsEnabled() =>
		Assert.That(() =>
		{
			using var context = new RockContext();
			var expectations = context.Create<IContexturalCreateExpectations>();
			expectations.Methods.DoStuff();
			expectations.Methods.DoOtherStuff();

			var mock = expectations.Instance();
			mock.DoStuff();
		}, Throws.TypeOf<VerificationException>());

	[Test]
	public static void CausesVerificationExceptionAndContextIsDisabled() =>
		Assert.That(() =>
		{
			using var context = new RockContext(DisableVerification.Yes);
			var expectations = context.Create<IContexturalCreateExpectations>();
			expectations.Methods.DoStuff();
			expectations.Methods.DoOtherStuff();

			var mock = expectations.Instance();
			mock.DoStuff();
		}, Throws.Nothing);
}