﻿using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.AbstractClassMethodReturnTestTypes;

public abstract class AbstractClassMethodReturn
{
	public abstract int NoParameters();
	public abstract int OneParameter(int a);
	public abstract int MultipleParameters(int a, string b);
}

public static class AbstractClassMethodReturnTests
{
	[Test]
	public static void CreateWithNoParameters()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithNoParameters()
	{
		var mock = new AbstractClassMethodReturnMakeExpectations().Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithNoParametersWithReturn()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.NoParameters().ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();
		mock.NoParameters();
	}

	[Test]
	public static void CreateWithNoParametersMultipleCallsNotMet()
	{
		var expectations = new AbstractClassMethodReturnCreateExpectations();
		expectations.Methods.NoParameters().ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock.NoParameters();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithNoParametersAndCallback()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.NoParameters().Callback(() => 3);

		var mock = expectations.Instance();
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithNoParametersNoExpectationSet()
	{
		var expectations = new AbstractClassMethodReturnCreateExpectations();

		var chunk = expectations.Instance();

		Assert.That(chunk.NoParameters, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithNoParametersExpectationsNotMet()
	{
		var expectations = new AbstractClassMethodReturnCreateExpectations();
		expectations.Methods.NoParameters();

		_ = expectations.Instance();

		Assert.That(expectations.Verify, Throws.TypeOf<VerificationException>());
	}

	[Test]
	public static void CreateWithOneParameter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameter()
	{
		var mock = new AbstractClassMethodReturnMakeExpectations().Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterWithReturn()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.OneParameter(3).ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterWithCallback()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.OneParameter(3).Callback(_ => 3);

		var mock = expectations.Instance();
		var value = mock.OneParameter(3);

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithOneParameterArgExpectationNotMet()
	{
		var expectations = new AbstractClassMethodReturnCreateExpectations();
		expectations.Methods.OneParameter(3);

		var mock = expectations.Instance();

		Assert.That(() => mock.OneParameter(1), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithMultipleParameters()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParameters()
	{
		var mock = new AbstractClassMethodReturnMakeExpectations().Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersWithReturn()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.MultipleParameters(3, "b").ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void CreateWithMultipleParametersWithCallback()
	{
		var aValue = 0;
		var bValue = string.Empty;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnCreateExpectations>();
		expectations.Methods.MultipleParameters(3, "b").Callback((a, b) =>
		{
			(aValue, bValue) = (a, b);
			return 3;
		});

		var mock = expectations.Instance();
		var value = mock.MultipleParameters(3, "b");

		Assert.Multiple(() =>
		{
			Assert.That(aValue, Is.EqualTo(3));
			Assert.That(bValue, Is.EqualTo("b"));
			Assert.That(value, Is.EqualTo(3));
		});
	}

	[Test]
	public static void CreateWithMultipleParametersArgExpectationNotMet()
	{
		var expectations = new AbstractClassMethodReturnCreateExpectations();
		expectations.Methods.MultipleParameters(3, "b");

		var mock = expectations.Instance();

		Assert.That(() => mock.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
	}
}