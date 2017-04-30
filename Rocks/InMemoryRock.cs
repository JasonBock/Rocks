using Rocks.Construction;
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

		internal InMemoryRock(RockOptions options, bool isMake)
		{
			this.options = options;
			this.isMake = isMake;
		}

		public override T Make()
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();
			var rockType = this.GetMockType(readOnlyHandlers);

			var rock = Activator.CreateInstance(rockType, readOnlyHandlers);
			this.Rocks.Add(rock as IMock);
			return rock as T;
		}

		public override T Make(object[] constructorArguments)
		{
			var readOnlyHandlers = this.CreateReadOnlyHandlerDictionary();
			var rockType = this.GetMockType(readOnlyHandlers);

			var arguments = new List<object> { readOnlyHandlers };
			arguments.AddRange(constructorArguments);

#if !NETCOREAPP1_1
			var rock = Activator.CreateInstance(rockType, arguments.ToArray(), null);
#else
			var rock = Activator.CreateInstance(rockType, arguments.ToArray());
#endif
			this.Rocks.Add(rock as IMock);
			return rock as T;
		}

		private Type GetMockType(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> readOnlyHandlers)
		{
			var tType = typeof(T);
			var rockType = default(Type);
			var key = new CacheKey(tType, this.options);

			if(this.options.Caching == CachingOptions.UseCache)
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
#if !NETCOREAPP1_1
						if (this.options.Serialization == SerializationOptions.Supported)
						{
							Rock.Binder.Assemblies.Add(rockType.Assembly);
						}
#endif
					}
				}
			}
			else
			{
				rockType = new InMemoryMaker(tType, readOnlyHandlers, this.Namespaces, 
					this.options, this.isMake).Mock;

#if !NETCOREAPP1_1
				if (this.options.Serialization == SerializationOptions.Supported)
				{
					Rock.Binder.Assemblies.Add(rockType.Assembly);
				}
#endif
			}

			return rockType;
		}
	}
}
