namespace Rocks.Exceptions;

/// <summary>
/// Throws when a return value could not be obtained
/// in a mock for a member that needs to return a value.
/// </summary>
[Serializable]
public sealed class MockException
	: Exception
{
	/// <summary>
	/// Creates a new <see cref="MockException"/> instance
	/// with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public MockException(string message)
		: base(message) { }
}