using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryTypeNameGenerator
		: TypeNameGenerator
	{
		internal InMemoryTypeNameGenerator(SortedSet<string> namespaces)
			: base() => this.Namespaces = namespaces;

		internal override string Generate(Type baseType) => baseType.IsGenericTypeDefinition ?
			$"Rock{baseType.MetadataToken}{baseType.GetGenericArguments(this.Namespaces).arguments}" :
			$"Rock{baseType.MetadataToken}";

		private SortedSet<string> Namespaces { get; }
	}
}