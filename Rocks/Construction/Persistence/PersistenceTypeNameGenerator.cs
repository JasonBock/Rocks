using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.Persistence
{
	internal sealed class PersistenceTypeNameGenerator
		: TypeNameGenerator
	{
		internal PersistenceTypeNameGenerator(SortedSet<string> namespaces)
			: base()
		{
			this.Namespaces = namespaces;
		}

		internal override string Generate(Type baseType)
		{
			var name = baseType.GetTypeInfo().IsGenericTypeDefinition ?
				$"{baseType.GetFullName(this.Namespaces)}" : baseType.GetSafeName();
			return $"Rock{name.Replace(".", string.Empty)}";
		}

		private SortedSet<string> Namespaces { get; set; }
	}
}
