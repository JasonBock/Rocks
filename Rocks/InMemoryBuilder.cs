using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Rocks
{
	internal sealed class InMemoryBuilder
		: Builder
	{
		internal InMemoryBuilder(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, bool shouldCreateCodeFile)
			: base(baseType, handlers, namespaces, shouldCreateCodeFile)
		{
			this.TypeName = $"Rock{Guid.NewGuid().ToString("N")}";
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
      }
	}
}
