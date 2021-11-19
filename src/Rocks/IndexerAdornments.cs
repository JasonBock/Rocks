namespace Rocks;

public class IndexerAdornments<T, TCallback>
	 : IAdornments<HandlerInformation>
	 where T : class
	 where TCallback : Delegate
{
	public IndexerAdornments(HandlerInformation handler) =>
		this.Handler = handler;

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

	public HandlerInformation Handler { get; }
}

public sealed class IndexerAdornments<T, TCallback, TResult>
	: IAdornments<HandlerInformation<TResult>>
	where T : class
	where TCallback : Delegate
{
	public IndexerAdornments(HandlerInformation<TResult> handler) =>
		this.Handler = handler;

	public IndexerAdornments<T, TCallback, TResult> CallCount(uint expectedCallCount)
	{
		this.Handler.SetExpectedCallCount(expectedCallCount);
		return this;
	}

	public IndexerAdornments<T, TCallback, TResult> Callback(TCallback callback)
	{
		this.Handler.SetCallback(callback);
		return this;
	}

	public IndexerAdornments<T, TCallback, TResult> Returns(TResult returnValue)
	{
		this.Handler.ReturnValue = returnValue;
		return this;
	}

	public HandlerInformation<TResult> Handler { get; }
}