using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class PropertyInfoExtensions
	{
		private static ReadOnlyDictionary<string, ArgumentExpectation> CreateEmptyExpectations()
		{
			return new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
		}

		private static Dictionary<string, ArgumentExpectation> GetIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var expectations = new Dictionary<string, ArgumentExpectation>();
			var propertyIndexers = @this.GetIndexParameters();

			for (var i = 0; i < indexers.Count; i++)
			{
				var propertyIndexer = propertyIndexers[i];
            expectations.Add(propertyIndexer.Name, indexers[i].Create(propertyIndexer.ParameterType));
			}

			return expectations;
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetGetterIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			return new ReadOnlyDictionary<string, ArgumentExpectation>(@this.GetIndexerExpectations(indexers));
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetSetterIndexerExpectations(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var expectations = @this.GetIndexerExpectations(indexers);
			var setterExpectations = @this.CreateDefaultSetterExpectation();
			expectations.Add(setterExpectations.Item1, setterExpectations.Item2);
			return new ReadOnlyDictionary<string, ArgumentExpectation>(expectations);
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetSetterIndexerExpectations<TProperty>(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers,
			Expression<Func<TProperty>> setterExpectation)
		{
			var expectations = @this.GetIndexerExpectations(indexers);
			var setterExpectations = @this.CreateDefaultSetterExpectation();
			expectations.Add(Constants.Values.PropertySetterArgumentName, setterExpectation.GetExpectationForSetter());
			return new ReadOnlyDictionary<string, ArgumentExpectation>(expectations);
		}

		private static Tuple<string, ArgumentExpectation> CreateDefaultSetterExpectation(this PropertyInfo @this)
		{
			var expectationType = typeof(ArgumentExpectation<>).MakeGenericType(@this.PropertyType);
			var expectation = expectationType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				Type.EmptyTypes, null).Invoke(null) as ArgumentExpectation;
			return new Tuple<string, ArgumentExpectation>(Constants.Values.PropertySetterArgumentName, expectation);
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> CreateDefaultSetterExpectationDictionary(this PropertyInfo @this)
		{
			var expectation = @this.CreateDefaultSetterExpectation();
			return new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>
				{
					{ expectation.Item1, expectation.Item2 }
				});
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this)
		{
			return new HandlerInformation(@this.CreateDefaultSetterExpectationDictionary());
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { @this.GetGetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { @this.GetSetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, @this.GetGetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, @this.GetSetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, uint expectedCallCount)
		{
			return new HandlerInformation(expectedCallCount, @this.CreateDefaultSetterExpectationDictionary());
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, @this.GetGetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, @this.GetSetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, @this.GetGetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, ReadOnlyCollection<Expression> indexers, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, @this.GetSetterIndexerExpectations(indexers) }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, Delegate handler)
		{
			return new HandlerInformation(handler, @this.CreateDefaultSetterExpectationDictionary());
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetSetterHandler(this PropertyInfo @this, Delegate handler, uint expectedCallCount)
		{
			return new HandlerInformation(handler, expectedCallCount, @this.CreateDefaultSetterExpectationDictionary());
		}
	}
}
