namespace Rocks.Exceptions;

/// <summary>
/// Thrown if an <c>Instance()</c> extension method is called
/// more than once on an <see cref="Expectations.Expectations{T}"/> instance.
/// </summary>
[Serializable]
public sealed class NewMockInstanceException
	: Exception
{
	/// <summary>
	/// Creates a new <see cref="NewMockInstanceException"/> instance
	/// with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public NewMockInstanceException(string message)
		: base(message) { }
}