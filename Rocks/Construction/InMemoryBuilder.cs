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

		protected override MethodInformation GetMethodInformation(MethodInfo baseMethod)
		{
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var descriptionWithOverride = baseMethod.GetMethodDescription(this.Namespaces, true);
			var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();
			var delegateCast = !containsRefAndOrOutParameters ?
				baseMethod.GetDelegateCast() :
				(this.Handlers.ContainsKey(description) ?
					this.Handlers[description][0].Method.GetType().GetSafeName(baseMethod, this.Namespaces) : string.Empty);
			
			return new MethodInformation
			{
				ContainsRefAndOrOutParameters = containsRefAndOrOutParameters,
				DelegateCast = delegateCast,
				Description = description,
				DescriptionWithOverride = descriptionWithOverride
			};
		}

		protected override string GetDirectoryForFile()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
