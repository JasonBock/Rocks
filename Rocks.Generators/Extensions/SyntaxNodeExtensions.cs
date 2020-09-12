using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	internal static class SyntaxNodeExtensions
	{
		internal static T FindParent<T>(this SyntaxNode @this)
			where T : SyntaxNode
		{
			var parent = @this.Parent;

			while (!(parent is T) && parent is not null)
			{
				parent = parent.Parent;
			}

			return (T)(parent!);
		}
	}
}