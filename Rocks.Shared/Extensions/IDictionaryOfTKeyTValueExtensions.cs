using System;
using System.Collections.Generic;

namespace Rocks.Extensions
{
	internal static class IDictionaryOfTKeyTValueExtensions
	{
		internal static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, 
			Func<TValue> add, Action<TValue> update)
		{
			TValue value = default(TValue);

			if(@this.TryGetValue(key, out value))
			{
				update(value);
			}
			else
			{
				@this[key] = add();
			}
		}
	}
}
