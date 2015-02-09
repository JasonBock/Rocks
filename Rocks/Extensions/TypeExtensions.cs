using System;

namespace Rocks.Extensions
{
	internal static class TypeExtensions
	{
		internal static string Validate(this Type @this)
		{
			if(@this.IsSealed)
			{
				return string.Format(Constants.ErrorMessages.CannotMockSealedType, @this.Name);
			}

			// TODO: Does this type have any virtual members that could be overridden?

			return string.Empty;
		}
	}
}
