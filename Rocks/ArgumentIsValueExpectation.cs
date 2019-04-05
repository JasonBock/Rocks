using System;

namespace Rocks
{
   [Serializable]
   public sealed class ArgumentIsValueExpectation<T>
	   : ArgumentExpectation
   {
		internal ArgumentIsValueExpectation(T value) => this.Value = value;

		public bool IsValid(T value) => ObjectEquality.AreEqual(this.Value, value);

		internal T Value { get; }
   }
}
