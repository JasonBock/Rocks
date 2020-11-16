using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsGetFullNameTests
	{
		[Test]
		public static void GetFullNameForValueType() =>
			Assert.That(typeof(int).GetFullName(), Is.EqualTo("int"));

		[Test]
		public static void GetFullNameForArray() =>
			Assert.That(typeof(int[]).GetFullName(), Is.EqualTo("int[]"));

		[Test]
		public static void GetFullNameForArrayOfOpenGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<>).MakeArrayType().GetFullName(), Is.EqualTo("IAmAGeneric<T>[]"));

		[Test]
		public static void GetFullNameForArrayOfClosedGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<int>).MakeArrayType().GetFullName(), Is.EqualTo("IAmAGeneric<int>[]"));

		[Test]
		public static void GetFullNameForOpenGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<>).GetFullName(), Is.EqualTo("IAmAGeneric<T>"));

		[Test]
		public static void GetFullNameForClosedGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<int>).GetFullName(), Is.EqualTo("IAmAGeneric<int>"));

		[Test]
		public static void GetFullNameForComplexTypeParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ComplexType));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("Dictionary<List<string>, KeyValuePair<Guid, byte[]?>>"));
		}

		[Test]
		public static void GetFullNameForReferenceTypeParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ReferenceType));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("string"));
		}

		[Test]
		public static void GetFullNameForReferenceTypeArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ReferenceTypeArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("string[]"));
		}

		[Test]
		public static void GetFullNameForReferenceTypeNullArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ReferenceTypeNullArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("string[]?"));
		}

		[Test]
		public static void GetFullNameForReferenceTypeNullValuesArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ReferenceTypeNullValuesArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("string?[]"));
		}

		[Test]
		public static void GetFullNameForReferenceTypeNullValuesAndArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ReferenceTypeNullValuesAndArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("string?[]?"));
		}

		[Test]
		public static void GetFullNameForValueTypeParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ValueType));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("int"));
		}

		[Test]
		public static void GetFullNameForValueTypeArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ValueTypeArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("int[]"));
		}

		[Test]
		public static void GetFullNameForValueTypeNullArrayParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ValueTypeNullArray));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("int[]?"));
		}

		[Test]
		public static void GetFullNameForValueTypeWithGenericsParameter()
		{
			var parameter = TypeExtensionsGetFullNameTests.GetParameter(nameof(TypeExtensionsGetFullNameTests.ValueTypeWithGenerics));
			Assert.That(parameter.ParameterType.GetFullName(parameter), Is.EqualTo("KeyValuePair<Guid, byte[]?>"));
		}

		private static ParameterInfo GetParameter(string methodName) =>
			typeof(TypeExtensionsGetFullNameTests).GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)!.GetParameters()[0];

		public static void ComplexType(Dictionary<List<string>, KeyValuePair<Guid, byte[]?>> value) { }

		public static void ReferenceType(string value) { }

		public static void ReferenceTypeArray(string[] values) { }

		public static void ReferenceTypeNullArray(string[]? values) { }

		public static void ReferenceTypeNullValuesArray(string?[] values) { }

		public static void ReferenceTypeNullValuesAndArray(string?[]? values) { }

		public static void ValueType(int value) { }

		public static void ValueTypeArray(int[] values) { }

		public static void ValueTypeNullArray(int[]? values) { }

		public static void ValueTypeWithGenerics(KeyValuePair<Guid, byte[]?> value) { }
	}

	public interface IAmAGeneric<T> { }
}