using System;

namespace Rocks
{
	public abstract class MethodAdornments<T>
		where T : class
	{
		public MethodAdornments(HandlerInformation handler) =>
			this.Handler = handler;

		public HandlerInformation Handler { get; }
	}

	public class MethodAdornments<T, TCallback>
		: MethodAdornments<T>
		where T : class
		where TCallback : Delegate
	{
		public MethodAdornments(HandlerInformation handler)
			: base(handler) 
		{ }

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
	}

	public sealed class MethodAdornments<T, TCallback, TResult>
		: MethodAdornments<T, TCallback>
		where T : class
		where TCallback : Delegate
	{
		public MethodAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public MethodAdornments<T, TCallback, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
	}
}