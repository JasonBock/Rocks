using NUnit.Framework;
using Rocks.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.IntegrationTests;

public class UsesDoesNotReturn
{
	[DoesNotReturn]
	public virtual void VoidMethod() => throw new NotSupportedException();

	[DoesNotReturn]
	public virtual int IntMethod() => throw new NotSupportedException();
}

public static class DoesNotReturnTests
{
	[Test]
	public static void CreateWithVoidCallThatHasHandler()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		expectations.Methods().VoidMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<NotImplementedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveHandler()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		expectations.Methods().VoidMethod();

		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<DoesNotReturnException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveExpectation()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatHasHandler()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		expectations.Methods().IntMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<NotImplementedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveHandler()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		expectations.Methods().IntMethod().Returns(1);

		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<DoesNotReturnException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveExpectation()
	{
		var expectations = Rock.Create<UsesDoesNotReturn>();
		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	public static void MakeWithVoidCall()
	{
		var mock = Rock.Make<UsesDoesNotReturn>().Instance();
		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<DoesNotReturnException>());
	}

	[Test]
	public static void MakeWithIntCall()
	{
		var mock = Rock.Make<UsesDoesNotReturn>().Instance();
		Assert.That(() => mock.IntMethod(), Throws.TypeOf<DoesNotReturnException>());
	}
}