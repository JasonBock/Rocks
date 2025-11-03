using NUnit.Framework;

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

internal static class ClassPropertyTests
{
	[Test]
	public static void CreateGet()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetData.Gets();

		var mock = expectations.Instance(null);
		var value = mock.GetData;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeGet()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetData;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateGetWithRaiseEvent()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetData.Gets().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.GetData;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.Default);
			Assert.That(wasEventRaised, Is.True);
		}
	}

	[Test]
	public static void CreateGetWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetData.Gets().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		});

		var mock = expectations.Instance(null);
		var value = mock.GetData;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateGetWithRaiseEventAndCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetData.Gets().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		})
		.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.GetData;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.EqualTo(3));
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateSet()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.SetData.Sets(Arg.Any<int>());

		var mock = expectations.Instance(null);
		mock.SetData = 1;
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
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.SetData.Sets(Arg.Any<int>())
			.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.SetData = 1;

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateSetWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.SetData.Sets(Arg.Any<int>())
			.Callback(_ => wasCallbackInvoked = true);

		var mock = expectations.Instance(null);
		mock.SetData = 1;

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateSetWithRaiseEventAndCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.SetData.Sets(Arg.Any<int>())
			.RaiseMyEvent(EventArgs.Empty)
			.Callback(_ => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var mock = expectations.Instance(null);
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.SetData = 1;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateGetAndInit()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetAndInitData.Gets();

		var mock = expectations.Instance(null);
		var value = mock.GetAndInitData;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateGetAndSet()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassPropertyCreateExpectations>();
		expectations.Setups.GetAndSetData.Gets();
		expectations.Setups.GetAndSetData.Sets(Arg.Any<int>());

		var mock = expectations.Instance(null);
		var value = mock.GetAndSetData;
		mock.GetAndSetData = value;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeGetAndInit()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetAndInitData;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeGetAndSet()
	{
		var mock = new ClassPropertyMakeExpectations().Instance(null);
		var value = mock.GetAndSetData;

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.Default);
			Assert.That(() => mock.GetAndSetData = value, Throws.Nothing);
		}
	}
}