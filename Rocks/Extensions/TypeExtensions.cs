using System;
using System.Collections.Generic;

namespace Rocks.Extensions
{
	internal static class TypeExtensions
	{
		internal static string Validate(this Type @this)
		{
			if(@this.IsSealed)
			{
				return string.Format(Constants.ErrorMessages.CannotMockSealedType, @this.Name);
			}

			// TODO: Does this type have any virtual members that could be overridden?

			return string.Empty;
		}

		internal static string GetImplementedProperties(this Type @this, SortedSet<string> namespaces)
		{
			var properties = new List<string>();

			foreach(var property in @this.GetProperties())
			{
				var accessors = new List<string>();

				if(property.CanRead)
				{
					accessors.Add("get;");
				}

				if (property.CanWrite)
				{
					accessors.Add("set;");
				}

				namespaces.Add(property.PropertyType.Namespace);

				if (property.Name == "Item")
				{
					var indexerParameter = property.CanRead ? property.GetMethod.GetParameters()[0] :
						property.SetMethod.GetParameters()[0];
					namespaces.Add(indexerParameter.ParameterType.Namespace);

					// Indexer
					properties.Add(string.Format(Constants.CodeTemplates.PropertyIndexerTemplate,
						property.PropertyType.Name, indexerParameter.ParameterType.Name, indexerParameter.Name,
						string.Join(" ", accessors)));
				}
				else
				{
					// Normal
					properties.Add(string.Format(Constants.CodeTemplates.PropertyTemplate,
						property.PropertyType.Name, property.Name, 
						string.Join(" ", accessors)));
				}
			}

			return string.Join(Environment.NewLine, properties);
		}

		internal static string GetImplementedEvents(this Type @this, SortedSet<string> namespaces)
		{
			var events = new List<string>();

			foreach(var @event in @this.GetEvents())
			{
				var eventHandlerType = @event.EventHandlerType;
				namespaces.Add(eventHandlerType.Namespace);

				if (eventHandlerType.IsGenericType)
				{
					var eventGenericType = eventHandlerType.GetGenericArguments()[0];
               events.Add(string.Format(Constants.CodeTemplates.EventTemplate,
						$"EventHandler<{eventGenericType.Name}>", @event.Name));
					namespaces.Add(eventGenericType.Namespace);
				}
				else
				{
					events.Add(string.Format(Constants.CodeTemplates.EventTemplate,
						eventHandlerType.Name, @event.Name));
				}
			}

			return string.Join(Environment.NewLine, events);
		}
	}
}
