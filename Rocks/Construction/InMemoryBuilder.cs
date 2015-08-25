using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Rocks.Construction
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

		protected override GetGeneratedEventsResults GetGeneratedEvents()
		{
			return new EventsGenerator().Generate(this.BaseType, this.Namespaces,
				this.NameGenerator, this.InformationBuilder);
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
