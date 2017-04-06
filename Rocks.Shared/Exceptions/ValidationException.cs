using System;
#if !NETCOREAPP1_1
using System.Runtime.Serialization;
#endif

namespace Rocks.Exceptions
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class ValidationException 
		: Exception
	{
		public ValidationException() { }
		public ValidationException(string message) : base(message) { }
		public ValidationException(string message, Exception inner) : base(message, inner) { }
#if !NETCOREAPP1_1
		private ValidationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }
#endif
	}
}
