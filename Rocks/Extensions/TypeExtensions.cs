using Rocks.Construction;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rocks.Extensions
{
	internal static class TypeExtensions
	{
		internal static ReadOnlyCollection<MethodInfo> GetMockableMethods(this Type @this)
		{
			var methods = new HashSet<MethodInfo>(@this.GetMethods(ReflectionValues.PublicNonPublicInstance)
				.Where(_ => !_.IsSpecialName && _.IsVirtual && !_.IsFinal));

			if (@this.IsInterface)
			{
				var namespaces = new SortedSet<string>();

				foreach (var @interface in @this.GetInterfaces())
				{
					var interfaceMethods = @interface.GetMockableMethods();
					var methodDescriptions = methods.Select(_ => _.GetMethodDescription());

					foreach (var interfaceMethod in interfaceMethods)
					{
						if (!methodDescriptions.Where(_ => _.Equals(interfaceMethod.GetMethodDescription())).Any())
						{
							methods.Add(interfaceMethod);
						}
					}
				}
			}

			return methods.ToList().AsReadOnly();
		}

		internal static ReadOnlyCollection<PropertyInfo> GetMockableProperties(this Type @this)
		{
			var properties = new HashSet<PropertyInfo>(@this.GetProperties(ReflectionValues.PublicNonPublicInstance)
				.Where(_ => _.GetDefaultMethod().IsVirtual && !_.GetDefaultMethod().IsFinal));

			if (@this.IsInterface)
			{
				foreach (var @interface in @this.GetInterfaces())
				{
					properties.UnionWith(@interface.GetMockableProperties());
				}
			}

			return properties.ToList().AsReadOnly();
		}

		internal static ReadOnlyCollection<EventInfo> GetMockableEvents(this Type @this)
		{
			var events = new HashSet<EventInfo>(@this.GetEvents(ReflectionValues.PublicNonPublicInstance)
				.Where(_ => _.AddMethod.IsVirtual && !_.AddMethod.IsFinal));

			if (@this.IsInterface)
			{
				foreach (var @interface in @this.GetInterfaces())
				{
					events.UnionWith(@interface.GetMockableEvents());
				}
			}

			return events.ToList().AsReadOnly();
		}

		internal static MethodInfo FindMethod(this Type @this, int methodHandle)
		{
			var foundMethod = (from method in @this.GetMethods()
									 where method.MetadataToken == methodHandle
									 select method).FirstOrDefault();

			if (foundMethod == null)
			{
				var types = new List<Type>(@this.GetInterfaces());

				if (@this.BaseType != null)
				{
					types.Add(@this.BaseType);
				}

				return (from type in types
						  let baseMethod = type.FindMethod(methodHandle)
						  where baseMethod != null
						  select baseMethod).FirstOrDefault();
			}
			else
			{
				return foundMethod;
			}
		}

		internal static PropertyInfo FindProperty(this Type @this, string name)
		{
			var property = @this.GetProperty(name);

			if (property == null)
			{
				throw new PropertyNotFoundException($"Property {name} on type {@this.Name} was not found.");
			}

			return property;
		}

		internal static PropertyInfo FindProperty(this Type @this, string name, PropertyAccessors accessors)
		{
			var property = @this.FindProperty(name);
			TypeExtensions.CheckPropertyAccessors(property, accessors);
			return property;
		}

		private static void CheckPropertyAccessors(PropertyInfo property, PropertyAccessors accessors)
		{
			if (accessors == PropertyAccessors.Get || accessors == PropertyAccessors.GetAndSet)
			{
				if (!property.CanRead)
				{
					throw new PropertyNotFoundException($"Property {property.Name} on type {property.DeclaringType.Name} cannot be read from.");
				}
			}

			if (accessors == PropertyAccessors.Set || accessors == PropertyAccessors.GetAndSet)
			{
				if (!property.CanWrite)
				{
					throw new PropertyNotFoundException($"Property {property.Name} on type {property.DeclaringType.Name} cannot be written to.");
				}
			}
		}

		internal static PropertyInfo FindProperty(this Type @this, Type[] indexers)
		{
			var property = (from p in @this.GetProperties()
								 where p.GetIndexParameters().Any()
								 let pTypes = p.GetIndexParameters().Select(pi => pi.ParameterType).ToArray()
								 where ObjectEquality.AreEqual(pTypes, indexers)
								 select p).SingleOrDefault();

			if (property == null)
			{
				throw new PropertyNotFoundException($"Indexer on type {@this.Name} with argument types [{string.Join(", ", indexers.Select(_ => _.Name))}] was not found.");
			}

			return property;
		}

		internal static PropertyInfo FindProperty(this Type @this, Type[] indexers, PropertyAccessors accessors)
		{
			var property = @this.FindProperty(indexers);
			TypeExtensions.CheckPropertyAccessors(property, accessors);
			return property;
		}

		internal static bool IsUnsafeToMock(this Type @this)
		{
			return @this.IsPointer ||
				@this.GetMethods(ReflectionValues.PublicNonPublicInstance).Where(m => m.IsUnsafeToMock()).Any() ||
				@this.GetProperties(ReflectionValues.PublicNonPublicInstance).Where(p => p.GetDefaultMethod().IsUnsafeToMock(false)).Any() ||
				@this.GetEvents(ReflectionValues.PublicNonPublicInstance).Where(e => e.AddMethod.IsUnsafeToMock(false)).Any();
		}

		internal static string Validate(this Type @this)
		{
			if (@this.IsSealed && @this.GetConstructor(new[] { typeof(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>) }) == null)
			{
				return ErrorMessages.GetCannotMockSealedType(@this.GetSafeName());
			}

			if (@this.IsAbstract &&
				(@this.GetMethods(ReflectionValues.NonPublicInstance).Where(_ => _.IsAssembly && _.IsAbstract).Any() ||
				@this.GetProperties(ReflectionValues.NonPublicInstance).Where(_ => _.GetDefaultMethod().IsAssembly && _.GetDefaultMethod().IsAbstract).Any() ||
				@this.GetEvents(ReflectionValues.NonPublicInstance).Where(_ => _.AddMethod.IsAssembly && _.AddMethod.IsAbstract).Any()))
			{
				if (!@this.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
					.Where(_ => _.AssemblyName == (@this.IsSealed ? new AssemblyNameGenerator(@this).AssemblyName : new InMemoryNameGenerator().AssemblyName)).Any())
				{
					return ErrorMessages.GetCannotMockTypeWithInternalAbstractMembers(@this.GetSafeName());
				}
			}

			return string.Empty;
		}

		internal static bool ContainsRefAndOrOutParameters(this Type @this)
		{
			return (from method in @this.GetMethods(ReflectionValues.PublicInstance)
					  where method.ContainsRefAndOrOutParameters()
					  select method).Any();
		}

		internal static string GetSafeName(this Type @this)
		{
			return @this.GetSafeName(null, null);
		}

		internal static string GetSafeName(this Type @this, MethodBase context, SortedSet<string> namespaces)
		{
			var name = !string.IsNullOrWhiteSpace(@this.FullName) ?
				@this.FullName.Split('`')[0].Split('.').Last().Replace("+", ".") :
				@this.Name.Split('`')[0];

			return name;
		}

		internal static GenericArgumentsResult GetGenericArguments(this Type @this, SortedSet<string> namespaces)
		{
			var arguments = string.Empty;
			var constraints = string.Empty;

			if (@this.IsGenericType)
			{
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach (var argument in @this.GetGenericArguments())
				{
					genericArguments.Add(argument.GetSafeName());
					var constraint = argument.GetConstraints(namespaces);

					if (!string.IsNullOrWhiteSpace(constraint))
					{
						genericConstraints.Add(constraint);
					}
				}

				arguments = $"<{string.Join(", ", genericArguments)}>";
				// TODO: This should not add a space in front. The Maker class
				// should adjust the constraints to have a space in front.
				constraints = genericConstraints.Count == 0 ?
					string.Empty : $"{string.Join(" ", genericConstraints)}";
			}

			return new GenericArgumentsResult(arguments, constraints);
		}

		internal static string GetConstraints(this Type @this, SortedSet<string> namespaces)
		{
			if (@this.IsGenericParameter)
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
			else
			{
				return string.Empty;
			}
		}
	}
}
