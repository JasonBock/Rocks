using System;
using System.Collections.Generic;
using System.Globalization;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryTypeNameGenerator
		: TypeNameGenerator
	{
		internal InMemoryTypeNameGenerator(SortedSet<string> namespaces)
			: base() => this.Namespaces = namespaces;

		internal override string Generate(Type baseType) => baseType.IsGenericTypeDefinition ?
			$"Rock{baseType.MetadataToken.ToString(CultureInfo.CurrentCulture)}{baseType.GetGenericArguments(this.Namespaces).arguments}" :
			$"Rock{baseType.MetadataToken.ToString(CultureInfo.CurrentCulture)}";

		private SortedSet<string> Namespaces { get; }
	}
}