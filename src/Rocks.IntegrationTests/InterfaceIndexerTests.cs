using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IInterfaceIndexerGetterInit
{
	int this[int a] { get; init; }
	int this[int a, string b] { get; init; }
}

public interface IInterfaceIndexerGetterSetter
{
	int this[int a] { get; set; }
	int this[int a, string b] { get; set; }
}

public interface IInterfaceIndexerGetter
{
	int this[int a] { get; }
	int this[int a, string b] { get; }

	event EventHandler MyEvent;
}

public interface IInterfaceIndexerInit
{
   int this[int a] { init; }
	int this[int a, string b] { init; }

   event EventHandler MyEvent;
}

public interface IInterfaceIndexerSetter
{
   int this[int a] { set; }
	int this[int a, string b] { set; }

   event EventHandler MyEvent;
}

public static class InterfaceIndexerTests
{
	[Test]
	public static void CreateWithOneParameterGetterAndInit()
	{
		var expectations = Rock.Create<IInterfaceIndexerGetterInit>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		var expectations = Rock.Create<IInterfaceIndexerGetterSetter>();
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
		var mock = Rock.Make<IInterfaceIndexerGetterInit>().Instance(null);
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = Rock.Make<IInterfaceIndexerGetterSetter>().Instance();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
		expectations.Indexers().Getters().This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = Rock.Make<IInterfaceIndexerGetter>().Instance();
		var value = mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerInit>();
		Assert.That(() => expectations.Instance(null), Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4);

		var mock = expectations.Instance();
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = Rock.Make<IInterfaceIndexerSetter>().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
		expectations.Indexers().Setters().This(3, 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;

		expectations.Verify();
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		var expectations = Rock.Create<IInterfaceIndexerGetterSetter>();
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
		var mock = Rock.Make<IInterfaceIndexerGetterSetter>().Instance();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
		expectations.Indexers().Getters().This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = Rock.Make<IInterfaceIndexerGetter>().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerGetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = Rock.Make<IInterfaceIndexerSetter>().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
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
		var expectations = Rock.Create<IInterfaceIndexerSetter>();
		expectations.Indexers().Setters().This(3, "b", 4).CallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;

		expectations.Verify();
	}
}