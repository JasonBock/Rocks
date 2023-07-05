using MassTransit;
using MassTransit.Configuration;
using MassTransit.Internals.GraphValidation;
using MassTransit.Mediator;
using MassTransit.Middleware;
using MassTransit.Observables;
using MassTransit.Transports;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;


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
						{ "T", "global::System.Object" },
						{ "TNode", "global::MassTransit.Internals.GraphValidation.Node<global::System.Object>" },
					}
				},
				{
					typeof(ActivityDefinition<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TLog", "global::System.Object" },
						{ "TArguments", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
					}
				},
				{
					typeof(BindContext<,>), new()
					{
						{ "TLeft", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TRight", "global::System.Object" },
					}
				},
				{
					typeof(BusInstance<>), new()
					{
						{ "TBus", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedBus" },
					}
				},
				{
					typeof(ChildSpecificationPipeBuilder<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
					}
				},
				{
					typeof(CompensateActivityContext<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "global::System.Object" },
					}
				},
				{
					typeof(CompensateActivityEndpointDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "global::System.Object" },
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
						{ "TData", "global::System.Object" },
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
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(ExecuteActivityDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(ExecuteActivityEndpointDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "global::System.Object" },
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
						{ "TLog", "global::System.Object" },
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(IActivityFactory<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TLog", "global::System.Object" },
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(IActivityRegistrationConfigurator<,,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedActivity" },
						{ "TArguments", "global::System.Object" },
						{ "TLog", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TEndpointConfigurator", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(ICompensateActivityConfigurator<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "global::System.Object" },
					}
				},
				{
					typeof(ICompensateActivityFactory<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCompensateActivity" },
						{ "TLog", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(IExecuteActivityDefinition<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(IExecuteActivityFactory<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "global::System.Object" },
					}
				},
				{
					typeof(IExecuteActivityRegistrationConfigurator<,>), new()
					{
						{ "TActivity", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedExecuteActivity" },
						{ "TArguments", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TEndpointConfigurator", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(IReceiveConnector<>), new()
					{
						{ "TEndpointConfigurator", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedReceiveEndpointConfigurator" },
					}
				},
				{
					typeof(IRequestConfigurator<,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IRequestConfigurator<,,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
					}
				},
				{
					typeof(IRequestConfigurator<,,,,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
						{ "TResponse3", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
					}
				},
				{
					typeof(ISagaMessageConfigurator<,>), new()
					{
						{ "TMessage", "global::System.Object" },
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
					}
				},
				{
					typeof(ISagaPolicy<,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSaga" },
						{ "TMessage", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
					}
				},
				{
					typeof(IServiceInstanceConfigurator<>), new()
					{
						{ "TEndpointConfigurator", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedReceiveEndpointConfigurator" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TResponse", "global::System.Object" },
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
						{ "T", "global::System.Object" },
						{ "TNode", "global::MassTransit.Internals.GraphValidation.Node<global::System.Object>" },
					}
				},
				{
					typeof(Orchestrates<>), new()
					{
						{ "TMessage", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedCorrelatedBy" },
					}
				},
				{
					typeof(PipeBuilder<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
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
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(Request<,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
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
					typeof(RequestSettings<,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestSettings<,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
					}
				},
				{
					typeof(RequestSettings<,,,,>), new()
					{
						{ "TSaga", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TRequest", "global::System.Object" },
						{ "TResponse", "global::System.Object" },
						{ "TResponse2", "global::System.Object" },
						{ "TResponse3", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
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
						{ "TMessage", "global::System.Object" },
					}
				},
				{
					typeof(ScheduleSettings<,>), new()
					{
						{ "TInstance", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedSagaStateMachineInstance" },
						{ "TMessage", "global::System.Object" },
					}
				},
				{
					typeof(SpecificationPipeBuilder<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
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
					typeof(SplitFilterPipeSpecification<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
						{ "TFilter", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedPipeContext" },
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
						{ "T", "global::System.Object" },
						{ "TNode", "global::Rocks.CodeGenerationTest.Mappings.MassTransit.MappedTarjanNodeProperties" },
					}
				},
				{
					typeof(TopologicalSort<,>), new()
					{
						{ "T", "global::System.Object" },
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

		public sealed class MappedReceiveEndpointConfigurator
			 : IReceiveEndpointConfigurator
		{
			public Uri InputAddress => throw new NotImplementedException();

			public bool ConfigureConsumeTopology { set => throw new NotImplementedException(); }
			public bool PublishFaults { set => throw new NotImplementedException(); }
			public int PrefetchCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public int? ConcurrentMessageLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public ContentType DefaultContentType { set => throw new NotImplementedException(); }
			public ContentType SerializerContentType { set => throw new NotImplementedException(); }
			public bool AutoStart { set => throw new NotImplementedException(); }

			public void AddDependency(IReceiveEndpointObserverConnector connector) => throw new NotImplementedException();
			public void AddDependency(IReceiveEndpointDependentConnector dependent) => throw new NotImplementedException();
			public void AddDependency(IReceiveEndpointDependency dependency) => throw new NotImplementedException();
			public void AddDependent(IReceiveEndpointObserverConnector dependency) => throw new NotImplementedException();
			public void AddDependent(IReceiveEndpointDependent dependent) => throw new NotImplementedException();
			public void AddDeserializer(ISerializerFactory factory, bool isDefault = false) => throw new NotImplementedException();
			public void AddEndpointSpecification(IReceiveEndpointSpecification configurator) => throw new NotImplementedException();
			public void AddPipeSpecification<T>(IPipeSpecification<ConsumeContext<T>> specification) where T : class => throw new NotImplementedException();
			public void AddPipeSpecification(IPipeSpecification<ConsumeContext> specification) => throw new NotImplementedException();
			public void AddPrePipeSpecification(IPipeSpecification<ConsumeContext> specification) => throw new NotImplementedException();
			public void AddSerializer(ISerializerFactory factory, bool isSerializer = true) => throw new NotImplementedException();
			public void ClearSerialization() => throw new NotImplementedException();
			public void ConfigureDeadLetter(Action<IPipeConfigurator<ReceiveContext>> callback) => throw new NotImplementedException();
			public void ConfigureError(Action<IPipeConfigurator<ExceptionReceiveContext>> callback) => throw new NotImplementedException();
			public void ConfigureMessageTopology<T>(bool enabled = true) where T : class => throw new NotImplementedException();
			public void ConfigurePublish(Action<IPublishPipeConfigurator> callback) => throw new NotImplementedException();
			public void ConfigureReceive(Action<IReceivePipeConfigurator> callback) => throw new NotImplementedException();
			public void ConfigureSend(Action<ISendPipeConfigurator> callback) => throw new NotImplementedException();
			public void ConfigureTransport(Action<ITransportConfigurator> callback) => throw new NotImplementedException();
			public ConnectHandle ConnectActivityConfigurationObserver(IActivityConfigurationObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectConsumerConfigurationObserver(IConsumerConfigurationObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectHandlerConfigurationObserver(IHandlerConfigurationObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectReceiveEndpointObserver(IReceiveEndpointObserver observer) => throw new NotImplementedException();
			public ConnectHandle ConnectSagaConfigurationObserver(ISagaConfigurationObserver observer) => throw new NotImplementedException();
			public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator) where TConsumer : class => throw new NotImplementedException();
			public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
			  where TConsumer : class
			  where TMessage : class => throw new NotImplementedException();
			public void HandlerConfigured<TMessage>(IHandlerConfigurator<TMessage> configurator) where TMessage : class => throw new NotImplementedException();
			void IActivityConfigurationObserver.ActivityConfigured<TActivity, TArguments>(IExecuteActivityConfigurator<TActivity, TArguments> configurator, Uri compensateAddress) => throw new NotImplementedException();
			void IActivityConfigurationObserver.CompensateActivityConfigured<TActivity, TLog>(ICompensateActivityConfigurator<TActivity, TLog> configurator) => throw new NotImplementedException();
			void IActivityConfigurationObserver.ExecuteActivityConfigured<TActivity, TArguments>(IExecuteActivityConfigurator<TActivity, TArguments> configurator) => throw new NotImplementedException();
			void ISagaConfigurationObserver.SagaConfigured<TSaga>(ISagaConfigurator<TSaga> configurator) => throw new NotImplementedException();
			void ISagaConfigurationObserver.SagaMessageConfigured<TSaga, TMessage>(ISagaMessageConfigurator<TSaga, TMessage> configurator) => throw new NotImplementedException();
			void ISagaConfigurationObserver.StateMachineSagaConfigured<TInstance>(ISagaConfigurator<TInstance> configurator, SagaStateMachine<TInstance> stateMachine) => throw new NotImplementedException();
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