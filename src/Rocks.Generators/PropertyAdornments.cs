using System;

namespace Rocks
{
	public class PropertyAdornments<T>
		where T : class
	{
		protected PropertyAdornments(HandlerInformation handler) =>
			this.Handler = handler;

		public HandlerInformation Handler { get; }
	}

	public class PropertyAdornments<T, TCallback>
		: PropertyAdornments<T>
		where T : class
		where TCallback : Delegate
	{
		public PropertyAdornments(HandlerInformation handler)
			: base(handler) 
		{ }

		public PropertyAdornments<T, TCallback> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public PropertyAdornments<T, TCallback> Callback(TCallback callback)
		{
			this.Handler.SetCallback(callback);
			return this;
		}
	}

	public sealed class PropertyAdornments<T, TCallback, TResult>
		: PropertyAdornments<T, TCallback>
		where T : class
		where TCallback : Delegate
	{
		public PropertyAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public PropertyAdornments<T, TCallback, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
	}
}