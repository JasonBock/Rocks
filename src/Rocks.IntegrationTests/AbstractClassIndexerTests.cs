using NUnit.Framework;

namespace Rocks.IntegrationTests;

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
	[RockCreate<AbstractClassIndexerGetterSetter>]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		var expectations = new AbstractClassIndexerGetterSetterCreateExpectations();
		expectations.Indexers.Getters.This(3);
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetterInit>]
	public static void CreateWithOneParameterGetterAndInit()
	{
		var expectations = new AbstractClassIndexerGetterInitCreateExpectations();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassIndexerGetterSetter>]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = new AbstractClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithOneParameterGetter()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassIndexerGetter>]
	public static void MakeWithOneParameterGetter()
	{
		var mock = new AbstractClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).AddRaiseEvent(new(nameof(AbstractClassIndexerGetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(wasEventRaised, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithOneParameterGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).Callback(_ =>
		{
			wasCallbackInvoked = true;
			return _;
		});

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithOneParameterGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).AddRaiseEvent(new(nameof(AbstractClassIndexerGetter.MyEvent), EventArgs.Empty))
			.Callback(_ =>
			{
				wasCallbackInvoked = true;
				return _;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithOneParameterGetterMultipleCalls()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3];
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithOneParameterSetter()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	[RockMake<AbstractClassIndexerSetter>]
	public static void MakeWithOneParameterSetter()
	{
		var mock = new AbstractClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).AddRaiseEvent(new(nameof(AbstractClassIndexerSetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithOneParameterSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).Callback((a, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithOneParameterSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).AddRaiseEvent(new(nameof(AbstractClassIndexerSetter.MyEvent), EventArgs.Empty))
			.Callback((a, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
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
	[RockCreate<AbstractClassIndexerGetterSetter>]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = new AbstractClassIndexerGetterSetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassIndexerGetterSetter>]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = new AbstractClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetter()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassIndexerGetter>]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = new AbstractClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").AddRaiseEvent(new(nameof(AbstractClassIndexerGetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(wasEventRaised, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").Callback((a, b) =>
		{
			wasCallbackInvoked = true;
			return a;
		});

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").AddRaiseEvent(new(nameof(AbstractClassIndexerGetter.MyEvent), EventArgs.Empty))
			.Callback((a, b) =>
			{
				wasCallbackInvoked = true;
				return a;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterMultipleCalls()
	{
		var expectations = new AbstractClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetter()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();
	}

	[Test]
	[RockMake<AbstractClassIndexerSetter>()]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = new AbstractClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").AddRaiseEvent(new(nameof(AbstractClassIndexerSetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").Callback((a, b, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").AddRaiseEvent(new(nameof(AbstractClassIndexerSetter.MyEvent), EventArgs.Empty))
			.Callback((a, b, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterMultipleCalls()
	{
		var expectations = new AbstractClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;

		expectations.Verify();
	}
}