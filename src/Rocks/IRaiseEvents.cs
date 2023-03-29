namespace Rocks;

/// <summary>
/// Defines the interface to raise an event
/// on a mock.
/// </summary>
public interface IRaiseEvents
{
	/// <summary>
	/// Raises the specified event.
	/// </summary>
	/// <param name="eventName">The name of the event.</param>
	/// <param name="args">The arguments for the event.</param>
	void Raise(string eventName, EventArgs args);
}