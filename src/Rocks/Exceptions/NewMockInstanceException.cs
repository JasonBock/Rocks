using System.Runtime.Serialization;

namespace Rocks.Exceptions;

[Serializable]
public sealed class NewMockInstanceException
	: Exception
{
	public NewMockInstanceException() { }
	public NewMockInstanceException(string message) : base(message) { }
	public NewMockInstanceException(string message, Exception inner) : base(message, inner) { }
	private NewMockInstanceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{ }
}