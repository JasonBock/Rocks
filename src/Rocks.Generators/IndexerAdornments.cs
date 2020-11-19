using System;

namespace Rocks
{
	public abstract class IndexerAdornments<T>
		where T : class
	{
		protected IndexerAdornments(HandlerInformation handler) =>
			this.Handler = handler;

		public HandlerInformation Handler { get; }
	}

	public class IndexerAdornments<T, TCallback>
		: IndexerAdornments<T>
		where T : class
		where TCallback : Delegate
	{
		public IndexerAdornments(HandlerInformation handler)
			: base(handler)
		{ }

		public IndexerAdornments<T, TCallback> CallCount(uint expectedCallCount)
		{
			this.Handler.SetExpectedCallCount(expectedCallCount);
			return this;
		}

		public IndexerAdornments<T, TCallback> Callback(TCallback callback)
		{
			this.Handler.SetCallback(callback);
			return this;
		}
	}

	public sealed class IndexerAdornments<T, TCallback, TResult>
		: IndexerAdornments<T, TCallback>
		where T : class
		where TCallback : Delegate
	{
		public IndexerAdornments(HandlerInformation<TResult> handler)
			: base(handler)
		{ }

		public IndexerAdornments<T, TCallback, TResult> Returns(TResult returnValue)
		{
			((HandlerInformation<TResult>)this.Handler).ReturnValue = returnValue;
			return this;
		}
	}
}