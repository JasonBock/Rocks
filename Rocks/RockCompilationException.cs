using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rocks
{
	[Serializable]
	public sealed class RockVerificationException 
		: Exception
	{
		public RockVerificationException(IReadOnlyList<string> failures)
		{
			this.Failures = failures;
		}

		public RockVerificationException(IReadOnlyList<string> failures, string message) 
			: base(message)
		{
			this.Failures = failures;
		}

		public RockVerificationException(IReadOnlyList<string> failures, string message, Exception inner) 
			: base(message, inner)
		{
			this.Failures = failures;
		}

		protected RockVerificationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }

		public IReadOnlyList<string> Failures { get; private set; }
	}
}
