using Rocks.Construction;
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
			var tType = typeof(T);
			var message = tType.Validate(options.Serialization,
				tType.IsSealed ? new AssemblyNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

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
			var tType = typeof(T);

			var message = tType.Validate(options.Serialization,
				tType.IsSealed ? new AssemblyNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (string.IsNullOrWhiteSpace(message))
			{
				result = Rock.NewRock<T>(options);
				isSuccessful = true;
			}

			return new CreateResult<T>(isSuccessful, result);
		}

		public static T Make<T>()
			where T : class
		{
			return Rock.Make<T>(new Options());
		}

		private static Options MapForMake(Options options)
		{
			return new Options(options.Level, options.CodeFile, 
				options.Serialization, CachingOptions.GenerateNewVersion);
		}

		public static T Make<T>(Options options)
			where T : class
		{
			var mappedOptions = Rock.MapForMake(options);
			var tType = typeof(T);
			var message = tType.Validate(mappedOptions.Serialization,
				tType.IsSealed ? new AssemblyNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new ValidationException(message);
			}

			return Rock.NewRock<T>(mappedOptions).Make();
		}

		public static MakeResult<T> TryMake<T>()
			where T : class
		{
			return Rock.TryMake<T>(new Options());
		}

		public static MakeResult<T> TryMake<T>(Options options)
			where T : class
		{
			var mappedOptions = Rock.MapForMake(options);
			var result = default(T);
			var isSuccessful = false;

			var tType = typeof(T);
			var message = tType.Validate(mappedOptions.Serialization,
				tType.IsSealed ? new AssemblyNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (string.IsNullOrWhiteSpace(message))
			{
				result = Rock.NewRock<T>(mappedOptions).Make();
				isSuccessful = true;
			}

			return new MakeResult<T>(isSuccessful, result);
		}

		internal static AssemblyBinder Binder { get; } = new AssemblyBinder();
      internal static Dictionary<CacheKey, Type> Cache { get; } = new Dictionary<CacheKey, Type>();
      internal static object CacheLock { get; } = new object();
   }
}