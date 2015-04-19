using System.Reflection;

namespace Rocks
{
	public static class ReflectionValues
	{
		public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
		public const BindingFlags PublicNonPublicInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
	}
}
