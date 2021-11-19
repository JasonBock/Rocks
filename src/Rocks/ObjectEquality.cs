namespace Rocks;

internal static class ObjectEquality
{
	internal static bool AreEqual<T>(T value1, T value2)
	{
		if (value1 is null && value2 is null)
		{
			return true;
		}
		else if (value1 is not null && value2 is not null)
		{
			if (!typeof(T).IsArray)
			{
				return value1.Equals(value2);
			}
			else
			{
				var array1 = (value1 as Array)!;
				var array2 = (value2 as Array)!;

				if (array1.Length != array2.Length)
				{
					return false;
				}

				for (var i = 0; i < array1.Length; i++)
				{
					if (!ObjectEquality.AreEqual(array1.GetValue(i), array2.GetValue(i)))
					{
						return false;
					}
				}

				return true;
			}
		}
		else
		{
			return false;
		}
	}
}