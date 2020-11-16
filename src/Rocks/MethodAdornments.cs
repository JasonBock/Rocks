using System;

namespace Rocks
{
	public class MethodAdornments
	{
		public MethodAdornments(HandlerInformation handler) => this.Handler = handler;

		public MethodAdornments Raises(string eventName, EventArgs args)
		{
			this.Handler.AddRaiseEvent(new RaiseEventInformation(eventName, args));
			return this;
		}

		protected HandlerInformation Handler { get; }
	}
}