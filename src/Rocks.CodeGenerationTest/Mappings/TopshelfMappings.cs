using Topshelf;
using Topshelf.Builders;
using Topshelf.ServiceConfigurators;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class TopshelfMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ControlServiceBuilder<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Topshelf.MappedServiceControl" },
					}
				},
				{
					typeof(ControlServiceConfigurator<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Topshelf.MappedServiceControl" },
					}
				},
			};
	}

	namespace Topshelf
	{
		public sealed class MappedServiceControl : ServiceControl
		{
			public bool Start(HostControl hostControl) => throw new NotImplementedException();
			public bool Stop(HostControl hostControl) => throw new NotImplementedException();
		}
	}
}