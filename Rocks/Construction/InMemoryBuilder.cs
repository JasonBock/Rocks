using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal sealed class InMemoryBuilder
		: Builder
	{
		internal InMemoryBuilder(Type baseType,
			ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options)
			: base(baseType, handlers, namespaces, options)
		{
			this.TypeName = $"Rock{Guid.NewGuid().ToString("N")}";
		}

		protected override MethodDescription GetMethodDescription(MethodInfo baseMethod)
		{
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();
			var delegateCast = !containsRefAndOrOutParameters ?
				baseMethod.GetDelegateCast() :
				(this.Handlers.ContainsKey(description) ?
					this.Handlers[description][0].Method.GetType().GetSafeName(baseMethod, this.Namespaces) : string.Empty);

			return new MethodDescription
			{
				ContainsRefAndOrOutParameters = containsRefAndOrOutParameters,
				DelegateCast = delegateCast,
				Description = description
			};
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
