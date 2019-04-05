using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class PropertyInfoExtensions
	{
		internal static void CheckPropertyAccessors(this PropertyInfo @this, PropertyAccessors accessors)
		{
			if (accessors == PropertyAccessors.Get || accessors == PropertyAccessors.GetAndSet)
			{
				if (!@this.CanRead)
				{
					throw new PropertyNotFoundException($"Property {@this.Name} on type {@this.DeclaringType.Name} cannot be read from.");
				}
			}

			if (accessors == PropertyAccessors.Set || accessors == PropertyAccessors.GetAndSet)
			{
				if (!@this.CanWrite)
				{
					throw new PropertyNotFoundException($"Property {@this.Name} on type {@this.DeclaringType.Name} cannot be written to.");
				}
			}
		}

		internal static MethodInfo GetDefaultMethod(this PropertyInfo @this) =>
			@this.CanRead ? @this.GetMethod : @this.SetMethod;

		private static ReadOnlyDictionary<string, ArgumentExpectation> CreateEmptyExpectations() =>
			new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());

		private static Dictionary<string, ArgumentExpectation> GetIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var expectations = new Dictionary<string, ArgumentExpectation>();
			var propertyIndexers = @this.GetIndexParameters();

			for (var i = 0; i < indexers.Count; i++)
			{
				var propertyIndexer = propertyIndexers[i];
				expectations.Add(propertyIndexer.Name, indexers[i].Create(
					propertyIndexer.ParameterType, propertyIndexer));
			}

			return expectations;
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetGetterIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers) =>
			new ReadOnlyDictionary<string, ArgumentExpectation>(@this.GetIndexerExpectations(indexers));

		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetSetterIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var expectations = @this.GetIndexerExpectations(indexers);
			var (name, expectation) = @this.CreateDefaultSetterExpectation();
			expectations.Add(name, expectation);
			return new ReadOnlyDictionary<string, ArgumentExpectation>(expectations);
		}

		private static (string, ArgumentExpectation) CreateDefaultSetterExpectation(this PropertyInfo @this) => 
			(Values.PropertySetterArgumentName, (ArgumentExpectation)typeof(ArgumentIsAnyExpectation).GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null, Type.EmptyTypes, null).Invoke(null));

	  internal static ReadOnlyDictionary<string, ArgumentExpectation> CreateDefaultSetterExpectationDictionary(this PropertyInfo @this)
		{
			var (name, expectation) = @this.CreateDefaultSetterExpectation();
			return new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>
				{
					{ name, expectation }
				});
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { PropertyInfoExtensions.CreateEmptyExpectations() });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this) =>
			new HandlerInformation(@this.CreateDefaultSetterExpectationDictionary());

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { @this.GetGetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { @this.GetSetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, @this.GetGetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, @this.GetSetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, uint expectedCallCount) =>
			new HandlerInformation(expectedCallCount, @this.CreateDefaultSetterExpectationDictionary());

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, @this.GetGetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, @this.GetSetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, @this.GetGetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
					ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, @this.GetSetterIndexerExpectations(indexers) });
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, PropertyInfoExtensions.CreateEmptyExpectations() });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, Delegate handler) =>
			new HandlerInformation(handler, @this.CreateDefaultSetterExpectationDictionary());

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return (HandlerInformation)handlerType.GetConstructor(
				ReflectionValues.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() });
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, Delegate handler, uint expectedCallCount) =>
			new HandlerInformation(handler, expectedCallCount, @this.CreateDefaultSetterExpectationDictionary());
	}
}