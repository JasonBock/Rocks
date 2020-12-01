using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class DelegateBuilder
	{
		internal static string Build(ImmutableArray<IParameterSymbol> parameters, ITypeSymbol? returnType = null)
		{
			if(parameters.Length > 0)
			{
				var parameterTypes = string.Join(", ", parameters.Select(_ => _.Type.GetName()));
				return returnType is not null ?
					$"Func<{parameterTypes}, {returnType.GetName()}>" : 
					$"Action<{parameterTypes}>";
			}
			else
			{
				return returnType is not null ?
					$"Func<{returnType.GetName()}>" :
					$"Action";
			}
		}
	}
}