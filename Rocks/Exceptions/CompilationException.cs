using System;
using System.Runtime.Serialization;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Rocks.Exceptions
{
	[Serializable]
	public sealed class CompilationException
		: Exception
	{
		public CompilationException(ImmutableArray<Diagnostic> diagnostics)
		{
			this.Diagnostics = diagnostics;
		}

		public CompilationException(ImmutableArray<Diagnostic> diagnostics, string message)
			: base(message)
		{
			this.Diagnostics = diagnostics;
		}

		public CompilationException(ImmutableArray<Diagnostic> diagnostics, string message, Exception inner)
			: base(message, inner)
		{
			this.Diagnostics = diagnostics;
		}

		protected CompilationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	}
}