namespace Rocks.Extensions;

/// <summary>
/// Extension methods for an <see cref="IDictionary{TKey, TValue}"/> instance.
/// </summary>
public static class IDictionaryOfTKeyTValueExtensions
{
	/// <summary>
	/// Adds a value to a dictionary, or updates it if it already exists for the given key.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="self">The <see cref="IDictionary{TKey, TValue}"/> instance.</param>
	/// <param name="key">The key.</param>
	/// <param name="add">The method that produces the new value.</param>
	/// <param name="update">The method that updates the existing value.</param>
	/// <exception cref="ArgumentNullException"></exception>
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