using NUnit.Framework;
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
	}

	public interface IAmAGeneric<T> { }
}