using System;
using System.Diagnostics.CodeAnalysis;

namespace Rocks
{
	[Serializable]
	public abstract class ArgumentExpectation<T>
		: ArgumentExpectation
	{
		public abstract bool IsValid([AllowNull] T value);
	}
}