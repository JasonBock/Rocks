using System;
using System.Diagnostics.CodeAnalysis;

namespace Rocks
{
   [Serializable]
   public sealed class ArgumentIsAnyExpectation<T>
	   : ArgumentExpectation<T>
   {
		internal ArgumentIsAnyExpectation() { }

		public override bool IsValid([AllowNull] T value) => true;
   }
}