using Rocks.Construction;
using Rocks.Exceptions;
using Rocks.Options;
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
		internal static string GetAttributes(this Type @this) =>
			@this.GetAttributes(false, new SortedSet<string>());

		internal static string GetAttributes(this Type @this, bool isReturn) =>
			@this.GetAttributes(isReturn, new SortedSet<string>());

		internal static string GetAttributes(this Type @this, SortedSet<string> namespaces) =>
			@this.GetAttributes(false, namespaces);

		internal static string GetAttributes(this Type @this, bool isReturn, SortedSet<string> namespaces)
		{
#if !NETCOREAPP1_1
			var attributeData = @this.GetCustomAttributesData();
#else
			var attributeData = @this.GetTypeInfo().CustomAttributes.ToList();
#endif
			return attributeData.GetAttributes(isReturn, namespaces, null);
		}

		internal static bool RequiresExplicitCast(this Type @this)
		{
			var thisTypeInfo = @this.GetTypeInfo();
			return thisTypeInfo.IsValueType ||
				(@this.IsGenericParameter && (thisTypeInfo.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == 0);
		}

		internal static bool AddNamespaces(this Type @this, SortedSet<string> namespaces)
		{
			namespaces.Add(@this.Namespace);

			if (@this.GetTypeInfo().IsGenericType)
			{
				foreach (var genericType in @this.GetGenericArguments())
				{
					genericType.AddNamespaces(namespaces);
				}
			}

			return true;
		}

		internal static string GetFullName(this Type @this) =>
			@this.GetFullName(new SortedSet<string>());

		internal static string GetFullName(this Type @this, SortedSet<string> namespaces)
		{
			var dissector = new TypeDissector(@this);

			var pointer = dissector.IsPointer ? "*" : string.Empty;
			var array = dissector.IsArray ? "[]" : string.Empty;

			return $"{dissector.SafeName}{dissector.RootType.GetGenericArguments(namespaces).Arguments}{pointer}{array}";
		}

		internal static ReadOnlyCollection<MockableResult<ConstructorInfo>> GetMockableConstructors(this Type @this, NameGenerator generator) =>
			new ReadOnlyCollection<MockableResult<ConstructorInfo>>(
				@this.GetConstructors(ReflectionValues.PublicNonPublicInstance)
					.Where(_ => !_.IsPrivate &&
						(_.GetCustomAttribute<ObsoleteAttribute>() == null || !_.GetCustomAttribute<ObsoleteAttribute>().IsError) &&
						_.DeclaringType.GetTypeInfo().Assembly.CanBeSeenByMockAssembly(_.IsPublic, false, _.IsFamily, _.IsFamilyOrAssembly, generator) &&
						!_.GetParameters().Where(p => !p.ParameterType.CanBeSeenByMockAssembly(generator)).Any())
					.Select(_ => new MockableResult<ConstructorInfo>(_, RequiresExplicitInterfaceImplementation.No)).ToList());

		internal static ReadOnlyCollection<MethodMockableResult> GetMockableMethods(this Type @this, NameGenerator generator)
		{
			var thisTypeInfo = @this.GetTypeInfo();

			var objectMethods = thisTypeInfo.IsInterface ?
				typeof(object).GetMethods().Where(_ => _.IsExtern() || _.IsVirtual).ToList() : new List<MethodInfo>();

			var methods = new HashSet<MockableResult<MethodInfo>>(@this.GetMethods(ReflectionValues.PublicNonPublicInstance)
				.Where(_ => !_.IsSpecialName && _.IsVirtual && !_.IsFinal &&
					!objectMethods.Where(om => om.Match(_) == MethodMatch.Exact).Any() &&
					_.DeclaringType.GetTypeInfo().Assembly.CanBeSeenByMockAssembly(_.IsPublic, _.IsPrivate, _.IsFamily, _.IsFamilyOrAssembly, generator))
				.Select(_ => new MockableResult<MethodInfo>(_, RequiresExplicitInterfaceImplementation.No)));

			if (thisTypeInfo.IsInterface)
			{
				var namespaces = new SortedSet<string>();

				foreach (var @interface in @this.GetInterfaces())
				{
					var interfaceMethods = @interface.GetMethods()
						.Where(_ => !_.IsSpecialName && !objectMethods.Where(om => om.Match(_) == MethodMatch.Exact).Any());

					foreach (var interfaceMethod in interfaceMethods)
					{
						if (interfaceMethod.CanBeSeenByMockAssembly(generator))
						{
							var matchMethodGroups = methods.GroupBy(_ => interfaceMethod.Match(_.Value)).ToDictionary(_ => _.Key);

							if (!matchMethodGroups.ContainsKey(MethodMatch.Exact))
							{
								methods.Add(new MockableResult<MethodInfo>(
									interfaceMethod, matchMethodGroups.ContainsKey(MethodMatch.DifferByReturnTypeOnly) ?
										RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No));
							}
						}
					}
				}
			}

			var baseStaticMethods = thisTypeInfo.IsInterface ?
				typeof(object).GetMethods().Where(_ => _.IsStatic).ToList() :
				@this.GetMethods().Where(_ => _.IsStatic).ToList();

			return methods.Select(_ => new MethodMockableResult(
				_.Value, _.RequiresExplicitInterfaceImplementation,
				baseStaticMethods.Where(osm => osm.Match(_.Value) == MethodMatch.Exact).Any() ?
					RequiresIsNewImplementation.Yes : RequiresIsNewImplementation.No)).ToList().AsReadOnly();
		}

		internal static ReadOnlyCollection<PropertyMockableResult> GetMockableProperties(this Type @this, NameGenerator generator)
		{
			var properties = new HashSet<PropertyMockableResult>(
				from property in @this.GetProperties(ReflectionValues.PublicNonPublicInstance)
				let canGet = property.CanRead && property.GetMethod.IsVirtual && !property.GetMethod.IsFinal &&
					property.GetMethod.DeclaringType.GetTypeInfo().Assembly.CanBeSeenByMockAssembly(
					property.GetMethod.IsPublic, property.GetMethod.IsPrivate, property.GetMethod.IsFamily, property.GetMethod.IsFamilyOrAssembly, generator)
				let canSet = property.CanWrite && property.SetMethod.IsVirtual && !property.SetMethod.IsFinal &&
					property.SetMethod.DeclaringType.GetTypeInfo().Assembly.CanBeSeenByMockAssembly(
					property.SetMethod.IsPublic, property.SetMethod.IsPrivate, property.SetMethod.IsFamily, property.SetMethod.IsFamilyOrAssembly, generator)
				where canGet || canSet
				select new PropertyMockableResult(property, RequiresExplicitInterfaceImplementation.No,
					(canGet && canSet ? PropertyAccessors.GetAndSet : (canGet ? PropertyAccessors.Get : PropertyAccessors.Set))));

			if (@this.GetTypeInfo().IsInterface)
			{
				var namespaces = new SortedSet<string>();

				foreach (var @interface in @this.GetInterfaces())
				{
					foreach (var interfaceProperty in @interface.GetMockableProperties(generator))
					{
						if (interfaceProperty.Value.GetDefaultMethod().CanBeSeenByMockAssembly(generator))
						{
							var matchMethodGroups = properties.GroupBy(_ => interfaceProperty.Value.GetDefaultMethod().Match(_.Value.GetDefaultMethod())).ToDictionary(_ => _.Key);

							if (!matchMethodGroups.ContainsKey(MethodMatch.Exact))
							{
								properties.Add(new PropertyMockableResult(interfaceProperty.Value,
									matchMethodGroups.ContainsKey(MethodMatch.DifferByReturnTypeOnly) ?
										RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No,
									interfaceProperty.Accessors));
							}
						}
					}
				}
			}

			return properties.ToList().AsReadOnly();
		}

		internal static ReadOnlyCollection<EventInfo> GetMockableEvents(this Type @this, NameGenerator generator) =>
			new HashSet<EventInfo>(
				from type in @this.GetTypeHierarchy(
					@this.GetTypeInfo().IsInterface ? IncludeInterfaces.Yes : IncludeInterfaces.No, IncludeBaseTypes.No)
				let typeEvents = type.GetEvents(ReflectionValues.PublicNonPublicInstance)
				from typeEvent in typeEvents
				where typeEvent.AddMethod.IsVirtual && !typeEvent.AddMethod.IsFinal && typeEvent.AddMethod.CanBeSeenByMockAssembly(generator)
				select typeEvent).ToList().AsReadOnly();

		internal static MethodInfo FindMethod(this Type @this, int methodHandle) =>
			(from type in @this.GetTypeHierarchy(IncludeInterfaces.Yes, IncludeBaseTypes.Yes)
			 from method in type.GetMethods()
			 where method.MetadataToken == methodHandle
			 select method).FirstOrDefault();

		internal static PropertyInfo FindProperty(this Type @this, string name) =>
			(from type in @this.GetTypeHierarchy(IncludeInterfaces.Yes, IncludeBaseTypes.Yes)
			 let baseProperty = type.GetProperty(name)
			 where baseProperty != null
			 select baseProperty).FirstOrDefault() ?? throw new PropertyNotFoundException($"Property {name} on type {@this.Name} was not found.");

		private static List<Type> GetTypeHierarchy(this Type @this, IncludeInterfaces includeInterfaces, IncludeBaseTypes includeBaseTypes)
		{
			var types = new List<Type> { @this };

			if(includeInterfaces == IncludeInterfaces.Yes)
			{
				types.AddRange(@this.GetInterfaces());
			}

			if (includeBaseTypes == IncludeBaseTypes.Yes)
			{
				var baseType = @this.GetTypeInfo().BaseType;

				while (baseType != null)
				{
					types.Add(baseType);
					baseType = baseType.GetTypeInfo().BaseType;
				}
			}

			return types;
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
			var types = new List<Type> { @this };
			types.AddRange(@this.GetInterfaces());

			var baseType = @this.GetTypeInfo().BaseType;

			while (baseType != null)
			{
				types.Add(baseType);
				baseType = baseType.GetTypeInfo().BaseType;
			}

			var property = (
				from type in types
				from p in type.GetProperties()
				where p.GetIndexParameters().Any()
				let pTypes = p.GetIndexParameters().Select(pi => pi.ParameterType).ToArray()
				where ObjectEquality.AreEqual(pTypes, indexers)
				select p).FirstOrDefault();

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

		internal static bool IsUnsafeToMock(this Type @this) =>
			@this.IsPointer ||
				@this.GetMethods(ReflectionValues.PublicNonPublicInstance).Where(m => m.IsUnsafeToMock()).Any() ||
				@this.GetProperties(ReflectionValues.PublicNonPublicInstance).Where(p => p.GetDefaultMethod().IsUnsafeToMock(false)).Any() ||
				@this.GetEvents(ReflectionValues.PublicNonPublicInstance).Where(e => e.AddMethod.IsUnsafeToMock(false)).Any();

#if !NETCOREAPP1_1
		internal static string Validate(this Type @this, SerializationOptions options, NameGenerator generator)
#else
		internal static string Validate(this Type @this, NameGenerator generator)
#endif
		{
			var thisTypeInfo = @this.GetTypeInfo();

			if (thisTypeInfo.IsSealed && !@this.GetConstructors()
				.Where(_ => _.GetParameters().Length == 1 &&
					typeof(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>)
					.IsAssignableFrom(_.GetParameters()[0].ParameterType)).Any())
			{
				return ErrorMessages.GetCannotMockSealedType(new TypeDissector(@this).SafeName);
			}

			if (thisTypeInfo.GetCustomAttribute<ObsoleteAttribute>()?.IsError ?? false)
			{
				return ErrorMessages.GetCannotMockObsoleteType(new TypeDissector(@this).SafeName);
			}

#if !NETCOREAPP1_1
			if (options == SerializationOptions.Supported && !@this.IsInterface &&
				@this.GetConstructor(Type.EmptyTypes) == null)
			{
				return ErrorMessages.GetCannotMockTypeWithSerializationRequestedAndNoPublicNoArgumentConstructor(
					new TypeDissector(@this).SafeName);
			}
#endif
			if (thisTypeInfo.IsAbstract &&
				(thisTypeInfo.GetConstructors(ReflectionValues.NonPublicInstance).Where(_ => _.IsAssembly).Any() ||
				thisTypeInfo.GetMethods(ReflectionValues.NonPublicInstance).Where(_ => _.IsAssembly && _.IsAbstract).Any() ||
				thisTypeInfo.GetProperties(ReflectionValues.NonPublicInstance).Where(_ => _.GetDefaultMethod().IsAssembly && _.GetDefaultMethod().IsAbstract).Any() ||
				thisTypeInfo.GetEvents(ReflectionValues.NonPublicInstance).Where(_ => _.AddMethod.IsAssembly && _.AddMethod.IsAbstract).Any()))
			{
				if (!thisTypeInfo.Assembly.CanBeSeenByMockAssembly(false, false, false, false, generator))
				{
					return ErrorMessages.GetCannotMockTypeWithInternalAbstractMembers(new TypeDissector(@this).SafeName);
				}
			}

			if (!thisTypeInfo.IsInterface && @this.GetMockableConstructors(generator).Count == 0)
			{
				return ErrorMessages.GetCannotMockTypeWithNoAccessibleConstructors(new TypeDissector(@this).SafeName);
			}

			return string.Empty;
		}
#if NETCOREAPP1_1
		internal static ConstructorInfo FindConstructor(this Type @this, BindingFlags flags, Type[] types)
		{
			var constructors = @this.GetMembers(flags).OfType<ConstructorInfo>().ToArray();

			foreach(var constructor in constructors)
			{
				var parameters = constructor.GetParameters();

				if(parameters.Length == types.Length)
				{
					var parametersExact = true;

					for(var i = 0; i < parameters.Length; i++)
					{
						parametersExact &= parameters[i].ParameterType == types[i];
					}

					if(parametersExact)
					{
						return constructor;
					}
				}
			}

			foreach (var constructor in constructors)
			{
				var parameters = constructor.GetParameters();

				if (parameters.Length == types.Length)
				{
					var parametersAssignable = true;

					for (var i = 0; i < parameters.Length; i++)
					{
						parametersAssignable &= parameters[i].ParameterType.IsAssignableFrom(types[i]);
					}

					if (parametersAssignable)
					{
						return constructor;
					}
				}
			}

			return null;
		}
#endif

		internal static Type GetRootElementType(this Type @this)
		{
			var root = @this;

			while (root.HasElementType)
			{
				root = root.GetElementType();
			}

			return root;
		}

		internal static bool CanBeSeenByMockAssembly(this Type @this, NameGenerator generator)
		{
			var root = @this.GetRootElementType();
			return root.GetTypeInfo().IsPublic || (root.GetTypeInfo().Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
				.Where(_ => _.AssemblyName == generator.AssemblyName).Any());
		}

		internal static bool ContainsRefAndOrOutParameters(this Type @this) =>
			(from method in @this.GetMethods(ReflectionValues.PublicInstance)
			 where method.ContainsDelegateConditions()
			 select method).Any();

		internal static GenericArgumentsResult GetGenericArguments(this Type @this, SortedSet<string> namespaces)
		{
			var arguments = string.Empty;
			var constraints = string.Empty;

			if (@this.GetTypeInfo().IsGenericType)
			{
				var genericArguments = new List<string>();
				var genericConstraints = new List<string>();

				foreach (var argument in @this.GetGenericArguments())
				{
					genericArguments.Add($"{argument.GetFullName(namespaces)}");
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
				var thisTypeInfo = @this.GetTypeInfo();

				var constraints = thisTypeInfo.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
				var constraintedTypes = thisTypeInfo.GetGenericParameterConstraints();

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
						foreach (var constraintedType in constraintedTypes.OrderBy(_ => _.GetTypeInfo().IsClass ? 0 : 1))
						{
							constraintValues.Add(new TypeDissector(constraintedType).SafeName);
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

					return $"where {new TypeDissector(@this).SafeName} : {string.Join(", ", constraintValues)}";
				}
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
