using System;

namespace Rocks
{
	public static class Arg
	{
		public static T Is<T>(Func<T, bool> evaluation) { return default(T); }
		public static T IsAny<T>() { return default(T); }
	}
}
