using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class VerificationException
		: Exception
	{
		public VerificationException()
		{
			this.Failures = new List<string>().AsReadOnly();
		}

		public VerificationException(string message)
			: base(message)
		{
			this.Failures = new List<string>().AsReadOnly();
		}

		public VerificationException(IReadOnlyList<string> failures)
		{
			this.Failures = failures;
		}

		public VerificationException(IReadOnlyList<string> failures, string message)
			: base(message)
		{
			this.Failures = failures;
		}

		public VerificationException(string message, Exception inner)
			: base(message, inner)
		{
			this.Failures = new List<string>().AsReadOnly();
		}

		public VerificationException(IReadOnlyList<string> failures, string message, Exception inner)
			: base(message, inner)
		{
			this.Failures = failures;
		}

		protected VerificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		public IReadOnlyList<string> Failures { get; private set; }
	}
}
