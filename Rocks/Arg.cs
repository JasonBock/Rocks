using System;

namespace Rocks
{
	public static class Arg
	{
		public static T Is<T>(Func<T, bool> evaluation)
		{
			if (evaluation == null)
			{
				throw new ArgumentNullException(nameof(evaluation));
			}

			return default;
		}

		public static T IsAny<T>() => default;

		public static T IsDefault<T>() => default;
	}
}
