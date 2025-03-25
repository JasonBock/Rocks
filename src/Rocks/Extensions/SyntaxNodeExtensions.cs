using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class SyntaxNodeExtensions
{
	internal static T FindParent<T>(this SyntaxNode self)
		where T : SyntaxNode
	{
		var parent = self.Parent;

		while (parent is not T && parent is not null)
		{
			parent = parent.Parent;
		}

		return (T)parent!;
	}
}