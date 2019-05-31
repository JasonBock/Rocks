using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryBuilder
		: Builder<InMemoryMethodInformationBuilder>
	{
		internal InMemoryBuilder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, RockOptions options, bool isMake)
			: base(baseType, handlers, namespaces, options, new InMemoryNameGenerator(),
				new InMemoryMethodInformationBuilder(namespaces, handlers),
				new InMemoryTypeNameGenerator(namespaces), isMake)
		{ }

		protected override string GetDirectoryForFile() => this.Options.CodeFileDirectory;
	}
}