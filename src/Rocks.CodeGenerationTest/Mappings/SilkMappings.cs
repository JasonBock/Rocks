using Silk.NET.Core.Native;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class SilkMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(NativeExtension<>), new()
					{
						{ "T", "global::Silk.NET.Core.Native.NativeAPI" },
					}
				},
			};
	}
}