using NUnit.Framework;
using Rocks.Runtime;

namespace Rocks.Analysis.IntegrationTests.ClassPropertyTestTypes;

public class ClassProperty
{
	public virtual int GetData => default;
	public virtual int GetAndInitData { get => default; init { } }
	public virtual int GetAndSetData { get => default; set { } }
#pragma warning disable CA1044 // Properties should not be write only
   public virtual int InitData { init { } }
   public virtual int SetData { set { } }
#pragma warning restore CA1044 // Properties should not be write only

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassPropertyTests
{
	[Test]
	public static void CreateGet()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetData();

		var mock = expectations.Instance(null);
		var value = mock.GetData;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeGet()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetData;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateGetWithRaiseEvent()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetData().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.GetData;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(wasEventRaised, Is.True);
		});
	}

	[Test]
	public static void CreateGetWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetData().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		});

		var mock = expectations.Instance(null);
		var value = mock.GetData;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateGetWithRaiseEventAndCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetData().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		})
		.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.GetData;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateSet()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Setters.SetData(Arg.Any<int>());

		var mock = expectations.Instance(null);
		mock.SetData = 1;

		expectations.Verify();
	}

	[Test]
	public static void MakeSet()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);

		Assert.That(() => mock.SetData = 1, Throws.Nothing);
	}

	[Test]
	public static void CreateSetWithRaiseEvent()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Setters.SetData(Arg.Any<int>())
			.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.SetData = 1;

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateSetWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Setters.SetData(Arg.Any<int>())
			.Callback(_ => wasCallbackInvoked = true);

		var mock = expectations.Instance(null);
		mock.SetData = 1;

		expectations.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateSetWithRaiseEventAndCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Setters.SetData(Arg.Any<int>())
			.RaiseMyEvent(EventArgs.Empty)
			.Callback(_ => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.SetData = 1;

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateGetAndInit()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetAndInitData();

		var mock = expectations.Instance(null);
		var value = mock.GetAndInitData;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateGetAndSet()
	{
		var expectations = new ClassPropertyCreateExpectations();
		expectations.Properties.Getters.GetAndSetData();
		expectations.Properties.Setters.GetAndSetData(Arg.Any<int>());

		var mock = expectations.Instance(null);
		var value = mock.GetAndSetData;
		mock.GetAndSetData = value;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeGetAndInit()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetAndInitData;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeGetAndSet()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetAndSetData;

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock.GetAndSetData = value, Throws.Nothing);
		});
	}
}