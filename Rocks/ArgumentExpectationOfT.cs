using System;

namespace Rocks
{
	[Serializable]
   public abstract class ArgumentExpectation<T>
		: ArgumentExpectation
   {
	  public abstract bool IsValid(T value);
   }
}
