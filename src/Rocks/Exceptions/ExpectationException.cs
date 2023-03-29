namespace Rocks.Exceptions;

/// <summary>
/// Thrown when an expectation has not been met in a mock object.
/// </summary>
[Serializable]
public sealed class ExpectationException
	 : Exception
{
	/// <summary>
	/// Creates a new <see cref="ExpectationException"/> instance
	/// with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public ExpectationException(string message)
		: base(message) { }
}