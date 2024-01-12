namespace Rocks;

/// <summary>
/// Defines adornments for a mocked member.
/// </summary>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
public sealed class Adornments<THandler, TCallback>
	where THandler : Handler<TCallback>
	where TCallback : Delegate
{
	/// <summary>
	/// The handler wrapped by this adornments instance.
	/// </summary>
	private readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="Adornments{THandler, TCallback}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public Adornments(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public Adornments<THandler, TCallback> AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return this;
	}

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public Adornments<THandler, TCallback> ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public Adornments<THandler, TCallback> Callback(TCallback callback)
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
public sealed class Adornments<THandler, TCallback, TReturnValue>
	where THandler : Handler<TCallback, TReturnValue>
	where TCallback : Delegate
{
	/// <summary>
	/// The handler wrapped by this adornments instance.
	/// </summary>
	private readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="Adornments{THandler, TCallback, TReturnValue}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public Adornments(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public Adornments<THandler, TCallback, TReturnValue> AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return this;
	}

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public Adornments<THandler, TCallback, TReturnValue> ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public Adornments<THandler, TCallback, TReturnValue> Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	public Adornments<THandler, TCallback, TReturnValue> ReturnValue(TReturnValue returnValue)
	{
		this.handler.ReturnValue = returnValue;
		return this;
	}
}