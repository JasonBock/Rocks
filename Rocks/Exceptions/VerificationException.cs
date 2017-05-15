using System;
#if !NETCOREAPP1_1
using System.Runtime.Serialization;
#endif
using System.Collections.Generic;

namespace Rocks.Exceptions
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class VerificationException
		: Exception
	{
		public VerificationException() =>
			this.Failures = new List<string>().AsReadOnly();

		public VerificationException(string message)
			: base(message) =>
			this.Failures = new List<string>().AsReadOnly();

		public VerificationException(IReadOnlyList<string> failures) =>
			this.Failures = failures;

		public VerificationException(IReadOnlyList<string> failures, string message)
			: base(message) =>
			this.Failures = failures;

		public VerificationException(string message, Exception inner)
			: base(message, inner) =>
			this.Failures = new List<string>().AsReadOnly();

		public VerificationException(IReadOnlyList<string> failures, string message, Exception inner)
			: base(message, inner) =>
			this.Failures = failures;

#if !NETCOREAPP1_1
		private VerificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif

		public IReadOnlyList<string> Failures { get; }
	}
}
