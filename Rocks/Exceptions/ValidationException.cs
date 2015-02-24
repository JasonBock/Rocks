using System;
using System.Runtime.Serialization;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class ValidationException 
		: Exception
	{
		public ValidationException() { }
		public ValidationException(string message) : base(message) { }
		public ValidationException(string message, Exception inner) : base(message, inner) { }
		protected ValidationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }
	}
}
