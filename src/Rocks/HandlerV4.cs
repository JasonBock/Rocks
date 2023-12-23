namespace Rocks;

/// <summary>
/// Specifies expectations on a member.
/// </summary>
public abstract class HandlerV4
{
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