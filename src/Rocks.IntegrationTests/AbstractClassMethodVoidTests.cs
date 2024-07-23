﻿using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests.AbstractClassMethodVoidTestTypes;

public abstract class AbstractMethodVoidTests
{
	public abstract void NoParameters();
	public abstract void OneParameter(int a);
	public abstract void MultipleParameters(int a, string b);
}

public static class AbstractClassMethodVoidTests
{
	[Test]
	public static void CreateWithNoParameters()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance();
		mock.NoParameters();

		expectations.Verify();
	}

	[Test]
	public static void MakeWithNoParameters()
	{
		var mock = new AbstractMethodVoidTestsMakeExpectations().Instance();

		Assert.That(mock.NoParameters, Throws.Nothing);
	}

	[Test]
	public static void CreateWithNoParametersMultipleCalls()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();
		mock.NoParameters();

		expectations.Verify();
	}

	[Test]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithNoParametersAndCallback()
	{
		var wasCallbackInvoked = false;

		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.NoParameters().Callback(() => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(wasCallbackInvoked, Is.True);
		expectations.Verify();
	}

	[Test]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();

		var mock = expectations.Instance();

		Assert.That(mock.NoParameters, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.NoParameters();

		_ = expectations.Instance();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithOneParameter()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();
		mock.OneParameter(3);

		expectations.Verify();
	}

	[Test]
	public static void MakeWithOneParameter()
	{
		var mock = new AbstractMethodVoidTestsMakeExpectations().Instance();

		Assert.That(() => mock.OneParameter(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterWithCallback()
	{
		var aValue = 0;
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.OneParameter(3).Callback(a => aValue = a);

		var mock = expectations.Instance();
		mock.OneParameter(3);

		expectations.Verify();

		Assert.That(aValue, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();

		Assert.That(() => mock.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMultipleParameters()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();
		mock.MultipleParameters(3, "b");

		expectations.Verify();
	}

	[Test]
	public static void MakeWithMultipleParameters()
	{
		var mock = new AbstractMethodVoidTestsMakeExpectations().Instance();

		Assert.That(() => mock.MultipleParameters(3, "b"), Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
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
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var expectations = new AbstractMethodVoidTestsCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();

		Assert.That(() => mock.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}