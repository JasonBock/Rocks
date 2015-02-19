using System;
using System.Runtime.Serialization;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class RockValidationException 
		: Exception
	{
		public RockValidationException() { }
		public RockValidationException(string message) : base(message) { }
		public RockValidationException(string message, Exception inner) : base(message, inner) { }
		protected RockValidationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }
	}
}
