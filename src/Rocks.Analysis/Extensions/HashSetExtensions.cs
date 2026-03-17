namespace Rocks.Analysis.Extensions;

internal static class HashSetExtensions
{
	internal static void AddRange<T>(this HashSet<T> self, IEnumerable<T> values)
	{
		foreach (var value in values)
		{
			_ = self.Add(value);
		}
	}
}