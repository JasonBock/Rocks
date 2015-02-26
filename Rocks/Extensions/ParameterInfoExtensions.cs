using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ParameterInfoExtensions
	{
		internal static string GetModifier(this ParameterInfo @this)
		{
			return @this.IsOut ? "out " : @this.ParameterType.IsByRef ? "ref " : string.Empty;
      }
	}
}
