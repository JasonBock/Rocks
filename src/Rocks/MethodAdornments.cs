namespace Rocks;

/// <summary>
/// Defines adornments for a mocked method.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
public class MethodAdornments<T, TCallback>
	 : IAdornments<HandlerInformation>
	 where T : class
	 where TCallback : Delegate
{
	/// <summary>
	/// Creates a new <see cref="MethodAdornments{T, TCallback}"/> instance
	/// with a handler.
	/// </summary>
	/// <param name="handler">The handler to wrap.</param>
	public MethodAdornments(HandlerInformation handler) =>
		this.Handler = handler;

	/// <summary>
	/// Sets the handler with a call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public MethodAdornments<T, TCallback> CallCount(uint expectedCallCount)
	{
		this.Handler.SetExpectedCallCount(expectedCallCount);
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public MethodAdornments<T, TCallback> Callback(TCallback callback)
	{
		this.Handler.SetCallback(callback);
		return this;
	}

	/// <summary>
	/// Gets the handler.
	/// </summary>
	public HandlerInformation Handler { get; }
}

/// <summary>
/// Defines adornments for a mocked method
/// with a return value.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
public sealed class MethodAdornments<T, TCallback, TResult>
	: IAdornments<HandlerInformation<TResult>>
	where T : class
	where TCallback : Delegate
{
	/// <summary>
	/// Creates a new <see cref="MethodAdornments{T, TCallback, TResult}"/> instance
	/// with a handler.
	/// </summary>
	/// <param name="handler">The handler to wrap.</param>
	public MethodAdornments(HandlerInformation<TResult> handler) =>
		this.Handler = handler;

	/// <summary>
	/// Sets the handler with a call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public MethodAdornments<T, TCallback, TResult> CallCount(uint expectedCallCount)
	{
		this.Handler.SetExpectedCallCount(expectedCallCount);
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public MethodAdornments<T, TCallback, TResult> Callback(TCallback callback)
	{
		this.Handler.SetCallback(callback);
		return this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	public MethodAdornments<T, TCallback, TResult> Returns(TResult returnValue)
	{
		this.Handler.ReturnValue = returnValue;
		return this;
	}

	/// <summary>
	/// Gets the handler.
	/// </summary>
	public HandlerInformation<TResult> Handler { get; }
}