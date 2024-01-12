using Coravel.Events.Interfaces;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class CoravelMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IEventSubscription<>), new()
					{
						{ "TEvent", "global::Coravel.Events.Interfaces.IEvent" },
					}
				},
				{
					typeof(IListener<>), new()
					{
						{ "TEvent", "global::Coravel.Events.Interfaces.IEvent" },
					}
				},
			};
	}
}