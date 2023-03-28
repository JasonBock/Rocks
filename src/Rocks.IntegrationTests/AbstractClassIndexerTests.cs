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
	public static void CreateWithOneParameterGetterAndSetter()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetterSetter>();
		expectations.Indexers().Getters().This(3);
		expectations.Indexers().Setters().This(3, 4);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterAndInit()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetterInit>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = Rock.Make<AbstractClassIndexerGetterSetter>().Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetter()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = Rock.Make<AbstractClassIndexerGetter>().Instance();
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
		expectations.Indexers().Getters().This(3).CallCount(2);

		var mock = expectations.Instance();
		var value = mock[3];
		value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = Rock.Make<AbstractClassIndexerSetter>().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetterSetter>();
		expectations.Indexers().Getters().This(3, "b");
		expectations.Indexers().Setters().This(3, "b", 4);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = Rock.Make<AbstractClassIndexerGetterSetter>().Instance();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = Rock.Make<AbstractClassIndexerGetter>().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerGetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = Rock.Make<AbstractClassIndexerSetter>().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
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
		var expectations = Rock.Create<AbstractClassIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;

		expectations.Verify();
	}
}