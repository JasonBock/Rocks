using System;
#if !NETCOREAPP1_1
using System.Runtime.Serialization;
#endif

namespace Rocks.Exceptions
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class PropertyNotFoundException
		: Exception
	{
		public PropertyNotFoundException() { }
		public PropertyNotFoundException(string message) : base(message) { }
		public PropertyNotFoundException(string message, Exception inner) : base(message, inner) { }
#if !NETCOREAPP1_1
		private PropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif
	}
}