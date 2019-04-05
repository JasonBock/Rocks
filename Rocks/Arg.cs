using System;

namespace Rocks
{
#nullable disable
   public static class Arg
	{
		public static T Is<T>(Func<T, bool> evaluation)
		{
			if (evaluation is null)
			{
				throw new ArgumentNullException(nameof(evaluation));
			}

			return default;
		}

		public static T IsAny<T>() => default;

		public static T IsDefault<T>() => default;
	}
#nullable enable
}
