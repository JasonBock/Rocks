using System;

namespace Rocks
{
	public class MethodAdornments<T>
		where T : class
	{
		public MethodAdornments(HandlerInformation handler) => 
			this.Handler = handler;

		public MethodAdornments<T> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public HandlerInformation Handler { get; }
	}
}