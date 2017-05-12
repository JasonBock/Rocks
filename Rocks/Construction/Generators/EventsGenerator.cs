using Rocks.Extensions;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;
using static System.Reflection.CustomAttributeExtensions;

namespace Rocks.Construction.Generators
{
	internal sealed class EventsGenerator
	{
		internal GenerateResults Generate(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder builder)
		{
			var requiresObsoleteSuppression = false;
			var generatedEvents = new List<string>();

			foreach (var @event in baseType.GetMockableEvents(generator))
			{
				var eventHandlerType = @event.EventHandlerType;
				var eventHandlerTypeInfo = eventHandlerType.GetTypeInfo();

				namespaces.Add(eventHandlerType.Namespace);

				var eventMethod = @event.AddMethod;

				var methodInformation = builder.Build(new MockableResult<MethodInfo>(
					eventMethod, RequiresExplicitInterfaceImplementation.No));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (eventMethod.IsPublic)
				{
					if (eventHandlerTypeInfo.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(EventTemplates.GetEvent(@override,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(EventTemplates.GetEvent(@override,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					requiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!eventMethod.IsPrivate && eventMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(eventMethod.IsFamily, eventMethod.IsFamilyOrAssembly);

					if (eventHandlerTypeInfo.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(EventTemplates.GetNonPublicEvent(visibility,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(EventTemplates.GetNonPublicEvent(visibility,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					requiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			var result = generatedEvents.Count > 0 ? EventTemplates.GetEvents(generatedEvents.AsReadOnly()) : string.Empty;
			return new GenerateResults(result, requiresObsoleteSuppression, false);
		}
	}
}
