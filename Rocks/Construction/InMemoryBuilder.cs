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
			: base(baseType, handlers, namespaces, options)
		{
			var name = this.BaseType.IsGenericTypeDefinition ?
				$"{Guid.NewGuid().ToString("N")}{this.BaseType.GetGenericArguments(this.Namespaces).Arguments}" : Guid.NewGuid().ToString("N");
			this.TypeName = $"Rock{name}";
		}

		protected override MethodInformation GetMethodInformation(MethodInfo baseMethod)
		{
			var description = baseMethod.GetMethodDescription(this.Namespaces);
			var descriptionWithOverride = baseMethod.GetMethodDescription(this.Namespaces, true);
			var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();

			var key = baseMethod.MetadataToken;
			var delegateCast = !containsRefAndOrOutParameters ?
				baseMethod.GetDelegateCast() :
				(this.Handlers.ContainsKey(key) ?
					this.Handlers[key][0].Method.GetType().GetSafeName(baseMethod, this.Namespaces) : string.Empty);
			
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
