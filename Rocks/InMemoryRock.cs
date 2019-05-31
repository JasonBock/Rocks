using Rocks.Construction.InMemory;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	internal sealed class InMemoryRock<T>
		: RockCore<T>
		where T : class
	{
		private readonly RockOptions options;
		private readonly bool isMake;

		internal InMemoryRock(RockOptions options, bool isMake) =>
			(this.options, this.isMake) = (options, isMake);

		public override T Make()
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();
			var rockType = this.GetMockType(readOnlyHandlers);

			var rock = Activator.CreateInstance(rockType, readOnlyHandlers);
			this.Rocks.Add((IMock)rock);
			return (T)rock;
		}

		public override T Make(object[] constructorArguments)
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();
			var rockType = this.GetMockType(readOnlyHandlers);

			var arguments = new List<object> { readOnlyHandlers };
			arguments.AddRange(constructorArguments);

			var rock = Activator.CreateInstance(rockType, arguments.ToArray(), null);
			this.Rocks.Add((IMock)rock);
			return (T)rock;
		}

		private Type GetMockType(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> readOnlyHandlers)
		{
			var tType = typeof(T);
			var rockType = default(Type);
			var key = new CacheKey(tType, this.options);

			if (this.options.Caching == CachingOptions.UseCache)
			{
				lock (Rock.CacheLock)
				{
					var cache = this.isMake ? Rock.MakeCache : Rock.Cache;

					if (cache.ContainsKey(key))
					{
						rockType = cache[key];
					}
					else
					{
						rockType = new InMemoryMaker(tType, readOnlyHandlers, this.Namespaces,
							this.options, this.isMake).Mock;

						if (this.isMake || !tType.ContainsRefAndOrOutParameters())
						{
							cache.Add(key, rockType);
						}

						if (this.options.Serialization == SerializationOptions.Supported)
						{
							Rock.Binder.Assemblies.Add(rockType.Assembly);
						}
					}
				}
			}
			else
			{
				rockType = new InMemoryMaker(tType, readOnlyHandlers, this.Namespaces,
					this.options, this.isMake).Mock;

				if (this.options.Serialization == SerializationOptions.Supported)
				{
					Rock.Binder.Assemblies.Add(rockType.Assembly);
				}
			}

			return rockType;
		}
	}
}