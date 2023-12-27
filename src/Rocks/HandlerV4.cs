﻿using Rocks.Builders.Create;
using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// Specifies expectations on a member.
/// </summary>
[Serializable]
public abstract class HandlerV4
{
	[NonSerialized]
	private readonly List<RaiseEventInformation> raiseEvents = [];

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public void AddRaiseEvent(RaiseEventInformation raiseEvent) => 
		this.raiseEvents.Add(raiseEvent);

	/// <summary>
	/// Raises events on a given mock object.
	/// </summary>
	/// <param name="target">The mock object to raise events on.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is <c>null</c>.</exception>
	public void RaiseEvents(IRaiseEvents target)
	{
		if (target is null) { throw new ArgumentNullException(nameof(target)); }

		foreach (var raiseEvent in this.raiseEvents)
		{
			target.Raise(raiseEvent.FieldName, raiseEvent.Args);
		}
	}

	/// <summary>
	/// Gets or set the current call count.
	/// </summary>
	public uint CallCount { get; set; }
	/// <summary>
	/// Gets or set the expected call count.
	/// </summary>
	public uint ExpectedCallCount { get; set; }
}

/// <summary>
/// Specifies expectations on a member
/// with a callback.
/// </summary>
public abstract class HandlerV4<TCallback>
	: HandlerV4
	where TCallback : Delegate
{
	/// <summary>
	/// Gets or sets the callback.
	/// </summary>
	public TCallback? Callback { get; set; }
}

/// <summary>
/// Specifies expectations on a member
/// with a callback and a return value.
/// </summary>
public abstract class HandlerV4<TCallback, TReturnValue>
	: HandlerV4<TCallback>
	where TCallback : Delegate
{
	/// <summary>
	/// Gets or sets the return value.
	/// </summary>
	public TReturnValue? ReturnValue { get; set; }
}

// We need a VNC for Handler-based usage,
// so the factory method produces one for everyone.
internal static class HandlerVariableNamingContextV4
{
	internal static VariableNamingContextV4 Create() =>
		new (new[] { "CallCount", "ExpectedCallCount", "Callback", "ReturnValue" }.ToImmutableArray());
}