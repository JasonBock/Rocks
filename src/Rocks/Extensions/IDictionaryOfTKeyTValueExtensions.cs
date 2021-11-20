namespace Rocks.Extensions;

public static class IDictionaryOfTKeyTValueExtensions
{
	public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key,
		Func<TValue>? add, Action<TValue>? update)
	{
		if (self is null)
		{
			throw new ArgumentNullException(nameof(self));
		}

		if (self.TryGetValue(key, out var value))
		{
			if (update is null)
			{
				throw new ArgumentNullException(nameof(update));
			}

			update(value);
		}
		else
		{
			if (add is null)
			{
				throw new ArgumentNullException(nameof(add));
			}

			self[key] = add();
		}
	}
}