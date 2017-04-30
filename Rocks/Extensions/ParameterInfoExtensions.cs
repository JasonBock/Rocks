using System;
using System.Collections.Generic;
#if NETCOREAPP1_1
using System.Linq;
#endif
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ParameterInfoExtensions
	{
		internal static string GetAttributes(this ParameterInfo @this)
		{
			return @this.GetAttributes(false, new SortedSet<string>());
		}

		internal static string GetAttributes(this ParameterInfo @this, bool isReturn)
		{
			return @this.GetAttributes(isReturn, new SortedSet<string>());
		}

		internal static string GetAttributes(this ParameterInfo @this, SortedSet<string> namespaces)
		{
			return @this.GetAttributes(false, namespaces);
		}

		internal static string GetAttributes(this ParameterInfo @this, bool isReturn, SortedSet<string> namespaces)
		{
#if !NETCOREAPP1_1
			var attributeData = @this.GetCustomAttributesData();
#else
			var attributeData = @this.CustomAttributes.ToList();
#endif
			return attributeData.GetAttributes(isReturn, namespaces, @this);
		}

		internal static string GetModifier(this ParameterInfo @this)
		{
			return @this.GetModifier(false);
      }

		internal static string GetModifier(this ParameterInfo @this, bool ignoreParams)
		{
			return (@this.IsOut && !@this.IsIn && @this.ParameterType.IsByRef) ? "out " :
				@this.ParameterType.IsByRef ? "ref " :
				ignoreParams ? string.Empty :
#if !NETCOREAPP1_1
				@this.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0 ? "params " : string.Empty;
#else
				@this.GetCustomAttributes(typeof(ParamArrayAttribute), false).ToArray().Length > 0 ? "params " : string.Empty;
#endif
		}
	}
}
