using System;

namespace Rocks
{
	public class PropertyAdornments<T, TCallback>
		: IAdornments<HandlerInformation>
		where T : class
		where TCallback : Delegate
	{
		public PropertyAdornments(HandlerInformation handler) =>
			this.Handler = handler;

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

		public HandlerInformation Handler { get; }
	}

	public sealed class PropertyAdornments<T, TCallback, TResult>
		: IAdornments<HandlerInformation<TResult>>
		where T : class
		where TCallback : Delegate
	{
		public PropertyAdornments(HandlerInformation<TResult> handler) =>
			this.Handler = handler;

		public PropertyAdornments<T, TCallback, TResult> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public PropertyAdornments<T, TCallback, TResult> Callback(TCallback callback)
		{
			this.Handler.SetCallback(callback);
			return this;
		}

		public PropertyAdornments<T, TCallback, TResult> Returns(TResult returnValue)
		{
			this.Handler.ReturnValue = returnValue;
			return this;
		}

		public HandlerInformation<TResult> Handler { get; }
	}
}