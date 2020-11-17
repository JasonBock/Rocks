using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class IPropertySymbolExtensions
	{
		internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IPropertySymbol self)
		{
			static IEnumerable<INamespaceSymbol> GetParameterNamespaces(IParameterSymbol parameter)
			{
				foreach(var parameterTypeNamespaces in parameter.Type.GetNamespaces())
				{
					yield return parameterTypeNamespaces;
				}

				foreach (var attributeNamespace in parameter.GetAttributes().SelectMany(_ => _.GetNamespaces()))
				{
					yield return attributeNamespace;
				}
			}

			var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

			namespaces.AddRange(self.GetAttributes().SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(self.Type.GetNamespaces());

			if (self.IsIndexer)
			{
				namespaces.AddRange(self.Parameters.SelectMany(_ => GetParameterNamespaces(_)));
			}

			return namespaces.ToImmutable();
		}

		internal static PropertyAccessor GetAccessors(this IPropertySymbol self) =>
			self.GetMethod is not null && self.SetMethod is not null ?
				PropertyAccessor.GetAndSet : (self.SetMethod is null ? PropertyAccessor.Get : PropertyAccessor.Set);
	}
}