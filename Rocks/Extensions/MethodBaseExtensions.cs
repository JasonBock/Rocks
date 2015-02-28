using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MethodBaseExtensions
	{
		internal static string GetArgumentNameList(this MethodBase @this)
		{
			return string.Join(", ",
				(from parameter in @this.GetParameters()
				 let modifier = parameter.GetModifier(true)
				 select $"{modifier}{parameter.Name}"));
		}

		internal static string GetParameters(this MethodBase @this, SortedSet<string> namespaces)
		{
			return string.Join(", ",
				from parameter in @this.GetParameters()
				let _ = namespaces.Add(parameter.ParameterType.Namespace)
				let modifier = parameter.GetModifier()
				let arrayText = parameter.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0 ? "[]" : string.Empty
				let parameterType = !string.IsNullOrWhiteSpace(modifier) ?
					$"{parameter.ParameterType.GetElementType().GetSafeName()}{arrayText}" :
					$"{parameter.ParameterType.GetSafeName()}{arrayText}"
				select $"{modifier}{parameterType} {parameter.Name}");
		}
	}
}
