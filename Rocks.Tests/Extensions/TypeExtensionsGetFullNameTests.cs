using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetFullNameTests
	{
		[Test]
		public void GetFullNameForValueType() =>
			Assert.That(typeof(int).GetFullName(), Is.EqualTo("int"));

		[Test]
		public void GetFullNameForArray() =>
			Assert.That(typeof(int[]).GetFullName(), Is.EqualTo("int[]"));

		[Test]
		public void GetFullNameForArrayOfOpenGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<>).MakeArrayType().GetFullName(), Is.EqualTo("IAmAGeneric<T>[]"));

		[Test]
		public void GetFullNameForArrayOfClosedGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<int>).MakeArrayType().GetFullName(), Is.EqualTo("IAmAGeneric<int>[]"));

		[Test]
		public void GetFullNameForOpenGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<>).GetFullName(), Is.EqualTo("IAmAGeneric<T>"));

		[Test]
		public void GetFullNameForClosedGenericTypes() =>
			Assert.That(typeof(IAmAGeneric<int>).GetFullName(), Is.EqualTo("IAmAGeneric<int>"));
	}

	public interface IAmAGeneric<T> { }
}