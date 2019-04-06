using System;

namespace Rocks
{
   [Serializable]
   public sealed class ArgumentIsAnyExpectation<T>
	   : ArgumentExpectation<T>
   {
		internal ArgumentIsAnyExpectation() { }

		public override bool IsValid(T value) => true;
   }
}
