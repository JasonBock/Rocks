using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class IMethodSymbolExtensions
	{
		internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IMethodSymbol self)
		{
			static IEnumerable<INamespaceSymbol> GetParameterNamespaces(IParameterSymbol parameter)
			{
				foreach(var parameterTypeNamespace in parameter.Type.GetNamespaces())
				{
					yield return parameterTypeNamespace;
				}

				foreach (var attributeNamespace in parameter.GetAttributes().SelectMany(_ => _.GetNamespaces()))
				{
					yield return attributeNamespace;
				}
			}

			var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

			if(!self.ReturnsVoid)
			{
				namespaces.AddRange(self.ReturnType.GetNamespaces());
				namespaces.AddRange(self.GetReturnTypeAttributes().SelectMany(_ => _.GetNamespaces()));
			}

			namespaces.AddRange(self.GetAttributes().SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(self.Parameters.SelectMany(_ => GetParameterNamespaces(_)));

			return namespaces.ToImmutable();
		}

		internal static MethodMatch Match(this IMethodSymbol self, IMethodSymbol other)
		{
			if (self.Name != other.Name)
			{
				return MethodMatch.None;
			}
			else
			{
				var selfParameters = self.Parameters;
				var otherParameters = other.Parameters;

				if (selfParameters.Length != otherParameters.Length)
				{
					return MethodMatch.None;
				}

				for (var i = 0; i < selfParameters.Length; i++)
				{
					var selfParameter = selfParameters[i];
					var otherParameter = otherParameters[i];

					if (!selfParameter.Type.Equals(otherParameter.Type, SymbolEqualityComparer.Default) ||
						!(selfParameter.RefKind == otherParameter.RefKind ||
							(selfParameter.RefKind == RefKind.Ref && otherParameter.RefKind == RefKind.Out) ||
							(selfParameter.RefKind == RefKind.Out && otherParameter.RefKind == RefKind.Ref)) ||
						selfParameter.IsParams != otherParameter.IsParams)
					{
						return MethodMatch.None;
					}
				}

				return self.ReturnType.Equals(other.ReturnType, SymbolEqualityComparer.Default) ? 
					MethodMatch.Exact : MethodMatch.DifferByReturnTypeOnly;
			}
		}
	}
}