using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ClassIndexerTestTypes;

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
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterInitCreateExpectations>();
		expectations.Setups[3].Gets();

		var mock = expectations.Instance(null);
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterAndSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterSetterCreateExpectations>();
		expectations.Setups[3].Gets();
		expectations.Setups[3].Sets(4);

		var mock = expectations.Instance();
		var value = mock[3];
		mock[3] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetterAndInit()
	{
		var mock = new ClassIndexerGetterInitMakeExpectations().Instance(null);
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetterAndSetter()
	{
		var mock = new ClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Default);
			Assert.That(() => mock[3] = 4, Throws.Nothing);
		}
	}

	[Test]
	public static void CreateWithOneParameterGetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3].Gets();

		var mock = expectations.Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithOneParameterGetter()
	{
		var mock = new ClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3].Gets().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Default);
			Assert.That(wasEventRaised, Is.True);
		}
	}

	[Test]
	public static void CreateWithOneParameterGetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3].Gets().Callback(_ =>
		{
			wasCallbackInvoked = true;
			return _;
		});

		var mock = expectations.Instance();
		var value = mock[3];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithOneParameterGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3].Gets().RaiseMyEvent(EventArgs.Empty)
			.Callback(_ =>
			{
				wasCallbackInvoked = true;
				return _;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithOneParameterGetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3].Gets().ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3];
		var value = mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithOneParameterInit()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerInitCreateExpectations>();
		_ = expectations.Instance(null);
	}

	[Test]
	public static void CreateWithOneParameterSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3].Sets(4);

		var mock = expectations.Instance();
		mock[3] = 4;
	}

	[Test]
	public static void MakeWithOneParameterInit() => 
		Assert.That(() => new ClassIndexerInitMakeExpectations().Instance(null), Throws.Nothing);

	[Test]
	public static void MakeWithOneParameterSetter()
	{
		var mock = new ClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3].Sets(4).RaiseMyEvent(EventArgs.Empty);

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
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3].Sets(4).Callback((a, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3] = 4;

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithOneParameterSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3].Sets(4).RaiseMyEvent(EventArgs.Empty)
			.Callback((a, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3] = 4;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithOneParameterSetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3].Sets(4).ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3] = 4;
		mock[3] = 4;
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndInit()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterInitCreateExpectations>();
		expectations.Setups[3, "b"].Gets();

		var mock = expectations.Instance(null);
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersGetterAndSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterSetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets();
		expectations.Setups[3, "b"].Sets(4);

		var mock = expectations.Instance();
		var value = mock[3, "b"];
		mock[3, "b"] = 4;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndInit()
	{
		var mock = new ClassIndexerGetterInitMakeExpectations().Instance(null);
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetterAndSetter()
	{
		var mock = new ClassIndexerGetterSetterMakeExpectations().Instance();
		var value = mock[3, "b"];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Default);
			Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
		}
	}

	[Test]
	public static void CreateWithMultipleParametersGetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets();

		var mock = expectations.Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeWithMultipleParametersGetter()
	{
		var mock = new ClassIndexerGetterMakeExpectations().Instance();
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Default);
			Assert.That(wasEventRaised, Is.True);
		}
	}

	[Test]
	public static void CreateWithMultipleParametersGetterCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets().Callback((a, b) =>
		{
			wasCallbackInvoked = true;
			return a;
		});

		var mock = expectations.Instance();
		var value = mock[3, "b"];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithMultipleParametersGetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets().RaiseMyEvent(EventArgs.Empty)
			.Callback((a, b) =>
			{
				wasCallbackInvoked = true;
				return a;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock[3, "b"];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithMultipleParametersGetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerGetterCreateExpectations>();
		expectations.Setups[3, "b"].Gets().ExpectedCallCount(2);

		var mock = expectations.Instance();
		_ = mock[3, "b"];
		var value = mock[3, "b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithMultipleParametersSetter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3, "b"].Sets(4);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
	}

	[Test]
	public static void MakeWithMultipleParametersSetter()
	{
		var mock = new ClassIndexerSetterMakeExpectations().Instance();

		Assert.That(() => mock[3, "b"] = 4, Throws.Nothing);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3, "b"].Sets(4).RaiseMyEvent(EventArgs.Empty);

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
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3, "b"].Sets(4).Callback((a, b, value) => wasCallbackInvoked = true);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3, "b"].Sets(4).RaiseMyEvent(EventArgs.Empty)
			.Callback((a, b, value) => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock[3, "b"] = 4;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateWithMultipleParametersSetterMultipleCalls()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassIndexerSetterCreateExpectations>();
		expectations.Setups[3, "b"].Sets(4).ExpectedCallCount(2);

		var mock = expectations.Instance();
		mock[3, "b"] = 4;
		mock[3, "b"] = 4;
	}
}