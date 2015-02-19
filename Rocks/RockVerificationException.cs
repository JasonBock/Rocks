using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace Rocks
{
	[Serializable]
	public sealed class RockCompilationException 
		: Exception
	{
		public RockCompilationException(ImmutableArray<Diagnostic> diagnostics)
		{
			this.Diagnostics = diagnostics;
		}

		public RockCompilationException(ImmutableArray<Diagnostic> diagnostics, string message) 
			: base(message)
		{
			this.Diagnostics = diagnostics;
		}

		public RockCompilationException(ImmutableArray<Diagnostic> diagnostics, string message, Exception inner) 
			: base(message, inner)
		{
			this.Diagnostics = diagnostics;
		}

		protected RockCompilationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) { }

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	}
}
