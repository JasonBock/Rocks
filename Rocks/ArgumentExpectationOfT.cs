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


	public class A<T>
	{
		public bool Equals([AllowNull] T x, [AllowNull] T y) => 
			new Func<T, T, bool>(this.Compare)(x, y);

		public bool Compare(T a, T b) => true;
	}
}