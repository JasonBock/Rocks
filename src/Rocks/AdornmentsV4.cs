namespace Rocks;

/// <summary>
/// Defines adornments for a mocked member.
/// </summary>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
public sealed class AdornmentsV4<THandler, TCallback>
	where THandler : HandlerV4<TCallback>
	where TCallback : Delegate
{
	private readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="AdornmentsV4{THandler, TCallback}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public AdornmentsV4(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public AdornmentsV4<THandler, TCallback> ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public AdornmentsV4<THandler, TCallback> Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return this;
	}
}

/// <summary>
/// Defines adornments for a mocked member.
/// </summary>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TReturnValue">The return value type.</typeparam>
public sealed class AdornmentsV4<THandler, TCallback, TReturnValue>
	where THandler : HandlerV4<TCallback, TReturnValue>
	where TCallback : Delegate
{
	private readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="AdornmentsV4{THandler, TCallback, TReturnValue}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public AdornmentsV4(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public AdornmentsV4<THandler, TCallback, TReturnValue> ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public AdornmentsV4<THandler, TCallback, TReturnValue> Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	public AdornmentsV4<THandler, TCallback, TReturnValue> ReturnValue(TReturnValue returnValue)
	{
		this.handler.ReturnValue = returnValue;
		return this;
	}
}