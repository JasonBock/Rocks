﻿using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassIndexerTestTypes;

public abstract class AbstractClassIndexerGetterSetter
{
	public abstract int this[int a] { get; set; }
	public abstract int this[int a, string b] { get; set; }
}

public abstract class AbstractClassIndexerGetterInit
{
	public abstract int this[int a] { get; init; }
	public abstract int this[int a, string b] { get; init; }
}

public abstract class AbstractClassIndexerGetter
{
	public abstract int this[int a] { get; }
	public abstract int this[int a, string b] { get; }

	public abstract event EventHandler MyEvent;
}

public abstract class AbstractClassIndexerSetter
{
#pragma warning disable CA1044 // Properties should not be write only
   public abstract int this[int a] { set; }
   public abstract int this[int a, string b] { set; }
#pragma warning restore CA1044 // Properties should not be write only

	public abstract event EventHandler MyEvent;
}

public static class AbstractClassIndexerTests
{
	[Test]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterSetterCreateExpectations>();
		expectations.Indexers.Getters.This(3);
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterAndInit()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterInitCreateExpectations>();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = new AbstractClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = new AbstractClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3).RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.Default);
			Assert.That(wasEventRaised, Is.True);
		});
	}

	[Test]
	public static void CreateWithOneParameterGetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3).Callback(_ =>
		{
			wasCallbackInvoked = true;
			return _;
		});

		var mock = expectations.Instance();
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3).RaiseMyEvent(EventArgs.Empty)
			.Callback(_ =>
			{
				wasCallbackInvoked = true;
				return _;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithOneParameterGetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3];
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		mock[3] = 4;
	}

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = new AbstractClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3).RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateWithOneParameterSetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3).Callback((a, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3] = 4;

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3).RaiseMyEvent(EventArgs.Empty)
			.Callback((a, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithOneParameterSetterMultipleCalls()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = new AbstractClassIndexerGetterSetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = new AbstractClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.Default);
			Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersGetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = new AbstractClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b").RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.Default);
			Assert.That(wasEventRaised, Is.True);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersGetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b").Callback((a, b) =>
		{
			wasCallbackInvoked = true;
			return a;
		});

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b").RaiseMyEvent(EventArgs.Empty)
			.Callback((a, b) =>
			{
				wasCallbackInvoked = true;
				return a;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersGetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3, "b"];
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = new AbstractClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b").RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b").Callback((a, b, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b").RaiseMyEvent(EventArgs.Empty)
			.Callback((a, b, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersSetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;
	}
}