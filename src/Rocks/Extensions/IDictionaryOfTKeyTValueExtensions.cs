namespace Rocks.Extensions;

public static class IDictionaryOfTKeyTValueExtensions
{
	public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key,
		Func<TValue> add, Action<TValue> update)
	{
		if (self is null)
		{
			throw new ArgumentNullException(nameof(self));
		}

		if (add is null)
		{
			throw new ArgumentNullException(nameof(add));
		}

		if (update is null)
		{
			throw new ArgumentNullException(nameof(update));
		}

		if (self.TryGetValue(key, out var value))
		{
			update(value);
		}
		else
		{
			self[key] = add();
		}
	}
}