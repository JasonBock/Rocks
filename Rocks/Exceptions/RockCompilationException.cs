using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class RockVerificationException 
		: Exception
	{
		public RockVerificationException()
		{
			this.Failures = new List<string>().AsReadOnly();
		}

		public RockVerificationException(string message)
			: base(message)
		{
			this.Failures = new List<string>().AsReadOnly();
		}

		public RockVerificationException(IReadOnlyList<string> failures)
		{
			this.Failures = failures;
		}

		public RockVerificationException(IReadOnlyList<string> failures, string message) 
			: base(message)
		{
			this.Failures = failures;
		}

		public RockVerificationException(string message, Exception inner) 
			: base(message, inner)
		{
			this.Failures = new List<string>().AsReadOnly();
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