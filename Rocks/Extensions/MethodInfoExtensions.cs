using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocks.Extensions
{
	internal static class MethodInfoExtensions
	{
		internal static string GetArgumentNameList(this MethodInfo @this)
		{
			return string.Join(", ", @this.GetParameters().Select(_ => _.Name));
		}

		internal static string GetMethodDescription(this MethodInfo @this, SortedSet<string> namespaces)
		{
			// TODO: this doesn't support generic methods yet.
			var result = new StringBuilder();

			if(@this.ReturnType == typeof(void))
			{
				result.Append("void ");
			}
			else
			{
				result.Append(@this.ReturnType.Name + " ");
				namespaces.Add(@this.ReturnType.Namespace);
			}

			result.Append(@this.Name + "(");
			result.Append(string.Join(", ",
				from parameter in @this.GetParameters()
				let _ = namespaces.Add(parameter.ParameterType.Namespace)
				select parameter.ParameterType.Name + " " + parameter.Name));
			result.Append(")");
			return result.ToString();
      }
	}
}
