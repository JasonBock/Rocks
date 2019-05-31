using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	public static class NullableContextTests
	{
		[Test]
		public static void GetContextForParameterInfoThatIsNull() =>
			Assert.That(() => new NullableContext((null as ParameterInfo)!), Throws.InstanceOf<ArgumentNullException>());

		[Test]
		public static void GetNextStateTooManyTimes()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ValueTypeArray))
				.GetParameters()[0];

			var context = new NullableContext(parameter);
			context.GetNextFlag();
			context.GetNextFlag();

			Assert.That(() => context.GetNextFlag(), Throws.InstanceOf<IndexOutOfRangeException>());
		}

		[Test]
		public static void GetContextForValueType()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ValueType))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(0), nameof(context.Count));
		}

		[Test]
		public static void GetContextForValueTypeArray()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ValueTypeArray))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(2), nameof(context.Count));
			Assert.That(context.GetNextFlag(), Is.EqualTo(1), $"{nameof(context.GetNextFlag)} - 0");
			Assert.That(context.GetNextFlag(), Is.EqualTo(0), $"{nameof(context.GetNextFlag)} - 1");
		}

		[Test]
		public static void GetContextForReferenceType()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ReferenceType))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(1), nameof(context.Count));
			Assert.That(context.GetNextFlag(), Is.EqualTo(1), $"{nameof(context.GetNextFlag)} - 0");
		}

		[Test]
		public static void GetContextForReferenceTypeArray()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ReferenceTypeArray))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(1), nameof(context.Count));
			Assert.That(context.GetNextFlag(), Is.EqualTo(1), $"{nameof(context.GetNextFlag)} - 0");
		}

		[Test]
		public static void GetContextForValueTypeWithGenerics()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ValueTypeWithGenerics))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(4), nameof(context.Count));
			Assert.That(context.GetNextFlag(), Is.EqualTo(0), $"{nameof(context.GetNextFlag)} - 0");
			Assert.That(context.GetNextFlag(), Is.EqualTo(0), $"{nameof(context.GetNextFlag)} - 1");
			Assert.That(context.GetNextFlag(), Is.EqualTo(2), $"{nameof(context.GetNextFlag)} - 2");
			Assert.That(context.GetNextFlag(), Is.EqualTo(0), $"{nameof(context.GetNextFlag)} - 3");
		}

		public static void ComplexType(Dictionary<List<string>?, KeyValuePair<Guid, byte[]?>> value) { }

		public static void ReferenceType(string value) { }

		public static void ReferenceTypeArray(string[] values) { }

		public static void ValueType(int value) { }

		public static void ValueTypeArray(int[] values) { }

		public static void ValueTypeWithGenerics(KeyValuePair<Guid, byte[]?> value) { }
	}
}