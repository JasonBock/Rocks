using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	public static class Rock
	{
		public static IRock<T> Create<T>()
			where T : class
		{
			return Rock.Create<T>(new Options());
		}

		public static IRock<T> Create<T>(Options options)
			where T : class
		{
			var message = typeof(T).Validate(options.Serialization);

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new ValidationException(message);
			}

			return Rock.NewRock<T>(options);
		}

		private static IRock<T> NewRock<T>(Options options)
			where T : class
		{
			var tType = typeof(T);
			// Can assume only sealed typed with the right constructor passed the .Validate() test.
			return tType.IsSealed ? new AssemblyRock<T>(options) as IRock<T> : 
				new InMemoryRock<T>(options) as IRock<T>;
		}

		public static CreateResult<T> TryCreate<T>()
			where T : class
		{
			return Rock.TryCreate<T>(new Options());
		}

		public static CreateResult<T> TryCreate<T>(Options options)
			where T : class
		{
			var result = default(IRock<T>);
			var isSuccessful = false;

			var message = typeof(T).Validate(options.Serialization);

			if (string.IsNullOrWhiteSpace(message))
			{
				result = Rock.NewRock<T>(options);
				isSuccessful = true;
			}

			return new CreateResult<T>(isSuccessful, result);
		}


		internal static AssemblyBinder Binder { get; } = new AssemblyBinder();
      internal static Dictionary<CacheKey, Type> Cache { get; } = new Dictionary<CacheKey, Type>();
      internal static object CacheLock { get; } = new object();
   }
}