using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class InMemoryTypeNameGenerator
		: TypeNameGenerator
	{
		internal InMemoryTypeNameGenerator(SortedSet<string> namespaces)
			: base()
		{
			this.Namespaces = namespaces;
		}

		internal override string Generate(Type baseType)
		{
			var name = baseType.IsGenericTypeDefinition ?
				$"{Guid.NewGuid().ToString("N")}{baseType.GetGenericArguments(this.Namespaces).Arguments}" : Guid.NewGuid().ToString("N");
			return $"Rock{name}";
		}

		private SortedSet<string> Namespaces { get; set; }
	}
}
