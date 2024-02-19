﻿namespace Rocks;

/// <summary>
/// Defines the interface for all adornments.
/// </summary>
/// <typeparam name="TAdornments">The adornments type.</typeparam>
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
public interface IAdornments<TAdornments>
	where TAdornments : IAdornments<TAdornments>
{
	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEventInformation">The event to raise.</param>
	/// <returns>The adornments instance.</returns>
	TAdornments AddRaiseEvent(RaiseEventInformation raiseEventInformation);

	/// <summary>
	/// Sets the handler with the expected call count and returns itself.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	/// <returns>The adornments instance.</returns>
	TAdornments ExpectedCallCount(uint expectedCallCount);
}

/// <summary>
/// Defines the interface for adornments
/// that have a callback.
/// </summary>
/// <typeparam name="TAdornments">The adornment.</typeparam>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
public interface IAdornments<TAdornments, THandler, TCallback>
	: IAdornments<TAdornments>
	where TAdornments : IAdornments<TAdornments, THandler, TCallback>
	where THandler : Handler<TCallback>
	where TCallback : Delegate
{
	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>The adornments instance.</returns>
	TAdornments Callback(TCallback callback);
}

/// <summary>
/// Defines the interface for adornments
/// that have a callback and a return value.
/// </summary>
/// <typeparam name="TAdornments">The adornment.</typeparam>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TReturnValue">The return value type.</typeparam>
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
public interface IAdornments<TAdornments, THandler, TCallback, TReturnValue>
	: IAdornments<TAdornments>
	where TAdornments : IAdornments<TAdornments, THandler, TCallback, TReturnValue>
	where THandler : Handler<TCallback, TReturnValue>
	where TCallback : Delegate
{
	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>The adornments instance.</returns>
	TAdornments Callback(TCallback callback);

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	/// <returns>The adornments instance.</returns>
	TAdornments ReturnValue(TReturnValue returnValue);
}

/// <summary>
/// Defines adornments for a mocked member
/// that has a callback.
/// </summary>
/// <typeparam name="TAdornments">The adornment.</typeparam>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
public class Adornments<TAdornments, THandler, TCallback>
	: IAdornments<TAdornments, THandler, TCallback>
	where TAdornments : Adornments<TAdornments, THandler, TCallback>
	where THandler : Handler<TCallback>
	where TCallback : Delegate
{
	/// <summary>
	/// The handler wrapped by this adornments instance.
	/// </summary>
	protected readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="Adornments{TAdornments, THandler, TCallback}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public Adornments(THandler handler) =>
		this.handler = handler;

	/// <inheritdoc/>
	public TAdornments AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return (TAdornments)this;
	}

	/// <inheritdoc/>
	public TAdornments ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return (TAdornments)this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>The adornments instance.</returns>
	public TAdornments Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return (TAdornments)this;
	}
}

/// <summary>
/// Defines adornments for a mocked member
/// that has a callback
/// and a return value.
/// </summary>
/// <typeparam name="TAdornments">The adornment.</typeparam>
/// <typeparam name="THandler">The handler type.</typeparam>
/// <typeparam name="TCallback">The callback delegate type.</typeparam>
/// <typeparam name="TReturnValue">The return value type.</typeparam>
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
public class Adornments<TAdornments, THandler, TCallback, TReturnValue>
	: IAdornments<TAdornments, THandler, TCallback, TReturnValue>
	where TAdornments : Adornments<TAdornments, THandler, TCallback, TReturnValue>
	where THandler : Handler<TCallback, TReturnValue>
	where TCallback : Delegate
{
	/// <summary>
	/// The handler wrapped by this adornments instance.
	/// </summary>
	protected readonly THandler handler;

	/// <summary>
	/// Creates a new <see cref="Adornments{THandler, TCallback, TReturnValue}"/> instance.
	/// </summary>
	/// <param name="handler">The handler instance.</param>
	public Adornments(THandler handler) =>
		this.handler = handler;

	/// <inheritdoc/>
	public TAdornments AddRaiseEvent(RaiseEventInformation raiseEvent)
	{
		this.handler.AddRaiseEvent(raiseEvent);
		return (TAdornments)this;
	}

	/// <inheritdoc/>
	public TAdornments ExpectedCallCount(uint expectedCallCount)
	{
		this.handler.ExpectedCallCount = expectedCallCount;
		return (TAdornments)this;
	}

	/// <summary>
	/// Sets the handler with a callback and returns itself.
	/// </summary>
	/// <param name="callback">The callback.</param>
	/// <returns>The adornments instance.</returns>
	public TAdornments Callback(TCallback callback)
	{
		this.handler.Callback = callback;
		return (TAdornments)this;
	}

	/// <summary>
	/// Sets the handler with a return value and returns itself.
	/// </summary>
	/// <param name="returnValue">The return value.</param>
	/// <returns>The adornments instance.</returns>
	public TAdornments ReturnValue(TReturnValue returnValue)
	{
		this.handler.ReturnValue = returnValue;
		return (TAdornments)this;
	}
}