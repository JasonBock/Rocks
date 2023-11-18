using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class OpenApiMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IEffective<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Microsoft.OpenApi.MappedOpenApiElement" },
					}
				},
				{
					typeof(OpenApiExtensibleDictionary<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Microsoft.OpenApi.MappedOpenApiSerializable" },
					}
				},
			};
	}

	namespace Microsoft.OpenApi
	{
		public sealed class MappedOpenApiElement
			: IOpenApiElement
		{ }

		public sealed class MappedOpenApiSerializable
			: IOpenApiSerializable
		{
			public void SerializeAsV2(IOpenApiWriter writer) => throw new NotImplementedException();
			public void SerializeAsV3(IOpenApiWriter writer) => throw new NotImplementedException();
		}
	}
}