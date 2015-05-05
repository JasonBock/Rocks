using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static System.Reflection.CustomAttributeExtensions;
using static Rocks.Extensions.TypeExtensions;
using System.Reflection;
using Rocks.Extensions;

namespace Rocks.Construction
{
	internal sealed class EventsBuilder
	{
		internal EventsBuilder(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder builder)
		{
			this.Events = this.GetGeneratedEvents(baseType, namespaces, generator, builder);
		}

		private ReadOnlyCollection<string> GetGeneratedEvents(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder builder)
		{
			var generatedEvents = new List<string>();

			foreach (var @event in baseType.GetMockableEvents(generator))
			{
				var eventHandlerType = @event.EventHandlerType;
				namespaces.Add(eventHandlerType.Namespace);

				var eventMethod = @event.AddMethod;

				var methodInformation = builder.Build(new MockableResult<MethodInfo>(
					eventMethod, RequiresExplicitInterfaceImplementation.No));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (eventMethod.IsPublic)
				{
					if (eventHandlerType.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(CodeTemplates.GetEventTemplate(@override,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(CodeTemplates.GetEventTemplate(@override,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					this.RequiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!eventMethod.IsPrivate && eventMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(eventMethod.IsFamily, eventMethod.IsFamilyOrAssembly);

					if (eventHandlerType.IsGenericType)
					{
						var eventGenericType = eventHandlerType.GetGenericArguments()[0];
						generatedEvents.Add(CodeTemplates.GetNonPublicEventTemplate(visibility,
							$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
						namespaces.Add(eventGenericType.Namespace);
					}
					else
					{
						generatedEvents.Add(CodeTemplates.GetNonPublicEventTemplate(visibility,
							eventHandlerType.GetSafeName(), @event.Name));
					}

					this.RequiresObsoleteSuppression |= @event.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedEvents.AsReadOnly();
		}

		internal ReadOnlyCollection<string> Events { get; }
		internal bool RequiresObsoleteSuppression { get; private set; }
   }
}
