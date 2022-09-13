using System.Runtime.Serialization;

namespace Rocks.Exceptions;

[Serializable]
public sealed class MockException
	: Exception
{
	public MockException() { }
	public MockException(string message) : base(message) { }
	public MockException(string message, Exception inner) : base(message, inner) { }
	private MockException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{ }
}