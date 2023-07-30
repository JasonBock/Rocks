using NServiceBus.DataBus;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;
using NServiceBus.Serialization;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class NServiceBusMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(Behavior<>), new()
					{
						{ "TContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(DataBusExtensions<>), new()
					{
						{ "T", "global::NServiceBus.DataBus.DataBusDefinition" },
					}
				},
				{
					typeof(ForkConnector<,>), new()
					{
						{ "TFromContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
						{ "TForkContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(IBehavior<,>), new()
					{
						{ "TInContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
						{ "TOutContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(ISagaFinder<,>), new()
					{
						{ "TSagaData", "global::NServiceBus.IContainSagaData" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(IToSagaExpression<>), new()
					{
						{ "TSagaData", "global::NServiceBus.IContainSagaData" },
					}
				},
				{
					typeof(PersistenceExtensions<>), new()
					{
						{ "T", "global::NServiceBus.Persistence.PersistenceDefinition" },
					}
				},
				{
					typeof(PersistenceExtensions<,>), new()
					{
						{ "T", "global::NServiceBus.Persistence.PersistenceDefinition" },
						{ "S", "global::NServiceBus.StorageType" },
					}
				},
				{
					typeof(PipelineTerminator<>), new()
					{
						{ "T", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(RoutingSettings<>), new()
					{
						{ "T", "global::NServiceBus.Transport.TransportDefinition" },
					}
				},
				{
					typeof(Saga<>), new()
					{
						{ "TSagaData", "global::Rocks.CodeGenerationTest.Mappings.NServiceBus.MappedContainSagaData" },
					}
				},
				{
					typeof(SagaPropertyMapper<>), new()
					{
						{ "TSagaData", "global::Rocks.CodeGenerationTest.Mappings.NServiceBus.MappedContainSagaData" },
					}
				},
				{
					typeof(SerializationExtensions<>), new()
					{
						{ "T", "global::NServiceBus.Serialization.SerializationDefinition" },
					}
				},
				{
					typeof(StageConnector<,>), new()
					{
						{ "TFromContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
						{ "TToContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(StageForkConnector<,,>), new()
					{
						{ "TFromContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
						{ "TToContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
						{ "TForkContext", "global::NServiceBus.Pipeline.IBehaviorContext" },
					}
				},
				{
					typeof(ToSagaExpression<,>), new()
					{
						{ "TSagaData", "global::Rocks.CodeGenerationTest.Mappings.NServiceBus.MappedContainSagaData" },
						{ "TMessage", "object" },
					}
				},
			};
	}

	namespace NServiceBus
	{
#pragma warning disable NSB0012 // Saga data classes should inherit ContainSagaData
		public sealed class MappedContainSagaData : IContainSagaData
#pragma warning restore NSB0012 // Saga data classes should inherit ContainSagaData
		{
			public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string? Originator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string? OriginalMessageId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}
	}
}