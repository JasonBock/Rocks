using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			if(@this.IsGenericMethod)
			{
				@this = @this.GetGenericMethodDefinition();
			}

			namespaces.Add(@this.ReturnType.Namespace);
         var returnType = @this.ReturnType == typeof(void) ?
				"void" :  @this.ReturnType.Name;

			var methodName = @this.Name;
			var genericsArguments = @this.IsGenericMethod ?
				string.Format("<{0}>",
					string.Join(", ", from argument in @this.GetGenericArguments()
											select argument.Name)) :
				string.Empty;

			var parameters = string.Join(", ",
				from parameter in @this.GetParameters()
				let _ = namespaces.Add(parameter.ParameterType.Namespace)
				select parameter.ParameterType.Name + " " + parameter.Name);
			return $"{returnType} {methodName}{genericsArguments}({parameters})";
      }
	}
}
