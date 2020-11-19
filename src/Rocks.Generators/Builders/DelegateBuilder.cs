using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class DelegateBuilder
	{
		internal static string GetDelegate(ImmutableArray<IParameterSymbol> parameters, ITypeSymbol? returnType = null)
		{
			if(parameters.Length > 0)
			{
				var parameterTypes = string.Join(", ", parameters.Select(_ => _.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
				return returnType is not null ?
					$"Func<{parameterTypes}, {returnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>" : 
					$"Action<{parameterTypes}>";
			}
			else
			{
				return returnType is not null ?
					$"Func<{returnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>" :
					$"Action";
			}
		}
	}
}