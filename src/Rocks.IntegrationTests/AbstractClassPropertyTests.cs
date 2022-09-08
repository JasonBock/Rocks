using NUnit.Framework;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassProperty
{
	public abstract int GetData { get; }
	public abstract int GetAndInitData { get; init; }
	public abstract int GetAndSetData { get; set; }
#pragma warning disable CA1044 // Properties should not be write only
	public abstract int InitData { init; }
	public abstract int SetData { set; }
#pragma warning restore CA1044 // Properties should not be write only
	public abstract event EventHandler MyEvent;
}

public static class AbstractClassPropertyTests
{
	[Test]
	public static void CreateGet()
	{
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetData();

		var chunk = rock.Instance(null);
		var value = chunk.GetData;

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeGet()
	{
		var chunk = Rock.Make<AbstractClassProperty>().Instance(null);
		var value = chunk.GetData;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateGetWithRaiseEvent()
	{
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetData().RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var chunk = rock.Instance(null);
		chunk.MyEvent += (s, e) => wasEventRaised = true;
		var value = chunk.GetData;

		rock.Verify();

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
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetData().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		});

		var chunk = rock.Instance(null);
		var value = chunk.GetData;

		rock.Verify();

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
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetData().Callback(() =>
		{
			wasCallbackInvoked = true;
			return 3;
		}).RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var chunk = rock.Instance(null);
		chunk.MyEvent += (s, e) => wasEventRaised = true;
		var value = chunk.GetData;

		rock.Verify();

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
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Setters().SetData(Arg.Any<int>());

		var chunk = rock.Instance(null);
		chunk.SetData = 1;

		rock.Verify();
	}

	[Test]
	public static void MakeSet()
	{
		var chunk = Rock.Make<AbstractClassProperty>().Instance(null);

		Assert.That(() => chunk.SetData = 1, Throws.Nothing);
	}

	[Test]
	public static void CreateSetWithRaiseEvent()
	{
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Setters().SetData(Arg.Any<int>())
			.RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var chunk = rock.Instance(null);
		chunk.MyEvent += (s, e) => wasEventRaised = true;
		chunk.SetData = 1;

		rock.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateSetWithCallback()
	{
		var wasCallbackInvoked = false;
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Setters().SetData(Arg.Any<int>())
			.Callback(_ => wasCallbackInvoked = true);

		var chunk = rock.Instance(null);
		chunk.SetData = 1;

		rock.Verify();

		Assert.That(wasCallbackInvoked, Is.True);
	}

	[Test]
	public static void CreateSetWithRaiseEventAndCallback()
	{
		var wasCallbackInvoked = false;
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Setters().SetData(Arg.Any<int>())
			.RaisesMyEvent(EventArgs.Empty)
			.Callback(_ => wasCallbackInvoked = true);

		var wasEventRaised = false;
		var chunk = rock.Instance(null);
		chunk.MyEvent += (s, e) => wasEventRaised = true;
		chunk.SetData = 1;

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	public static void CreateGetAndInit()
	{
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetAndInitData();

		var chunk = rock.Instance(null);
		var value = chunk.GetAndInitData;

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateGetAndSet()
	{
		var rock = Rock.Create<AbstractClassProperty>();
		rock.Properties().Getters().GetAndSetData();
		rock.Properties().Setters().GetAndSetData(Arg.Any<int>());

		var chunk = rock.Instance(null);
		var value = chunk.GetAndSetData;
		chunk.GetAndSetData = value;

		rock.Verify();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeGetAndInit()
	{
		var chunk = Rock.Make<AbstractClassProperty>().Instance(null);
		var value = chunk.GetAndInitData;

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
		});
	}

	[Test]
	public static void MakeGetAndSet()
	{
		var chunk = Rock.Make<AbstractClassProperty>().Instance(null);
		var value = chunk.GetAndSetData;

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => chunk.GetAndSetData = value, Throws.Nothing);
		});
	}
}