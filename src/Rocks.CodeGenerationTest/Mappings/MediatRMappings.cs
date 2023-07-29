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
					typeof(INotificationHandler<>), new()
					{
						{ "TNotification", "global::MediatR.INotification" },
					}
				},
				{
					typeof(IPipelineBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(IRequestExceptionAction<,>), new()
					{
						{ "TRequest", "object" },
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(IRequestExceptionHandler<,,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
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
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(IRequestPostProcessor<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(IStreamPipelineBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IStreamRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(IStreamRequestHandler<,>), new()
					{
						{ "TRequest", "global::MediatR.IStreamRequest<object>" },
						{ "TResponse", "object" },
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
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(RequestExceptionProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
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
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(RequestPreProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
				{
					typeof(RequestPostProcessorBehavior<,>), new()
					{
						{ "TRequest", "global::MediatR.IRequest<object>" },
						{ "TResponse", "object" },
					}
				},
			};
	}
}