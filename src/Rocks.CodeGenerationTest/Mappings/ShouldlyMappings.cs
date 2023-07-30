using Shouldly.Configuration;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class ShouldlyMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(FindMethodUsingAttribute<>), new()
					{
						{ "T", "global::System.Attribute" },
					}
				},
			};
	}
}