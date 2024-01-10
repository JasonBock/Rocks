using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassIndexerGetterInit
{
	public virtual int this[int a] { get => default; init { } }
	public virtual int this[int a, string b] { get => default; init { } }
}

public class ClassIndexerGetterSetter
{
	public virtual int this[int a] { get => default; set { } }
	public virtual int this[int a, string b] { get => default; set { } }
}

public class ClassIndexerGetter
{
	public virtual int this[int a] => default;
	public virtual int this[int a, string b] => default;

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public class ClassIndexerInit
{
#pragma warning disable CA1044 // Properties should not be write only
   public virtual int this[int a] { init { } }
   public virtual int this[int a, string b] { init { } }
#pragma warning restore CA1044 // Properties should not be write only

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public class ClassIndexerSetter
{
#pragma warning disable CA1044 // Properties should not be write only
	public virtual int this[int a] { set { } }
	public virtual int this[int a, string b] { set { } }
#pragma warning restore CA1044 // Properties should not be write only

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassIndexerTests
{
	[Test]
	[RockCreate<ClassIndexerGetterInit>]
	public static void CreateWithOneParameterGetterAndInit()
	{
		var expectations = new ClassIndexerGetterInitCreateExpectations();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerGetterSetter>]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		var expectations = new ClassIndexerGetterSetterCreateExpectations();
		expectations.Indexers.Getters.This(3);
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetterInit>]
	public static void MakeWithOneParameterGetterAndInit()
	{
		var mock = new ClassIndexerGetterInitMakeExpectations().Instance(null);
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetterSetter>]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = new ClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock[3] = 4, Throws.Nothing);
		});
	}

	[Test]
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithOneParameterGetter()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetter>]
	public static void MakeWithOneParameterGetter()
	{
		var mock = new ClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).AddRaiseEvent(new(nameof(ClassIndexerGetter.MyEvent), EventArgs.Empty));

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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithOneParameterGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerGetterCreateExpectations();
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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithOneParameterGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).AddRaiseEvent(new(nameof(ClassIndexerGetter.MyEvent), EventArgs.Empty))
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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithOneParameterGetterMultipleCalls()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		var value = mock[3];
		value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerInit>]
	public static void CreateWithOneParameterInit()
	{
		var expectations = new ClassIndexerInitCreateExpectations();
		_ = expectations.Instance(null);
		expectations.Verify();
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithOneParameterSetter()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	[RockMake<ClassIndexerInit>]
	public static void MakeWithOneParameterInit() => 
		Assert.That(() => new ClassIndexerInitMakeExpectations().Instance(null), Throws.Nothing);

	[Test]
	[RockMake<ClassIndexerSetter>]
	public static void MakeWithOneParameterSetter()
	{
		var mock = new ClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).AddRaiseEvent(new(nameof(ClassIndexerSetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithOneParameterSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).Callback((a, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithOneParameterSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).AddRaiseEvent(new(nameof(ClassIndexerSetter.MyEvent), EventArgs.Empty))
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
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithOneParameterSetterMultipleCalls()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	[RockCreate<ClassIndexerGetterInit>]
	public static void CreateWithMultipleParametersGetterAndInit()
	{
		var expectations = new ClassIndexerGetterInitCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");

		var mock = expectations.Instance(null);
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerGetterSetter>]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = new ClassIndexerGetterSetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetterInit>]
	public static void MakeWithMultipleParametersGetterAndInit()
	{
		var mock = new ClassIndexerGetterInitMakeExpectations().Instance(null);
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetterSetter>]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = new ClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
		});
	}

	[Test]
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetter()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassIndexerGetter>]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = new ClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").AddRaiseEvent(new(nameof(ClassIndexerGetter.MyEvent), EventArgs.Empty));

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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerGetterCreateExpectations();
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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").AddRaiseEvent(new(nameof(ClassIndexerGetter.MyEvent), EventArgs.Empty))
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
	[RockCreate<ClassIndexerGetter>]
	public static void CreateWithMultipleParametersGetterMultipleCalls()
	{
		var expectations = new ClassIndexerGetterCreateExpectations();
		expectations.Indexers.Getters.This(3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetter()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();
	}

	[Test]
	[RockMake<ClassIndexerSetter>]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = new ClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").AddRaiseEvent(new(nameof(ClassIndexerSetter.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").Callback((a, b, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").AddRaiseEvent(new(nameof(ClassIndexerSetter.MyEvent), EventArgs.Empty))
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
	[RockCreate<ClassIndexerSetter>]
	public static void CreateWithMultipleParametersSetterMultipleCalls()
	{
		var expectations = new ClassIndexerSetterCreateExpectations();
		expectations.Indexers.Setters.This(4, 3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;

		expectations.Verify();
	}
}