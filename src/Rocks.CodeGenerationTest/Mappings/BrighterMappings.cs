using Paramore.Brighter;
using Paramore.Brighter.FeatureSwitch.Handlers;
using Paramore.Brighter.Inbox.Exceptions;
using Paramore.Brighter.Inbox.Handlers;
using Paramore.Brighter.Logging.Handlers;
using Paramore.Brighter.Monitoring.Handlers;
using Paramore.Brighter.Policies.Handlers;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class BrighterMappings
	{
#pragma warning disable CS0618 // Type or member is obsolete
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AsyncPipelines<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(ExceptionPolicyHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(ExceptionPolicyHandlerAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(FallbackPolicyHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(FallbackPolicyHandlerRequestHandlerAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(FeatureSwitchHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(FeatureSwitchHandlerAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(IAmABulkOutboxAsync<>), new()
					{
						{ "T", "global::Paramore.Brighter.Message" },
					}
				},
				{
					typeof(IAmABulkOutboxSync<>), new()
					{
						{ "T", "global::Paramore.Brighter.Message" },
					}
				},
				{
					typeof(IAmAnOutbox<>), new()
					{
						{ "T", "global::Paramore.Brighter.Message" },
					}
				},
				{
					typeof(IAmAnOutboxAsync<>), new()
					{
						{ "T", "global::Paramore.Brighter.Message" },
					}
				},
				{
					typeof(IAmAnOutboxSync<>), new()
					{
						{ "T", "global::Paramore.Brighter.Message" },
					}
				},
				{
					typeof(IAmAMessageMapper<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(IHandleRequests<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(IHandleRequestsAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(InMemoryBox<>), new()
					{
						{ "T", "global::Paramore.Brighter.IHaveABoxWriteTime" },
					}
				},
				{
					typeof(MonitorHandler<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(MonitorHandlerAsync<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(PipelineBuilder<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(Pipelines<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(RequestLoggingHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(RequestLoggingHandlerAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(RequestHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(RequestHandlerAsync<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(RequestNotFoundException<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(Subscription<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(TimeoutPolicyHandler<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(TransformPipeline<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(UnwrapPipeline<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(UseInboxHandler<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(UseInboxHandlerAsync<>), new()
					{
						{ "T", "global::Paramore.Brighter.IRequest" },
					}
				},
				{
					typeof(WrapPipeline<>), new()
					{
						{ "TRequest", "global::Paramore.Brighter.IRequest" },
					}
				},
			};
#pragma warning restore CS0618 // Type or member is obsolete
	}
}