using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public class Events
	{
		public event EventHandler Target;

		public static void Test()
		{
			var @event = new Events();
			@event.Target += (s, e) => Console.Out.WriteLine("Event was raised.");
			@event.Raise(nameof(Events.Target), EventArgs.Empty);
		}

		public void Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName,
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { this, args });
				}
			}
		}
	}
}
