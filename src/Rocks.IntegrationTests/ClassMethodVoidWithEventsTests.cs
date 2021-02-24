using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public class ClassMethodVoidWithEvents
	{
		public virtual void NoParameters() { }

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
		public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
	}

	public static class ClassMethodVoidWithEventsTests
	{
		[Test]
		public static void CreateEvent()
		{
			var rock = Rock.Create<ClassMethodVoidWithEvents>();
			rock.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.NoParameters();

			rock.Verify();

			Assert.That(wasEventRaised, Is.True);
		}

		[Test]
		public static void CreateEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<ClassMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.Callback(() => wasCallbackInvoked = true)
				.RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateEventWithMultipleCalls()
		{
			var rock = Rock.Create<ClassMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();

			Assert.That(eventRaisedCount, Is.EqualTo(2));
		}

		[Test]
		public static void CreateEventWithMultipleCallsWithCallback()
		{
			var callbackInvokedCount = 0;
			var rock = Rock.Create<ClassMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.Callback(() => callbackInvokedCount++)
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(eventRaisedCount, Is.EqualTo(2));
				Assert.That(callbackInvokedCount, Is.EqualTo(2));
			});
		}
	}
}