using System;
#if !NETCOREAPP1_1
using System.Runtime.Serialization;
#endif
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Rocks.Exceptions
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class CompilationException
		: Exception
	{
		public CompilationException() { }
		public CompilationException(string message) : base(message) { }
		public CompilationException(string message, Exception inner) : base(message, inner) { }

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

#if !NETCOREAPP1_1
		private CompilationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif

		public ImmutableArray<Diagnostic> Diagnostics { get; }
	}
}