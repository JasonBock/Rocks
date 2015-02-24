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
		protected ExpectationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}