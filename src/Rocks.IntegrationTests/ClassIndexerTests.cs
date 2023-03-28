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
	public static void CreateWithOneParameterGetterAndInit()
	{
		var expectations = Rock.Create<ClassIndexerGetterInit>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		var expectations = Rock.Create<ClassIndexerGetterSetter>();
		expectations.Indexers().Getters().This(3);
		expectations.Indexers().Setters().This(3, 4);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetterAndInit()
	{
		var mock = Rock.Make<ClassIndexerGetterInit>().Instance(null);
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = Rock.Make<ClassIndexerGetterSetter>().Instance();
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock[3] = 4, Throws.Nothing);
		});
	}

	[Test]
	public static void CreateWithOneParameterGetter()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = Rock.Make<ClassIndexerGetter>().Instance();
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3).RaisesMyEvent(EventArgs.Empty);

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
	public static void CreateWithOneParameterGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3).Callback(_ =>
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
	public static void CreateWithOneParameterGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3).RaisesMyEvent(EventArgs.Empty)
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
	public static void CreateWithOneParameterGetterMultipleCalls()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3).CallCount(2);

		var mock = expectations.Instance();
		var value = mock[3];
		value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterInit()
	{
		var expectations = Rock.Create<ClassIndexerInit>();
		_ = expectations.Instance(null);
		expectations.Verify();
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithOneParameterInit() => 
		Assert.That(() => Rock.Make<ClassIndexerInit>().Instance(null), Throws.Nothing);

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = Rock.Make<ClassIndexerSetter>().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateWithOneParameterSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).Callback((a, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).RaisesMyEvent(EventArgs.Empty)
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
	public static void CreateWithOneParameterSetterMultipleCalls()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndInit()
	{
		var expectations = Rock.Create<ClassIndexerGetterInit>();
		expectations.Indexers().Getters().This(3, "b");

		var mock = expectations.Instance(null);
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = Rock.Create<ClassIndexerGetterSetter>();
		expectations.Indexers().Getters().This(3, "b");
		expectations.Indexers().Setters().This(3, "b", 4);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndInit()
	{
		var mock = Rock.Make<ClassIndexerGetterInit>().Instance(null);
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = Rock.Make<ClassIndexerGetterSetter>().Instance();
		var value = mock[3, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
		});
	}

	[Test]
	public static void CreateWithMultipleParametersGetter()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = Rock.Make<ClassIndexerGetter>().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b").RaisesMyEvent(EventArgs.Empty);

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
	public static void CreateWithMultipleParametersGetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b").Callback((a, b) =>
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
	public static void CreateWithMultipleParametersGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b").RaisesMyEvent(EventArgs.Empty)
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
	public static void CreateWithMultipleParametersGetterMultipleCalls()
	{
		var expectations = Rock.Create<ClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b").CallCount(2);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersSetter()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = Rock.Make<ClassIndexerSetter>().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).Callback((a, b, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).RaisesMyEvent(EventArgs.Empty)
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
	public static void CreateWithMultipleParametersSetterMultipleCalls()
	{
		var expectations = Rock.Create<ClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;

		expectations.Verify();
	}
}