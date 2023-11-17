using Orleans;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class OrleansMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ILifecycleParticipant<>), new()
					{
						{ "TLifecycleObservable", "global::Orleans.ILifecycleObservable" },
					}
				},
			};
	}
}