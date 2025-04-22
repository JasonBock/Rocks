namespace Rocks.Analysis.Extensions;

internal static class HashSetBuilderExtensions
{
	internal static void AddRange<T>(this HashSet<T> self, IEnumerable<T> values)
	{
		foreach (var value in values)
		{
			self.Add(value);
		}
	}
}