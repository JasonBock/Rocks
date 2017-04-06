using System;
#if !NETCOREAPP1_1
using System.Runtime.Serialization;
#endif

namespace Rocks.Exceptions
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class ExpectationException
		: Exception
	{
		public ExpectationException() { }
		public ExpectationException(string message) : base(message) { }
		public ExpectationException(string message, Exception inner) : base(message, inner) { }
#if !NETCOREAPP1_1
		private ExpectationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif
	}
}