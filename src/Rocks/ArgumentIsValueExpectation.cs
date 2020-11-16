using System;
using System.Diagnostics.CodeAnalysis;

namespace Rocks
{
   [Serializable]
   public sealed class ArgumentIsValueExpectation<T>
	   : ArgumentExpectation<T>
   {
		internal ArgumentIsValueExpectation(T value) => this.Value = value;

		public override bool IsValid([AllowNull] T value) => ObjectEquality.AreEqual(this.Value, value);

		internal T Value { get; }
   }
}
