using System;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ParameterInfoExtensions
	{
		internal static string GetModifier(this ParameterInfo @this)
		{
			return @this.GetModifier(false);
      }

		internal static string GetModifier(this ParameterInfo @this, bool ignoreParams)
		{
			return (@this.IsOut && !@this.IsIn) ? "out " :
				@this.ParameterType.IsByRef ? "ref " :
				ignoreParams ? string.Empty :
				@this.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0 ? "params " : string.Empty;
		}
	}
}
