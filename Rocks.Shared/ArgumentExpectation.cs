using System;

namespace Rocks
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public abstract class ArgumentExpectation
	{
		internal ArgumentExpectation() { }
   }
}
