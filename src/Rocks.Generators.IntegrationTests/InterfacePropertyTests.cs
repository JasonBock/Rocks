using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IInterfacePropertyTests
	{
		int GetData { get; }
		int GetAndSetData { get; set; }
#pragma warning disable CA1044 // Properties should not be write only
		int SetData { set; }
#pragma warning restore CA1044 // Properties should not be write only

		event EventHandler MyEvent;
	}

	public static class InterfacePropertyTests
	{
		[Test]
		public static void MockGet()
		{
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().GetGetData();

			var chunk = rock.Instance();
			var value = chunk.GetData;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockGetWithRaiseEvent()
		{
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().GetGetData().RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
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
		public static void MockGetWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().GetGetData().Callback(() =>
			{
				wasCallbackInvoked = true;
				return 3;
			});

			var chunk = rock.Instance();
			var value = chunk.GetData;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void MockGetWithRaiseEventAndCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().GetGetData().Callback(() =>
			{
				wasCallbackInvoked = true;
				return 3;
			}).RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
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
		public static void MockSet()
		{
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().SetSetData(Arg.Any<int>());

			var chunk = rock.Instance();
			chunk.SetData = 1;

			rock.Verify();
		}

		[Test]
		public static void MockSetWithRaiseEvent()
		{
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().SetSetData(Arg.Any<int>())
				.RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.SetData = 1;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
			});
		}

		[Test]
		public static void MockSetWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().SetSetData(Arg.Any<int>())
				.Callback(_ => wasCallbackInvoked = true);

			var chunk = rock.Instance();
			chunk.SetData = 1;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void MockSetWithRaiseEventAndCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().SetSetData(Arg.Any<int>())
				.RaisesMyEvent(EventArgs.Empty)
				.Callback(_ => wasCallbackInvoked = true);

			var wasEventRaised = false;
			var chunk = rock.Instance();
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
		public static void MockGetAndSet()
		{
			var rock = Rock.Create<IInterfacePropertyTests>();
			rock.Properties().GetGetAndSetData();
			rock.Properties().SetGetAndSetData(Arg.Any<int>());

			var chunk = rock.Instance();
			var value = chunk.GetAndSetData;
			chunk.GetAndSetData = value;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}
	}
}