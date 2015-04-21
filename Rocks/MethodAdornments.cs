using System;

namespace Rocks
{
	public class MethodAdornments
	{
		protected HandlerInformation handler;

		public MethodAdornments(HandlerInformation handler)
		{
			this.handler = handler;
		}

		public void Raises(string eventName, EventArgs args)
		{
			this.handler.AddRaiseEvent(new RaiseEventInformation(eventName, args));
		}
	}
}
