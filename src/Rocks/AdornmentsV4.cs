﻿namespace Rocks;

/// <summary>
/// Defines adornments for a mocked member.
/// </summary>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
public class AdornmentsV4<THandler, TCallback>
	where THandler : HandlerV4<TCallback>
	where TCallback : Delegate
{
	/// <summary>
	/// The handler wrapped by this adornments instance.
	/// </summary>
	protected readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="AdornmentsV4{THandler, TCallback}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public AdornmentsV4(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public AdornmentsV4<THandler, TCallback> AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return this;
	}

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
	: AdornmentsV4<THandler, TCallback>
	where THandler : HandlerV4<TCallback, TReturnValue>
	where TCallback : Delegate
{
	/// <summary>
	/// Creates a new <see cref="AdornmentsV4{THandler, TCallback, TReturnValue}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public AdornmentsV4(THandler handler) : base(handler) { }

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

/// <summary>
/// Defines adornments for a mocked member with a pointer-based return value
/// </summary>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TReturnValue">The return value type.</typeparam>
public unsafe sealed class PointerAdornmentsV4<THandler, TCallback, TReturnValue>
	where THandler : PointerHandlerV4<TCallback, TReturnValue>
	where TCallback : Delegate
	where TReturnValue : unmanaged
{
	private readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="AdornmentsV4{THandler, TCallback, TReturnValue}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public PointerAdornmentsV4(THandler handler) =>
		this.handler = handler;

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public PointerAdornmentsV4<THandler, TCallback, TReturnValue> AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return this;
	}

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public PointerAdornmentsV4<THandler, TCallback, TReturnValue> ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	public PointerAdornmentsV4<THandler, TCallback, TReturnValue> Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	public PointerAdornmentsV4<THandler, TCallback, TReturnValue> ReturnValue(TReturnValue* returnValue)
	{
		this.handler.ReturnValue = returnValue;
		return this;
	}
}