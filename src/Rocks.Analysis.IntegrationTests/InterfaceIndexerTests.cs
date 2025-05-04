using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.InterfaceIndexerTestTypes;

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
#pragma warning disable CA1044 // Properties should not be write only
   int this[int a] { init; }
   int this[int a, string b] { init; }
#pragma warning restore CA1044 // Properties should not be write only

	event EventHandler MyEvent;
}

public interface IInterfaceIndexerSetter
{
#pragma warning disable CA1044 // Properties should not be write only
	int this[int a] { set; }
	int this[int a, string b] { set; }
#pragma warning restore CA1044 // Properties should not be write only

	event EventHandler MyEvent;
}

public static class InterfaceIndexerTests
{
	[Test]
	public static void CreateWithOneParameterGetterAndInit()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterInitCreateExpectations>();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance(null);
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterSetterCreateExpectations>();
		expectations.Indexers.Getters.This(3);
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetterAndInit()
	{
		var mock = new IInterfaceIndexerGetterInitMakeExpectations().Instance(null);
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = new IInterfaceIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.Default);
			Assert.That(() => mock[3] = 4, Throws.Nothing);
		});
	}

	[Test]
	public static void CreateWithOneParameterGetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3);

		var mock = expectations.Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = new IInterfaceIndexerGetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3)
			.RaiseMyEvent(EventArgs.Empty)
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3];
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterInit()
	{
		var expectations = new IInterfaceIndexerInitCreateExpectations();
		Assert.That(() => expectations.Instance(null), Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3);

		var mock = expectations.Instance();
		mock[3] = 4;
	}

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = new IInterfaceIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3)
			.RaiseMyEvent(EventArgs.Empty)
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
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3).ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterSetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b");
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = new IInterfaceIndexerGetterSetterMakeExpectations().Instance();
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b");

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = new IInterfaceIndexerGetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b")
			.RaiseMyEvent(EventArgs.Empty);

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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
		expectations.Indexers.Getters.This(3, "b")
			.RaiseMyEvent(EventArgs.Empty)
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
		var expectations = context.Create<IInterfaceIndexerGetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b");

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = new IInterfaceIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b")
			.RaiseMyEvent(EventArgs.Empty);

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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b")
			.RaiseMyEvent(EventArgs.Empty)
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
		var expectations = context.Create<IInterfaceIndexerSetterCreateExpectations>();
		expectations.Indexers.Setters.This(4, 3, "b").ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;
	}
}