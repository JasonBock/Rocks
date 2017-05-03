using System;
using System.Reflection;

namespace Rocks.Construction.Persistence
{
	internal sealed class PersistenceNameGenerator
		: NameGenerator
	{
		internal PersistenceNameGenerator(Type type)
			: this(type.GetTypeInfo().Assembly)
		{ }

		internal PersistenceNameGenerator(Assembly assembly) =>
			this.AssemblyName = $"{assembly.GetName().Name}.Rocks";
	}
}
