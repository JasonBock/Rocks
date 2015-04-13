using Rocks.Construction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	internal sealed class InMemoryRock<T>
		: RockCore<T>
		where T : class
	{
		private readonly Options options;

		internal InMemoryRock(Options options)
		{
			this.options = options;
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

			var rock = Activator.CreateInstance(rockType, arguments.ToArray(), null);
			this.Rocks.Add(rock as IMock);
			return rock as T;
		}

		private Type GetMockType(ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> readOnlyHandlers)
		{
			var tType = typeof(T);
			var rockType = default(Type);
			var key = new CacheKey(tType, this.options);

			lock (Rock.CacheLock)
			{
				if (Rock.Cache.ContainsKey(key))
				{
					rockType = Rock.Cache[key];
				}
				else
				{
					rockType = new InMemoryMaker(tType, readOnlyHandlers, this.Namespaces, this.options).Mock;

					if (!tType.ContainsRefAndOrOutParameters())
					{
						Rock.Cache.Add(key, rockType);
					}

					if (this.options.Serialization == SerializationOptions.Supported)
					{
						Rock.Binder.Assemblies.Add(rockType.Assembly);
					}
				}
			}

			return rockType;
		}
	}
}
