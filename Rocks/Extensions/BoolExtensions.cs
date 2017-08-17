namespace Rocks.Extensions
{
	internal static class BoolExtensions
	{
		internal static string GetValue(this bool @this) => @this ? "true" : "false";
	}
}
