using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks.Extensions
{
	internal static class ImmutableHashSetBuilderExtensions
	{
		internal static void AddRange<T>(this ImmutableHashSet<T>.Builder self, IEnumerable<T> values)
		{
			foreach(var value in values)
			{
				self.Add(value);
			}
		}
	}
}