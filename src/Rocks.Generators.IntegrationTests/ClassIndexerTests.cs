using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
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
		public static void MockWithOneParameterGetterAndSetter()
		{
			var rock = Rock.Create<ClassIndexerGetterSetter>();
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
		public static void MockWithOneParameterGetter()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithOneParameterGetterRaiseEvent()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithOneParameterGetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithOneParameterGetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithOneParameterGetterMultipleCalls()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithOneParameterSetter()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
			rock.Indexers().Setters().This(3, 4);

			var chunk = rock.Instance();
			chunk[3] = 4;

			rock.Verify();
		}

		[Test]
		public static void MockWithOneParameterSetterRaiseEvent()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithOneParameterSetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithOneParameterSetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithOneParameterSetterMultipleCalls()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
			rock.Indexers().Setters().This(3, 4).CallCount(2);

			var chunk = rock.Instance();
			chunk[3] = 4;
			chunk[3] = 4;

			rock.Verify();
		}

		[Test]
		public static void MockWithMultipleParametersGetterAndSetter()
		{
			var rock = Rock.Create<ClassIndexerGetterSetter>();
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
		public static void MockWithMultipleParametersGetter()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithMultipleParametersGetterRaiseEvent()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithMultipleParametersGetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithMultipleParametersGetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithMultipleParametersGetterMultipleCalls()
		{
			var rock = Rock.Create<ClassIndexerGetter>();
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
		public static void MockWithMultipleParametersSetter()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4);

			var chunk = rock.Instance();
			chunk[3, "b"] = 4;

			rock.Verify();
		}

		[Test]
		public static void MockWithMultipleParametersSetterRaiseEvent()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithMultipleParametersSetterCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithMultipleParametersSetterRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassIndexerSetter>();
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
		public static void MockWithMultipleParametersSetterMultipleCalls()
		{
			var rock = Rock.Create<ClassIndexerSetter>();
			rock.Indexers().Setters().This(3, "b", 4).CallCount(2);

			var chunk = rock.Instance();
			chunk[3, "b"] = 4;
			chunk[3, "b"] = 4;

			rock.Verify();
		}
	}
}