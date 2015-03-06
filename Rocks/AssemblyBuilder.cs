using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Rocks
{
	internal sealed class AssemblyBuilder
		: Builder
	{
		internal AssemblyBuilder(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, bool shouldCreateCodeFile)
			: base(baseType, handlers, namespaces, shouldCreateCodeFile)
		{
			this.TypeName = $"Rock{baseType.Name}";
		}

		protected override string GetDirectoryForFile()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), this.BaseType.Namespace.Replace(".", "\\"));
		}
	}
}
