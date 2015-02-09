using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocks.Extensions
{
	internal static class MethodInfoExtensions
	{
		internal static string GetMethodDescription(this MethodInfo @this)
		{
			// TODO: this doesn't support generic methods yet.
			var result = new StringBuilder();

			result.Append(@this.ReturnType == typeof(void) ?
				"void " : @this.ReturnType.FullName + " ");

			result.Append(@this.Name + "(");
			result.Append(string.Join(", ",
				from parameter in @this.GetParameters()
				select parameter.ParameterType.FullName + " " + parameter.Name));
			result.Append(")");
			return result.ToString();
      }
	}
}
