using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassMethodReturnWithEvents
{
   public virtual int NoParameters() => default;

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
   public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassMethodReturnWithEventsTests
{
   [Test]
   public static void CreateRaiseEvent()
   {
	  var rock = Rock.Create<ClassMethodReturnWithEvents>();
	  rock.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

	  var wasEventRaised = false;
	  var chunk = rock.Instance();
	  chunk.MyEvent += (s, e) => wasEventRaised = true;
	  var value = chunk.NoParameters();

	  rock.Verify();

	  Assert.Multiple(() =>
	  {
		 Assert.That(wasEventRaised, Is.True);
		 Assert.That(value, Is.EqualTo(default(int)));
	  });
   }

   [Test]
   public static void CreateRaiseEventWithCallback()
   {
	  var wasCallbackInvoked = false;
	  var rock = Rock.Create<ClassMethodReturnWithEvents>();
	  rock.Methods().NoParameters()
		  .Callback(() =>
		  {
			 wasCallbackInvoked = true;
			 return 3;
		  })
		  .RaisesMyEvent(EventArgs.Empty);

	  var wasEventRaised = false;
	  var chunk = rock.Instance();
	  chunk.MyEvent += (s, e) => wasEventRaised = true;
	  var value = chunk.NoParameters();

	  rock.Verify();

	  Assert.Multiple(() =>
	  {
		 Assert.That(wasEventRaised, Is.True);
		 Assert.That(wasCallbackInvoked, Is.True);
		 Assert.That(value, Is.EqualTo(3));
	  });
   }

   [Test]
   public static void CreateRaiseEventWithMultipleCalls()
   {
	  var rock = Rock.Create<ClassMethodReturnWithEvents>();
	  rock.Methods().NoParameters()
		  .CallCount(2)
		  .RaisesMyEvent(EventArgs.Empty);

	  var eventRaisedCount = 0;
	  var chunk = rock.Instance();
	  chunk.MyEvent += (s, e) => eventRaisedCount++;
	  var valueOne = chunk.NoParameters();
	  var valueTwo = chunk.NoParameters();

	  rock.Verify();

	  Assert.Multiple(() =>
	  {
		 Assert.That(eventRaisedCount, Is.EqualTo(2));
		 Assert.That(valueOne, Is.EqualTo(default(int)));
		 Assert.That(valueTwo, Is.EqualTo(default(int)));
	  });
   }

   [Test]
   public static void CreateRaiseEventWithMultipleCallsWithCallback()
   {
	  var callbackInvokedCount = 0;
	  var rock = Rock.Create<ClassMethodReturnWithEvents>();
	  rock.Methods().NoParameters()
		  .CallCount(2)
		  .Callback(() =>
		  {
			 callbackInvokedCount++;
			 return 3;
		  })
		  .RaisesMyEvent(EventArgs.Empty);

	  var eventRaisedCount = 0;
	  var chunk = rock.Instance();
	  chunk.MyEvent += (s, e) => eventRaisedCount++;
	  var valueOne = chunk.NoParameters();
	  var valueTwo = chunk.NoParameters();

	  rock.Verify();

	  Assert.Multiple(() =>
	  {
		 Assert.That(eventRaisedCount, Is.EqualTo(2));
		 Assert.That(callbackInvokedCount, Is.EqualTo(2));
		 Assert.That(valueOne, Is.EqualTo(3));
		 Assert.That(valueTwo, Is.EqualTo(3));
	  });
   }
}