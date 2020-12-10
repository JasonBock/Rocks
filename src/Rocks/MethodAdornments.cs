using System;

namespace Rocks
{
	public class MethodAdornments<T, TCallback>
		: IAdornments<HandlerInformation>
		where T : class
		where TCallback : Delegate
	{
		public MethodAdornments(HandlerInformation handler) =>
			this.Handler = handler;

		public MethodAdornments<T, TCallback> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public MethodAdornments<T, TCallback> Callback(TCallback callback)
		{
			this.Handler.SetCallback(callback);
			return this;
		}

		public HandlerInformation Handler { get; }
	}

	public sealed class MethodAdornments<T, TCallback, TResult>
		: IAdornments<HandlerInformation<TResult>>
		where T : class
		where TCallback : Delegate
	{
		public MethodAdornments(HandlerInformation<TResult> handler) =>
			this.Handler = handler;

		public MethodAdornments<T, TCallback, TResult> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public MethodAdornments<T, TCallback, TResult> Callback(TCallback callback)
		{
			this.Handler.SetCallback(callback);
			return this;
		}

		public MethodAdornments<T, TCallback, TResult> Returns(TResult returnValue)
		{
			this.Handler.ReturnValue = returnValue;
			return this;
		}

		public HandlerInformation<TResult> Handler { get; }
	}
}