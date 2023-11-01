using Ninject.Planning.Targets;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class NinjectMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(Target<>), new()
					{
						{ "T", "global::System.Reflection.ICustomAttributeProvider" },
					}
				},
			};
	}
}