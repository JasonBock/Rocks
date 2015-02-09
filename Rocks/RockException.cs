using System;
using System.Runtime.Serialization;

namespace Rocks
{
	[Serializable]
	public sealed class RockException 
		: Exception
	{
		public RockException() { }
		public RockException(string message) : base(message) { }
		public RockException(string message, Exception inner) : base(message, inner) { }
		protected RockException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }
	}
}
