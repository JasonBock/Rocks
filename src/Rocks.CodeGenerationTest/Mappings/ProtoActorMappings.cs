using Proto.Deduplication;
using Proto.Extensions;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class ProtoActorMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(DeduplicationContext<>), new()
					{
						{ "T", "int" },
					}
				},
				{
					typeof(IActorSystemExtension<>), new()
					{
						{ "T", "global::Proto.Extensions.IActorSystemExtension" },
					}
				},
			};
	}
}