using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class InMemoryBuilder
		: Builder<InMemoryMethodInformationBuilder>
	{
		internal InMemoryBuilder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options, new InMemoryNameGenerator(), 
				  new EventsBuilder(baseType, namespaces, new InMemoryNameGenerator(), new InMemoryMethodInformationBuilder(namespaces, handlers)),
				  new InMemoryMethodInformationBuilder(namespaces, handlers))
		{
			var name = this.BaseType.IsGenericTypeDefinition ?
				$"{Guid.NewGuid().ToString("N")}{this.BaseType.GetGenericArguments(this.Namespaces).Arguments}" : Guid.NewGuid().ToString("N");
			this.TypeName = $"Rock{name}";
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
