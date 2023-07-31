using Npgsql.Internal.TypeHandling;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class NinjectMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(NpgsqlSimpleTypeHandlerWithPsv<,>), new()
					{
						{ "TDefault", "string" },
						{ "TPsv", "object" },
					}
				},
			};
	}
}