using System;
using System.Runtime.Serialization;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class ExpectationException
		: Exception
	{
		public ExpectationException() { }
		public ExpectationException(string message) : base(message) { }
		public ExpectationException(string message, Exception inner) : base(message, inner) { }
		private ExpectationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}