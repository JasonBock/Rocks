namespace Rocks.CodeGenerationTest.Extensions.Extensions;

internal static class DictionaryOfTKeyTValueExtensions
{
	internal static Dictionary<TKey, TValue> AddItems<TKey, TValue>(
		this Dictionary<TKey, TValue> self, Dictionary<TKey, TValue> other)
		where TKey : notnull
	{ 
		foreach((var otherKey, var otherValue) in other)
		{
			self[otherKey] = otherValue;
		}

		return self;
	}
}