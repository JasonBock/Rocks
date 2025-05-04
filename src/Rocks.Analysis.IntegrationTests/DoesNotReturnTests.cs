using NUnit.Framework;
using Rocks.Exceptions;
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
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		expectations.Methods.VoidMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(mock.VoidMethod, Throws.TypeOf<NotImplementedException>());
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveHandler()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		expectations.Methods.VoidMethod();

		var mock = expectations.Instance();

		Assert.That(mock.VoidMethod, Throws.TypeOf<DoesNotReturnException>());
	}

	[Test]
	public static void CreateWithVoidCallThatDoesNotHaveExpectation()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		var mock = expectations.Instance();

		Assert.That(mock.VoidMethod, Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void CreateWithIntCallThatHasHandler()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		expectations.Methods.IntMethod().Callback(() => throw new NotImplementedException());

		var mock = expectations.Instance();

		Assert.That(mock.IntMethod, Throws.TypeOf<NotImplementedException>());
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveHandler()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		expectations.Methods.IntMethod().ReturnValue(1);

		var mock = expectations.Instance();

		Assert.That(mock.IntMethod, Throws.TypeOf<DoesNotReturnException>());
	}

	[Test]
	public static void CreateWithIntCallThatDoesNotHaveExpectation()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<UsesDoesNotReturnCreateExpectations>();
		var mock = expectations.Instance();

		Assert.That(mock.IntMethod, Throws.TypeOf<NotSupportedException>());
	}

	[Test]
	public static void MakeWithVoidCall()
	{
		var mock = new UsesDoesNotReturnMakeExpectations().Instance();
		Assert.That(mock.VoidMethod, Throws.TypeOf<DoesNotReturnException>());
	}

	[Test]
	public static void MakeWithIntCall()
	{
		var mock = new UsesDoesNotReturnMakeExpectations().Instance();
		Assert.That(mock.IntMethod, Throws.TypeOf<DoesNotReturnException>());
	}
}