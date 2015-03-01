using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class TypeExtensions
	{
		internal static string Validate(this Type @this)
		{
			if(@this.IsSealed)
			{
				return string.Format(Constants.ErrorMessages.CannotMockSealedType, @this.GetSafeName());
			}

			if(!@this.GetMembers(Constants.Reflection.PublicInstance).Any())
			{
				return string.Format(Constants.ErrorMessages.NoVirtualMembers, @this.GetSafeName());
			}

			return string.Empty;
		}

		internal static bool ContainsRefAndOrOutParameters(this Type @this)
		{
			return (from method in @this.GetMethods(Constants.Reflection.PublicInstance)
					  where method.ContainsRefAndOrOutParameters()
					  select method).Any();
		}

		internal static string GetSafeName(this Type @this)
		{
			return @this.GetSafeName(null, null);
		}

		internal static string GetSafeName(this Type @this, MethodBase context, SortedSet<string> namespaces)
		{
			// The context should come from the method the delegate is providing a hook to
			if(typeof(MulticastDelegate).IsAssignableFrom(@this.BaseType) && @this.IsGenericType)
			{
				var arguments = context != null ? context.GetGenericArguments(namespaces).Arguments :
					$"<{string.Join(", ", @this.GetGenericArguments().Select(_ => _.GetSafeName()))}>";
				return $"{@this.FullName.Split('`')[0].Split('.').Last().Replace("+", ".")}{arguments}";
         }
			else
			{
				return !string.IsNullOrWhiteSpace(@this.FullName) ?
					@this.FullName.Split('.').Last().Replace("+", ".") :
					@this.Name;
			}
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
						property.PropertyType.Name, indexerParameter.ParameterType.GetSafeName(), indexerParameter.Name,
						string.Join(" ", accessors)));
				}
				else
				{
					// Normal
					properties.Add(string.Format(Constants.CodeTemplates.PropertyTemplate,
						property.PropertyType.GetSafeName(), property.Name, 
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
						$"EventHandler<{eventGenericType.GetSafeName()}>", @event.Name));
					namespaces.Add(eventGenericType.Namespace);
				}
				else
				{
					events.Add(string.Format(Constants.CodeTemplates.EventTemplate,
						eventHandlerType.GetSafeName(), @event.Name));
				}
			}

			return string.Join(Environment.NewLine, events);
		}

		internal static string GetConstraints(this Type @this, SortedSet<string> namespaces)
		{
			var constraints = @this.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
			var constraintedTypes = @this.GetGenericParameterConstraints();

			if (constraints == GenericParameterAttributes.None && constraintedTypes.Length == 0)
			{
				return string.Empty;
			}
			else
			{
				var constraintValues = new List<string>();

				if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
				{
					constraintValues.Add("struct");
				}
				else
				{
					foreach (var constraintedType in constraintedTypes.OrderBy(_ => _.IsClass ? 0 : 1))
					{
						constraintValues.Add(constraintedType.GetSafeName());
						namespaces.Add(constraintedType.Namespace);
					}

					if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
					{
						constraintValues.Add("class");
					}
					if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
					{
						constraintValues.Add("new()");
					}
				}

				return $"where {@this.GetSafeName()} : {string.Join(", ", constraintValues)}";
			}
		}
	}
}
