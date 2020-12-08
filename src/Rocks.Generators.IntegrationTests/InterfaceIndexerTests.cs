using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
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
		public static void CreateWithOneParameterGetterAndSetter()
		{
			var rock = Rock.Create<IInterfaceIndexerGetterSetter>();
			rock.Indexers().Getters().This(3);
			rock.Indexers().Setters().This(3, 4);

			var chunk = rock.Instance();
			var value = chunk[3];
			chunk[3] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MakeWithOneParameterGetterAndSetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerGetterSetter>().Instance();
			var value = chunk[3];
			
			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
				Assert.That(() => chunk[3] = 4, Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithOneParameterGetter()
		{
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3);

			var chunk = rock.Instance();
			var value = chunk[3];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MakeWithOneParameterGetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerGetter>().Instance();
			var value = chunk[3];

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithOneParameterGetterRaiseEvent()
		{
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3).RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk[3];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3).Callback(_ =>
			{
				wasCallbackInvoked = true;
				return _;
			});

			var chunk = rock.Instance();
			var value = chunk[3];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3).RaisesMyEvent(EventArgs.Empty)
				.Callback(_ =>
				{
					wasCallbackInvoked = true;
					return _;
				});

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk[3];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3).CallCount(2);

			var chunk = rock.Instance();
			var value = chunk[3];
			value = chunk[3];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithOneParameterSetter()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, 4);

			var chunk = rock.Instance();
			chunk[3] = 4;

			rock.Verify();
		}

		[Test]
		public static void MakeWithOneParameterSetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerSetter>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk[3] = 4, Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithOneParameterSetterRaiseEvent()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, 4).RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk[3] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
			});
		}

		[Test]
		public static void CreateWithOneParameterSetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, 4).Callback((a, value) => wasCallbackInvoked = true);

			var chunk = rock.Instance();
			chunk[3] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateWithOneParameterSetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, 4).RaisesMyEvent(EventArgs.Empty)
				.Callback((a, value) => wasCallbackInvoked = true);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk[3] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateWithOneParameterSetterMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, 4).CallCount(2);

			var chunk = rock.Instance();
			chunk[3] = 4;
			chunk[3] = 4;

			rock.Verify();
		}

		[Test]
		public static void CreateWithMultipleParametersGetterAndSetter()
		{
			var rock = Rock.Create<IInterfaceIndexerGetterSetter>();
			rock.Indexers().Getters().This(3, "b");
			rock.Indexers().Setters().This(3, "b", 4);

			var chunk = rock.Instance();
			var value = chunk[3, "b"];
			chunk[3, "b"] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MakeWithMultipleParametersGetterAndSetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerGetterSetter>().Instance();
			var value = chunk[3, "b"];
			
			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
				Assert.That(() => chunk[3, "b"] = 4, Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithMultipleParametersGetter()
		{
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3, "b");

			var chunk = rock.Instance();
			var value = chunk[3, "b"];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MakeWithMultipleParametersGetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerGetter>().Instance();
			var value = chunk[3, "b"];

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithMultipleParametersGetterRaiseEvent()
		{
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3, "b").RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk[3, "b"];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3, "b").Callback((a, b) =>
			{
				wasCallbackInvoked = true;
				return a;
			});

			var chunk = rock.Instance();
			var value = chunk[3, "b"];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3, "b").RaisesMyEvent(EventArgs.Empty)
				.Callback((a, b) =>
				{
					wasCallbackInvoked = true;
					return a;
				});

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk[3, "b"];

			rock.Verify();

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
			var rock = Rock.Create<IInterfaceIndexerGetter>();
			rock.Indexers().Getters().This(3, "b").CallCount(2);

			var chunk = rock.Instance();
			var value = chunk[3, "b"];
			value = chunk[3, "b"];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithMultipleParametersSetter()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4);

			var chunk = rock.Instance();
			chunk[3, "b"] = 4;

			rock.Verify();
		}

		[Test]
		public static void MakeWithMultipleParametersSetter()
		{
			var chunk = Rock.Make<IInterfaceIndexerSetter>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk[3, "b"] = 4, Throws.Nothing);
			});		
		}

		[Test]
		public static void CreateWithMultipleParametersSetterRaiseEvent()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4).RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk[3, "b"] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
			});
		}

		[Test]
		public static void CreateWithMultipleParametersSetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4).Callback((a, b, value) => wasCallbackInvoked = true);

			var chunk = rock.Instance();
			chunk[3, "b"] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateWithMultipleParametersSetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4).RaisesMyEvent(EventArgs.Empty)
				.Callback((a, b, value) => wasCallbackInvoked = true);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk[3, "b"] = 4;

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateWithMultipleParametersSetterMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4).CallCount(2);

			var chunk = rock.Instance();
			chunk[3, "b"] = 4;
			chunk[3, "b"] = 4;

			rock.Verify();
		}
	}
}