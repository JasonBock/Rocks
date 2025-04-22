using NUnit.Framework;
using Rocks.Runtime.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.IntegrationTests.DoesNotReturnTestTypes;

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
		var expectations = new UsesDoesNotReturnCreateExpectations();
		expectations.Methods.VoidMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<NotImplementedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveHandler()
	{
		var expectations = new UsesDoesNotReturnCreateExpectations();
		expectations.Methods.VoidMethod();

		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<DoesNotReturnException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveExpectation()
	{
		var expectations = new UsesDoesNotReturnCreateExpectations();
		var mock = expectations.Instance();

		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatHasHandler()
	{
		var expectations = new UsesDoesNotReturnCreateExpectations();
		expectations.Methods.IntMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<NotImplementedException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveHandler()
	{
		var expectations = new UsesDoesNotReturnCreateExpectations();
		expectations.Methods.IntMethod().ReturnValue(1);

		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<DoesNotReturnException>());

		expectations.Verify();
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveExpectation()
	{
		var expectations = new UsesDoesNotReturnCreateExpectations();
		var mock = expectations.Instance();

		Assert.That(() => mock.IntMethod(), Throws.TypeOf<NotSupportedException>());

		expectations.Verify();
	}

	[Test]
	public static void MakeWithVoidCall()
	{
		var mock = new UsesDoesNotReturnMakeExpectations().Instance();
		Assert.That(() => mock.VoidMethod(), Throws.TypeOf<DoesNotReturnException>());
	}

	[Test]
	public static void MakeWithIntCall()
	{
		var mock = new UsesDoesNotReturnMakeExpectations().Instance();
		Assert.That(() => mock.IntMethod(), Throws.TypeOf<DoesNotReturnException>());
	}
}