using Autofac.Features.Scanning;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class AutofacMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(BaseScanningActivatorData<,>), new()
					{
						{ "TActivatorData", "global::Autofac.Builder.ReflectionActivatorData" },
						{ "TRegistrationStyle", "object" },
					}
				},
			};
	}
}