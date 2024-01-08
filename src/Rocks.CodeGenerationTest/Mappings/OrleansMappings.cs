using Orleans;
using Orleans.Services;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class OrleansMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IGrainServiceClient<>), new()
					{
						{ "TGrainService", "global::Orleans.Services.IGrainService" },
					}
				},
				{
					typeof(ILifecycleParticipant<>), new()
					{
						{ "TLifecycleObservable", "global::Orleans.ILifecycleObservable" },
					}
				},
			};
	}
}