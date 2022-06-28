using System.Runtime.Serialization;

namespace Rocks.Exceptions;

/// <summary>
/// Thrown when a mocked method has the <see cref="DoesNotReturnAttribute"/> on it.
/// </summary>
[Serializable]
public sealed class DoesNotReturnException
	: Exception
{
	public DoesNotReturnException() { }
	public DoesNotReturnException(string message) : base(message) { }
	public DoesNotReturnException(string message, Exception inner) : base(message, inner) { }
	private DoesNotReturnException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{ }
}