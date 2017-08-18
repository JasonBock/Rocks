using Rocks.Construction;
using Rocks.Construction.InMemory;
using Rocks.Construction.Persistence;
using Rocks.Exceptions;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	public static class Rock
	{
		public static IRock<T> Create<T>()
			where T : class => Rock.Create<T>(new RockOptions());

		public static IRock<T> Create<T>(RockOptions options)
			where T : class
		{
			var tType = typeof(T);
			var message = tType.Validate(options.Serialization,
				tType.IsSealed ? new PersistenceNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new ValidationException(message);
			}

			return Rock.NewRock<T>(options, false);
		}

		private static IRock<T> NewRock<T>(RockOptions options, bool isMake)
			where T : class
		{
			var tType = typeof(T);
			// Can assume only sealed typed with the right constructor passed the .Validate() test.
			return tType.IsSealed ? new AssemblyRock<T>(options) as IRock<T> :
				new InMemoryRock<T>(options, isMake) as IRock<T>;
		}

		public static CreateResult<T> TryCreate<T>()
			where T : class => Rock.TryCreate<T>(new RockOptions());

		public static CreateResult<T> TryCreate<T>(RockOptions options)
			where T : class
		{
			var result = default(IRock<T>);
			var isSuccessful = false;
			var tType = typeof(T);

			var message = tType.Validate(options.Serialization,
				tType.IsSealed ? new PersistenceNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (string.IsNullOrWhiteSpace(message))
			{
				result = Rock.NewRock<T>(options, false);
				isSuccessful = true;
			}

			return new CreateResult<T>(isSuccessful, result);
		}

		public static T Make<T>()
			where T : class => Rock.Make<T>(new RockOptions());

		/// <summary>
		/// Ensure that makes are always cached no matter what
		/// the given options are.
		/// </summary>
		/// <param name="options">The requested <seealso cref="RockOptions"/> value from the user.</param>
		/// <returns>A mapped <seealso cref="RockOptions"/> with caching enabled.</returns>
		private static RockOptions MapForMake(RockOptions options) =>
			new RockOptions(
				level: options.Optimization, 
				codeFile: options.CodeFile, 
				serialization: options.Serialization,
				caching: CachingOptions.UseCache, 
				allowWarnings: options.AllowWarnings,
				codeFileDirectory: options.CodeFileDirectory);

		public static T Make<T>(RockOptions options)
			where T : class
		{
			var mappedOptions = Rock.MapForMake(options);
			var tType = typeof(T);
			var message = tType.Validate(mappedOptions.Serialization,
				tType.IsSealed ? new PersistenceNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new ValidationException(message);
			}

			return Rock.NewRock<T>(mappedOptions, true).Make();
		}

		public static MakeResult<T> TryMake<T>()
			where T : class => Rock.TryMake<T>(new RockOptions());

		public static MakeResult<T> TryMake<T>(RockOptions options)
			where T : class
		{
			var mappedOptions = Rock.MapForMake(options);
			var result = default(T);
			var isSuccessful = false;

			var tType = typeof(T);
			var message = tType.Validate(mappedOptions.Serialization,
				tType.IsSealed ? new PersistenceNameGenerator(tType) as NameGenerator :
					new InMemoryNameGenerator() as NameGenerator);

			if (string.IsNullOrWhiteSpace(message))
			{
				result = Rock.NewRock<T>(mappedOptions, true).Make();
				isSuccessful = true;
			}

			return new MakeResult<T>(isSuccessful, result);
		}

		internal static AssemblyBinder Binder { get; } = new AssemblyBinder();
		internal static Dictionary<CacheKey, Type> Cache { get; } = new Dictionary<CacheKey, Type>();
		internal static object CacheLock { get; } = new object();
		internal static Dictionary<CacheKey, Type> MakeCache { get; } = new Dictionary<CacheKey, Type>();
	}
}