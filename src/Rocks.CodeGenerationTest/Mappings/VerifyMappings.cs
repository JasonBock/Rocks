namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class VerifyMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(WriteOnlyJsonConverter<>), new()
					{
						{ "T", "string" },
					}
				},
			};
	}
}