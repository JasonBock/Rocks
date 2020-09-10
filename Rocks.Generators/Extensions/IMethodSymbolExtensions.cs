using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

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
						!IMethodSymbolExtensions.AreCustomModifiersEqual(selfParameter.CustomModifiers, otherParameter.CustomModifiers) ||
						!IMethodSymbolExtensions.AreCustomModifiersEqual(selfParameter.RefCustomModifiers, otherParameter.RefCustomModifiers) ||
						selfParameter.IsParams != otherParameter.IsParams)
					{
						return MethodMatch.None;
					}
				}

				return self.ReturnType.Equals(other.ReturnType, SymbolEqualityComparer.Default) ? 
					MethodMatch.Exact : MethodMatch.DifferByReturnTypeOnly;
			}
		}

		private static bool AreCustomModifiersEqual(ImmutableArray<CustomModifier> selfModifiers, ImmutableArray<CustomModifier> otherModifiers)
		{
			if (selfModifiers.Length != otherModifiers.Length)
			{
				return false;
			}

			foreach (var selfModifier in selfModifiers)
			{
				if (!otherModifiers.Contains(selfModifier))
				{
					return false;
				}
			}

			return true;
		}
	}
}