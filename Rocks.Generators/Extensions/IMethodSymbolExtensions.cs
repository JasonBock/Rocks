using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	internal static class IMethodSymbolExtensions
	{
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