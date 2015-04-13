using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class AssemblyBuilder
		: Builder
	{
		internal AssemblyBuilder(Type baseType,
			ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options)
		{
			this.TypeName = $"Rock{baseType.GetSafeName()}";
		}

		protected override string GetDirectoryForFile()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), this.BaseType.Namespace.Replace(".", "\\"));
		}
	}
}
