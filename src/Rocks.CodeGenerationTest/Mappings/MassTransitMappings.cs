using MassTransit;
using MassTransit.Transports;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class MassTransitMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(SagaConsumeContext<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedTransit" },
					}
				},
				{
					typeof(Request<,,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
						{ "TResponse3", "global::System.Object" },
					}
				},
				{
					typeof(ITransportSupervisor<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(State<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
			};
	}

	namespace MassTransit
	{
		public sealed class MappedSagaStateMachineInstance
			: SagaStateMachineInstance
		{
			public Guid CorrelationId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}

		public sealed class MappedPipeContext
			 : PipeContext
		{
			public CancellationToken CancellationToken => throw new NotImplementedException();

			public T AddOrUpdatePayload<T>(PayloadFactory<T> addFactory, UpdatePayloadFactory<T> updateFactory) where T : class => throw new NotImplementedException();
			public T GetOrAddPayload<T>(PayloadFactory<T> payloadFactory) where T : class => throw new NotImplementedException();
			public bool HasPayloadType(Type payloadType) => throw new NotImplementedException();
			public bool TryGetPayload<T>(out T? payload) where T : class => throw new NotImplementedException();
		}

		public sealed class MappedSaga
			 : ISaga
		{
			public Guid CorrelationId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}
	}
}