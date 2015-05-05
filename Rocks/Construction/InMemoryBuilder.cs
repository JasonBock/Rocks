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
		: Builder
	{
		internal InMemoryBuilder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options, new InMemoryNameGenerator())
		{
			var name = this.BaseType.IsGenericTypeDefinition ?
				$"{Guid.NewGuid().ToString("N")}{this.BaseType.GetGenericArguments(this.Namespaces).Arguments}" : Guid.NewGuid().ToString("N");
			this.TypeName = $"Rock{name}";
		}

		protected override string GetDelegateCast(MethodInfo baseMethod)
		{
			var key = baseMethod.MetadataToken;

			if (this.Handlers.ContainsKey(key))
			{
				var delegateType = this.Handlers[key][0].Method.GetType();

				if(baseMethod.IsGenericMethodDefinition)
				{
					delegateType = delegateType.GetGenericTypeDefinition();
				}

				return $"{delegateType.GetFullName(this.Namespaces)}";
         }
			else
			{
				return string.Empty;
			}
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
