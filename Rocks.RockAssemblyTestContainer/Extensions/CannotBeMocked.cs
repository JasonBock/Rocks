namespace Rocks.RockAssemblyTestContainer.Extensions
{
	namespace TestAssembly.Extensions
	{
		public static class CannotBeMocked
		{
			public static void Method1(this string @this) { }
		}
	}
}