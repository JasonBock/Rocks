using System;
using System.Reflection;

namespace Rocks.Construction
{
	internal sealed class AssemblyNameGenerator
		: NameGenerator
	{
		internal AssemblyNameGenerator(Type type)
			: this(type.Assembly)
		{ }

		internal AssemblyNameGenerator(Assembly assembly)
		{
			this.AssemblyName = $"{assembly.GetName().Name}.Rocks";
		}
	}
}
