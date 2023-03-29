using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Rocks;

/// <summary>
/// Specifies expectations on a member.
/// </summary>
[Serializable]
public class HandlerInformation
{
	private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

	private int callCount;

	[NonSerialized]
	private readonly List<RaiseEventInformation> raiseEvents = new();

	/// <summary>
	/// Creates a new <see cref="HandlerInformation"/> instance.
	/// </summary>
	public HandlerInformation()
		: this(null, ImmutableArray<Argument>.Empty)
	{ }

	/// <summary>
	/// Creates a new <see cref="HandlerInformation"/> instance
	/// with a callback method.
	/// </summary>
	/// <param name="method">The callback method.</param>
	public HandlerInformation(Delegate method)
		: this(method, ImmutableArray<Argument>.Empty)
	{ }

	/// <summary>
	/// Creates a new <see cref="HandlerInformation"/> instance
	/// with a set of argument expectations.
	/// </summary>
	/// <param name="expectations">The set of argument expectations.</param>
	public HandlerInformation(ImmutableArray<Argument> expectations)
		: this(null, expectations)
	{ }

	/// <summary>
	/// Creates a new <see cref="HandlerInformation"/> instance
	/// with a callback method and
	/// a set of argument expectations.
	/// </summary>
	/// <param name="method">The callback method.</param>
	/// <param name="expectations">The set of argument expectations.</param>
	public HandlerInformation(Delegate? method, ImmutableArray<Argument> expectations) =>
		(this.Method, this.Expectations) = (method, expectations);

	/// <summary>
	/// Adds an expectation to raise an event.
	/// </summary>
	/// <param name="raiseEvent">The event to raise.</param>
	public void AddRaiseEvent(RaiseEventInformation raiseEvent) => this.raiseEvents.Add(raiseEvent);

	/// <summary>
	/// Increments the call count.
	/// </summary>
	public void IncrementCallCount() => Interlocked.Increment(ref this.callCount);

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
	/// Sets the callback method.
	/// </summary>
	/// <typeparam name="TCallback">The callback method type.</typeparam>
	/// <param name="callback">The callback method.</param>
	public void SetCallback<TCallback>(TCallback callback)
		where TCallback : Delegate =>
			this.Method = callback;

	/// <summary>
	/// Sets the expected call count.
	/// </summary>
	/// <param name="expectedCallCount">The expected call count.</param>
	public void SetExpectedCallCount(uint expectedCallCount) => this.ExpectedCallCount = expectedCallCount;

	internal IReadOnlyList<string> Verify()
	{
		var verifications = new List<string>();

		if (this.ExpectedCallCount != this.callCount)
		{
			verifications.Add(string.Format(
				CultureInfo.CurrentCulture, HandlerInformation.ErrorExpectedCallCount,
				this.ExpectedCallCount.ToString(CultureInfo.CurrentCulture), this.callCount.ToString(CultureInfo.CurrentCulture)));
		}

		return verifications.AsReadOnly();
	}

	internal int CallCount => this.callCount;

	/// <summary>
	/// Gets the set of argument expectations.
	/// </summary>
	public ImmutableArray<Argument> Expectations { get; }
	internal uint ExpectedCallCount { get; private set; } = 1;
	/// <summary>
	/// Gets the callback method.
	/// </summary>
	public Delegate? Method { get; private set; }
	internal ReadOnlyCollection<RaiseEventInformation> GetRaiseEvents() => this.raiseEvents.AsReadOnly();
}