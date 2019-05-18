using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	public static class NullableContextTests
	{
		[Test]
		public static void GetContextForFlagsThatIsNull() => 
			Assert.That(() => new NullableContext((null as byte[])!), Throws.InstanceOf<ArgumentNullException>());

		[Test]
		public static void GetContextForParameterInfoThatIsNull() =>
			Assert.That(() => new NullableContext((null as ParameterInfo)!), Throws.InstanceOf<ArgumentNullException>());

		[Test]
		public static void GetContextForFlags()
		{
			var context = new NullableContext(new byte[] { 1, 2, 2, 1 });

			Assert.That(context.Count, Is.EqualTo(4), nameof(context.Count));
			Assert.That(context.GetNextState(), Is.EqualTo(1), $"{nameof(context.GetNextState)} - 0");
			Assert.That(context.GetNextState(), Is.EqualTo(2), $"{nameof(context.GetNextState)} - 1");
			Assert.That(context.GetNextState(), Is.EqualTo(2), $"{nameof(context.GetNextState)} - 2");
			Assert.That(context.GetNextState(), Is.EqualTo(1), $"{nameof(context.GetNextState)} - 3");
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
			Assert.That(context.GetNextState(), Is.EqualTo(1), $"{nameof(context.GetNextState)} - 0");
			Assert.That(context.GetNextState(), Is.EqualTo(0), $"{nameof(context.GetNextState)} - 1");
		}

		[Test]
		public static void GetContextForReferenceType()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ReferenceType))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(1), nameof(context.Count));
			Assert.That(context.GetNextState(), Is.EqualTo(1), $"{nameof(context.GetNextState)} - 0");
		}

		[Test]
		public static void GetContextForReferenceTypeArray()
		{
			var parameter = typeof(NullableContextTests).GetMethod(nameof(NullableContextTests.ReferenceTypeArray))
				.GetParameters()[0];

			var context = new NullableContext(parameter);

			Assert.That(context.Count, Is.EqualTo(1), nameof(context.Count));
			Assert.That(context.GetNextState(), Is.EqualTo(1), $"{nameof(context.GetNextState)} - 0");
		}

		public static void ReferenceType(string value) { }

		public static void ReferenceTypeArray(string[] values) { }

		public static void ValueType(int value) { }

		public static void ValueTypeArray(int[] values) { }

	}
}
