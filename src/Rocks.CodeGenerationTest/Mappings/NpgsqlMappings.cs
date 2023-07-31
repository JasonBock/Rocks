using Ninject;
using Ninject.Planning.Directives;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class NpgsqlMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(GlobalKernelRegistrationModule<>), new()
					{
						{ "TGlobalKernelRegistry", "global::Ninject.GlobalKernelRegistration" },
					}
				},
				{
					typeof(MethodInjectionDirectiveBase<,>), new()
					{
						{ "TMethod", "global::System.Reflection.MethodBase" },
						{ "TInjector", "object" },
					}
				},
			};
	}
}