using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Extensions;

internal static class SyntaxNodeExtensions
{
	internal static T? FindParent<T>(this SyntaxNode self, 
		Predicate<T> isTarget)
		where T : SyntaxNode
	{
		var parent = self.Parent;

		while (parent is not null)
		{
			if (parent is T tParent && isTarget(tParent))
			{
				return (T)parent!;
			}

			parent = parent.Parent;
		}

		return null;
	}
}