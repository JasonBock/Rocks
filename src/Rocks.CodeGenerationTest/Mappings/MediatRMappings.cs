using MediatR;
using MediatR.Pipeline;
using MediatR.Wrappers;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class MediatRMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AsyncRequestExceptionAction<>), new()
					{
						{ "TRequest", "global::MediatR.IRequest" },
					}
				},
				{
					typeof(AsyncRequestExceptionHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(INotificationHandler<>), new()
					{
						{ "TNotification", "global::Rocks.CodeGenerationTest.Mappings.MediatR.MappedNotification" },
					}
				},
				{
					typeof(IPipelineBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IRequestExceptionAction<,>), new()
					{
						{ "TRequest", "global::System.Object" },
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(IRequestExceptionHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IRequestExceptionHandler<,,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(IRequestHandler<>), new()
					{
						{ "TRequest", "global::MediatR.IRequest" },
					}
				},
				{
					typeof(IRequestHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IRequestPostProcessor<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IStreamPipelineBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IStreamRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(IStreamRequestHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IStreamRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(NotificationHandler<>), new()
					{
						{ "TNotification", "global::MediatR.INotification" },
					}
				},
				{
					typeof(NotificationHandlerWrapperImpl<>), new()
					{
						{ "TNotification", "global::MediatR.INotification" },
					}
				},
				{
					typeof(RequestExceptionActionProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestExceptionAction<,>), new()
					{
						{ "TRequest", "global::System.Object" },
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(RequestExceptionHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestExceptionHandler<,,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(RequestExceptionProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestHandlerWrapperImpl<>), new()
					{
						{ "TRequest", "global::MediatR.IRequest" },
					}
				},
				{
					typeof(RequestHandlerWrapperImpl<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestPreProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
				{
					typeof(RequestPostProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<global::System.Object>" },
						{ "TResponse", "global::System.Object" },
					}
				},
			};
	}

	namespace MediatR
	{
		public sealed class MappedNotification
			: INotification
		{ }
	}
}