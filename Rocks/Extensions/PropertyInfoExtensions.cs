using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class PropertyInfoExtensions
	{
		private static ReadOnlyDictionary<string, ArgumentExpectation> CreateEmptyExpectations()
		{
			return new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
		}

		internal static ReadOnlyDictionary<string, ArgumentExpectation> CreateDefaultSetterExpectation(this PropertyInfo @this)
		{
			var expectationType = typeof(ArgumentExpectation<>).MakeGenericType(@this.PropertyType);
			var expectation = expectationType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				Type.EmptyTypes, null).Invoke(null) as ArgumentExpectation;
			return new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>
				{
					{ Constants.Values.PropertySetterArgumentName, expectation }
				});
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new[] { PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}

		internal static HandlerInformation GetGetterHandler(this PropertyInfo @this, Delegate handler, uint expectedCallCount)
		{
			var handlerType = typeof(HandlerInformation<>).MakeGenericType(@this.PropertyType);
			return handlerType.GetConstructor(Constants.Reflection.PublicNonPublicInstance, null,
				new[] { typeof(Delegate), typeof(uint), typeof(ReadOnlyDictionary<string, ArgumentExpectation>) }, null)
				.Invoke(new object[] { handler, expectedCallCount, PropertyInfoExtensions.CreateEmptyExpectations() }) as HandlerInformation;
		}
	}
}
