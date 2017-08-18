using System;
using System.Runtime.Serialization;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class PropertyNotFoundException
		: Exception
	{
		public PropertyNotFoundException() { }
		public PropertyNotFoundException(string message) : base(message) { }
		public PropertyNotFoundException(string message, Exception inner) : base(message, inner) { }
		private PropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}