using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetFullNameTests
	{
		[Test]
		public void GetFullNameForValueType()
		{
			Assert.AreEqual("Int32", typeof(int).GetFullName());
		}

		[Test]
		public void GetFullNameForArray()
		{
			Assert.AreEqual("Int32[]", typeof(int[]).GetFullName());
		}

		[Test]
		public void GetFullNameForArrayOfOpenGenericTypes()
		{
			Assert.AreEqual("IAmAGeneric<T>[]", typeof(IAmAGeneric<>).MakeArrayType().GetFullName());
		}

		[Test]
		public void GetFullNameForArrayOfClosedGenericTypes()
		{
			Assert.AreEqual("IAmAGeneric<Int32>[]", typeof(IAmAGeneric<int>).MakeArrayType().GetFullName());
		}

		[Test]
		public void GetFullNameForOpenGenericTypes()
		{
			Assert.AreEqual("IAmAGeneric<T>", typeof(IAmAGeneric<>).GetFullName());
		}

		[Test]
		public void GetFullNameForClosedGenericTypes()
		{
			Assert.AreEqual("IAmAGeneric<Int32>", typeof(IAmAGeneric<int>).GetFullName());
		}
	}

	public interface IAmAGeneric<T> { }
}
