using System.Collections;

namespace Rocks.Runtime.Extensions;

/// <summary>
/// Provides extensions for <see cref="object"/> values.
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// This gets a stringified version of a value
	/// that will be put into the call site of an emitted method.
	/// </summary>
	public static string FormatValue(this object? self)
	{
		if (self is Enum)
		{
			var selfType = self.GetType();
			var selfName = Enum.GetName(selfType, self);
			var selfValue = selfName is not null ?
				$"{selfType.FullName}.{selfName}" :
				$"({selfType.FullName}){self}";

			return selfValue;
		}
		else if (self is ICollection c)
		{
			return $"{c}, Count = {c.Count}";
		}
		else if (self is null)
		{
			return "null";
		}
		else
		{
			return self.ToString() ?? "";
		}
	}
}
