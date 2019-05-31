using Rocks.Extensions;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;
using static System.Reflection.CustomAttributeExtensions;

namespace Rocks.Construction.Generators
{
	internal static class EventsGenerator
	{
		internal static GenerateResults Generate(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder builder)
		{
			var requiresObsoleteSuppression = false;
			var generatedEvents = new List<string>();

			foreach (var @event in baseType.GetMockableEvents(generator))
			{
				var eventHandlerType = @event.EventHandlerType;
				var eventHandlerTypeInfo = eventHandlerType;

				namespaces.Add(eventHandlerType.Namespace);

				var eventMethod = @event.AddMethod;

				var methodInformation = builder.Build(new MockableResult<MethodInfo>(
					eventMethod, RequiresExplicitInterfaceImplementation.No));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (eventMethod.IsPublic)
				{
					generatedEvents.Add(EventTemplates.GetEvent(@override,
						eventHandlerType.GetFullName(namespaces), @event.Name));
				}
				else if (!eventMethod.IsPrivate && eventMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(eventMethod.IsFamily, eventMethod.IsFamilyOrAssembly);

					generatedEvents.Add(EventTemplates.GetNonPublicEvent(visibility,
						eventHandlerType.GetFullName(namespaces), @event.Name));
				}

				requiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
			}

			var result = generatedEvents.Count > 0 ? EventTemplates.GetEvents(generatedEvents.AsReadOnly()) : string.Empty;
			return new GenerateResults(result, requiresObsoleteSuppression, false);
		}
	}
}