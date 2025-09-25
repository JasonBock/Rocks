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
	public static void CausesVerificationException() =>
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
	public static void CallbackCausesException() =>
		Assert.That(() =>
		{
			using var context = new RockContext();
			var expectations = context.Create<IContexturalCreateExpectations>();
			expectations.Methods.DoStuff()
				.Callback(() => throw new NotImplementedException());

			var mock = expectations.Instance();
			mock.DoStuff();
		}, Throws.TypeOf<NotImplementedException>());

	[Test]
	public static void CallbackCausesExceptionAndVerificationFails() =>
		Assert.That(() =>
		{
			using var context = new RockContext();
			var expectations = context.Create<IContexturalCreateExpectations>();
			expectations.Methods.DoStuff()
				.Callback(() => throw new NotImplementedException());
			expectations.Methods.DoOtherStuff();

			var mock = expectations.Instance();
			mock.DoStuff();
		}, Throws.TypeOf<VerificationException>());
}