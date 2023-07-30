using Elasticsearch.Net;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ElasticSearchMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ConnectionConfiguration<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Elasticsearch.MappedConnectionConfiguration" },
					}
				},
				{
					typeof(ITransport<>), new()
					{
						{ "TConnectionSettings", "global::Elasticsearch.Net.IConnectionConfigurationValues" },
					}
				},
				{
					typeof(RequestParameters<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Elasticsearch.MappedRequestParameters" },
					}
				},
				{
					typeof(Transport<>), new()
					{
						{ "TConnectionSettings", "global::Elasticsearch.Net.IConnectionConfigurationValues" },
					}
				},
			};
	}

	namespace Elasticsearch
	{
		public sealed class MappedConnectionConfiguration
			: ConnectionConfiguration<MappedConnectionConfiguration>
		{
			public MappedConnectionConfiguration(IConnectionPool connectionPool, IConnection connection, IElasticsearchSerializer requestResponseSerializer) : base(connectionPool, connection, requestResponseSerializer)
			{
			}
		}

		public sealed class MappedRequestParameters
			: RequestParameters<MappedRequestParameters>
		{
			public override global::Elasticsearch.Net.HttpMethod DefaultHttpMethod => throw new NotImplementedException();

			public override bool SupportsBody => throw new NotImplementedException();
		}
	}
}