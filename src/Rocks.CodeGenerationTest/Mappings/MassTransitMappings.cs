using MassTransit;
using MassTransit.Configuration;
using MassTransit.Internals.GraphValidation;
using MassTransit.Mediator;
using MassTransit.Middleware;
using MassTransit.Observables;
using MassTransit.Transports;
using System.Diagnostics.CodeAnalysis;


namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class MassTransitMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AdjacencyList<,>), new()
					{
						{ "T", "object" },
						{ "TNode", "global::MassTransit.Internals.GraphValidation.Node<object>" },
					}
				},
				{
					typeof(ActivityDefinition<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TLog", "object" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(AsyncDelegateFilter<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(AsyncDelegatePipeSpecification<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(BehaviorContext<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(BehaviorContext<,>), new()
					{
						{ "TMessage", "object" },
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(BehaviorExceptionContext<,>), new()
					{
						{ "TException", "global::System.Exception" },
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(BehaviorExceptionContext<,,>), new()
					{
						{ "TException", "global::System.Exception" },
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(BindContext<,>), new()
					{
						{ "TLeft", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TRight", "object" },
					}
				},
				{
					typeof(BusInstance<>), new()
					{
						{ "TBus", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedBus" },
					}
				},
				{
					typeof(CompensateActivityContext<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "object" },
					}
				},
				{
					typeof(CompensateActivityEndpointDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "object" },
					}
				},
				{
					typeof(ConsumerDefinition<>), new()
					{
						{ "TConsumer", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedConsumer" },
					}
				},
				{
					typeof(ConsumerEndpointDefinition<>), new()
					{
						{ "TConsumer", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedConsumer" },
					}
				},
				{
					typeof(DelegateFilter<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(DelegatePipeSpecification<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(EmptyPipe<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(EventCorrelation<,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TData", "object" },
					}
				},
				{
					typeof(ExceptionSagaConsumeContext<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ExecuteActivityContext<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(ExecuteActivityDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(ExecuteActivityEndpointDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(FilterObservable<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(FilterPipe<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(FilterPipeSpecification<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(FutureDefinition<>), new()
					{
						{ "TFuture", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachine" },
					}
				},
				{
					typeof(IAgent<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IActivityDefinition<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TLog", "object" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IActivityFactory<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TLog", "object" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IActivityRegistrationConfigurator<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TArguments", "object" },
						{ "TLog", "object" },
					}
				},
				{
					typeof(IBehavior<>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IBehavior<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(IBuildPipeConfigurator<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IBusFactoryConfigurator<>), new()
					{
						{ "TEndpointConfigurator", "global::MassTransit.IReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(ICompensateActivityConfigurator<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "object" },
					}
				},
				{
					typeof(ICompensateActivityFactory<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "object" },
					}
				},
				{
					typeof(IConsumerDefinition<>), new()
					{
						{ "TConsumer", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedConsumer" },
					}
				},
				{
					typeof(IConsumerRegistrationConfigurator<>), new()
					{
						{ "TConsumer", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedConsumer" },
					}
				},
				{
					typeof(IEventCorrelationConfigurator<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(IEventObserver<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IExecuteActivityConfigurator<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IExecuteActivityDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IExecuteActivityFactory<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IExecuteActivityRegistrationConfigurator<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "object" },
					}
				},
				{
					typeof(IFilter<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IFilterObserver<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IFilterObserverConnector<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IFutureDefinition<>), new()
					{
						{ "TFuture", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachine" },
					}
				},
				{
					typeof(IInstanceConfigurator<>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedConsumer" },
					}
				},
				{
					typeof(ILoadSagaRepository<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IMissingInstanceConfigurator<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(InitiatedBy<>), new()
					{
						{ "TMessage", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCorrelatedBy" },
					}
				},
				{
					typeof(InitiatedByOrOrchestrates<>), new()
					{
						{ "TMessage", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCorrelatedBy" },
					}
				},
				{
					typeof(IPipe<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IPipeBuilder<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IPipeConfigurator<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IPipeContextSource<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IPipeContextSource<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TInput", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IPipeSpecification<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IReceiveConfigurator<>), new()
					{
						{ "TEndpointConfigurator", "global::MassTransit.IReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(IReceiveConnector<>), new()
					{
						{ "TEndpointConfigurator", "global::MassTransit.IReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(IRequestConfigurator<,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(IRequestConfigurator<,,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
					}
				},
				{
					typeof(IRequestConfigurator<,,,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
						{ "TResponse3", "object" },
					}
				},
				{
					typeof(IQuerySagaRepository<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaConfigurator<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaDefinition<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaFactory<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(ISagaMessageConfigurator<,>), new()
					{
						{ "TMessage", "object" },
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaPolicy<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(ISagaQuery<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaQueryFactory<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(ISagaRepository<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IScheduleConfigurator<,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(IServiceInstanceConfigurator<>), new()
					{
						{ "TEndpointConfigurator", "global::MassTransit.IReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(ISpecificationPipeBuilder<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(ISpecificationPipeSpecification<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(IStateAccessor<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IStateMachineActivity<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(IStateMachineActivity<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(IStateObserver<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISupervisor<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(ITopologyPipeBuilder<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(ITransportSupervisor<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(LastPipe<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(MediatorRequestHandler<,>), new()
					{
						{ "TRequest", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedRequest" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(MergePipe<,>), new()
					{
						{ "TSplit", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TInput", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(NodeList<,>), new()
					{
						{ "T", "object" },
						{ "TNode", "global::MassTransit.Internals.GraphValidation.Node<object>" },
					}
				},
				{
					typeof(Orchestrates<>), new()
					{
						{ "TMessage", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCorrelatedBy" },
					}
				},
				{
					typeof(PipeConfigurator<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(Request<,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(Request<,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
					}
				},
				{
					typeof(Request<,,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
						{ "TResponse3", "object" },
					}
				},
				{
					typeof(RequestSettings<,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(RequestSettings<,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
					}
				},
				{
					typeof(RequestSettings<,,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "object" },
						{ "TResponse", "object" },
						{ "TResponse2", "object" },
						{ "TResponse3", "object" },
					}
				},
				{
					typeof(SagaConsumeContext<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(SagaConsumeContext<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(SagaDefinition<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(SagaEndpointDefinition<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(SagaStateMachine<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
					}
				},
				{
					typeof(Schedule<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
					}
				},
				{
					typeof(Schedule<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(ScheduleSettings<,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "object" },
					}
				},
				{
					typeof(SplitFilter<,>), new()
					{
						{ "TSplit", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TInput", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(State<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(StateMachine<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(Tarjan<,>), new()
					{
						{ "T", "object" },
						{ "TNode", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedTarjanNodeProperties" },
					}
				},
				{
					typeof(TopologicalSort<,>), new()
					{
						{ "T", "object" },
						{ "TNode", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedTopologicalSortNodeProperties" },
					}
				},
				{
					typeof(UnhandledEventContext<>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
			};
	}

	namespace MassTransit
   {
	  public sealed class MappedTopologicalSortNodeProperties
			: Node<object>, ITopologicalSortNodeProperties
		{
			public MappedTopologicalSortNodeProperties(int index, object value) : base(index, value)
			{
			}

			public bool Visited { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}

		public sealed class MappedTarjanNodeProperties
			 : Node<object>, ITarjanNodeProperties
		{
			public MappedTarjanNodeProperties(int index, object value) : base(index, value)
			{
			}

			public int Index { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public int LowLink { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}

		public sealed class MappedBus
			 : IBus
		{
			public Uri Address => throw new NotImplementedException();

			public IBusTopology Topology => throw new NotImplementedException();

			public ConnectHandle ConnectConsumeMessageObserver<T>(IConsumeMessageObserver<T> observer) where T : class => throw new NotImplementedException();
			public ConnectHandle ConnectConsumeObserver(IConsumeObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectConsumePipe<T>(IPipe<ConsumeContext<T>> pipe) where T : class => throw new NotImplementedException();
			public ConnectHandle ConnectConsumePipe<T>(IPipe<ConsumeContext<T>> pipe, ConnectPipeOptions options) where T : class => throw new NotImplementedException();
			public ConnectHandle ConnectEndpointConfigurationObserver(IEndpointConfigurationObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectPublishObserver(IPublishObserver observer) => throw new NotImplementedException();
			public HostReceiveEndpointHandle ConnectReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter? endpointNameFormatter = null, Action<IReceiveEndpointConfigurator>? configureEndpoint = null) => throw new NotImplementedException();
			public HostReceiveEndpointHandle ConnectReceiveEndpoint(string queueName, Action<IReceiveEndpointConfigurator>? configureEndpoint = null) => throw new NotImplementedException();
			public ConnectHandle ConnectReceiveEndpointObserver(IReceiveEndpointObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectReceiveObserver(IReceiveObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectRequestPipe<T>(Guid requestId, IPipe<ConsumeContext<T>> pipe) where T : class => throw new NotImplementedException();
			public ConnectHandle ConnectSendObserver(ISendObserver observer) => throw new NotImplementedException();
			public Task<ISendEndpoint> GetPublishSendEndpoint<T>() where T : class => throw new NotImplementedException();
			public Task<ISendEndpoint> GetSendEndpoint(Uri address) => throw new NotImplementedException();
			public void Probe(ProbeContext context) => throw new NotImplementedException();
			public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
			public Task Publish<T>(T message, IPipe<PublishContext<T>> publishPipe, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
			public Task Publish<T>(T message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
			public Task Publish(object message, CancellationToken cancellationToken = default) => throw new NotImplementedException();
			public Task Publish(object message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) => throw new NotImplementedException();
			public Task Publish(object message, Type messageType, CancellationToken cancellationToken = default) => throw new NotImplementedException();
			public Task Publish(object message, Type messageType, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) => throw new NotImplementedException();
			public Task Publish<T>(object values, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
			public Task Publish<T>(object values, IPipe<PublishContext<T>> publishPipe, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
			public Task Publish<T>(object values, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T : class => throw new NotImplementedException();
		}

		public sealed class MappedActivity
			 : IActivity<object, object>
		{
			public Task<CompensationResult> Compensate(CompensateContext<object> context) => throw new NotImplementedException();
			public Task<ExecutionResult> Execute(ExecuteContext<object> context) => throw new NotImplementedException();
		}

		public sealed class MappedCompensateActivity
			 : ICompensateActivity<object>
		{
			public Task<CompensationResult> Compensate(CompensateContext<object> context) => throw new NotImplementedException();
		}

		public sealed class MappedExecuteActivity
			 : IExecuteActivity<object>
		{
			public Task<ExecutionResult> Execute(ExecuteContext<object> context) => throw new NotImplementedException();
		}

		public sealed class MappedCorrelatedBy
			 : CorrelatedBy<Guid>
		{
			public Guid CorrelationId => throw new NotImplementedException();
		}

		public sealed class MappedConsumer
			 : IConsumer
		{ }

		public sealed class MappedSagaStateMachine
			: SagaStateMachine<FutureState>
		{
			public IEnumerable<EventCorrelation> Correlations => throw new NotImplementedException();

			public IStateAccessor<FutureState> Accessor => throw new NotImplementedException();

			public string Name => throw new NotImplementedException();

			public IEnumerable<Event> Events => throw new NotImplementedException();

			public IEnumerable<State> States => throw new NotImplementedException();

			public Type InstanceType => throw new NotImplementedException();

			public State Initial => throw new NotImplementedException();

			public State Final => throw new NotImplementedException();

			public void Accept(StateMachineVisitor visitor) => throw new NotImplementedException();
			public IDisposable ConnectEventObserver(IEventObserver<FutureState> observer) => throw new NotImplementedException();
			public IDisposable ConnectEventObserver(Event @event, IEventObserver<FutureState> observer) => throw new NotImplementedException();
			public IDisposable ConnectStateObserver(IStateObserver<FutureState> observer) => throw new NotImplementedException();
			public Event GetEvent(string name) => throw new NotImplementedException();
			public State<FutureState> GetState(string name) => throw new NotImplementedException();
			public Task<bool> IsCompleted(BehaviorContext<FutureState> context) => throw new NotImplementedException();
			public bool IsCompositeEvent(Event @event) => throw new NotImplementedException();
			public IEnumerable<Event> NextEvents(State state) => throw new NotImplementedException();
			public void Probe(ProbeContext context) => throw new NotImplementedException();
			public Task RaiseEvent(BehaviorContext<FutureState> context) => throw new NotImplementedException();
			public Task RaiseEvent<T>(BehaviorContext<FutureState, T> context) where T : class => throw new NotImplementedException();
			State StateMachine.GetState(string name) => throw new NotImplementedException();
		}

		public sealed class MappedRequest
			 : Request<object>
		{ }

		public sealed class MappedStateMachineActivity
			: IStateMachineActivity
		{
			public void Accept(StateMachineVisitor visitor) => throw new NotImplementedException();
			public void Probe(ProbeContext context) => throw new NotImplementedException();
		}

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
			public bool TryGetPayload<T>([NotNullWhen(true)] out T? payload) where T : class => throw new NotImplementedException();
		}

		public sealed class MappedSaga
			 : ISaga
		{
			public Guid CorrelationId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}
	}
}