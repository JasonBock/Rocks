using Twilio.Base;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class TwilioMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IOptions<>), new()
					{
						{ "T", "global::Twilio.Base.Resource" },
					}
				},
				{
					typeof(ReadOptions<>), new()
					{
						{ "T", "global::Twilio.Base.Resource" },
					}
				},
				{
					typeof(ResourceSet<>), new()
					{
						{ "T", "global::Twilio.Base.Resource" },
					}
				},
			};
	}
}