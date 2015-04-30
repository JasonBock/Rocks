using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetSafeNameTests
	{
		[Test]
		public void GetSafeName()
		{
			Assert.AreEqual("TypeExtensionsGetSafeNameTests.SubnestedClass.IAmSubnested",
				typeof(SubnestedClass.IAmSubnested).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithOpenGenerics()
		{
			Assert.AreEqual("IHaveGenerics", typeof(IHaveGenerics<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithClosedGenerics()
		{
			Assert.AreEqual("IHaveGenerics", typeof(IHaveGenerics<string>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithNestedOpenGenerics()
		{
			Assert.AreEqual("NestedGenerics.IHaveGenerics", typeof(NestedGenerics.IHaveGenerics<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameWithNestedClosedGenerics()
		{
			Assert.AreEqual("NestedGenerics.IHaveGenerics", typeof(NestedGenerics.IHaveGenerics<string>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndRefArguments()
		{
			Assert.AreEqual("RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndRefArguments()
		{
			Assert.AreEqual("RefTargetWithGeneric",
				typeof(Rocks.Tests.Extensions.RefTargetWithGeneric<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenericsAndRefArguments()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithoutGeneric",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithoutGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForNestedDelegateWithGenericsAndRefArguments()
		{
			Assert.AreEqual("TypeExtensionsTests.RefTargetWithGeneric",
				typeof(Rocks.Tests.Extensions.TypeExtensionsTests.RefTargetWithGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics()
		{
			Assert.AreEqual("MapForNonGeneric",
				typeof(Rocks.Tests.Extensions.MapForNonGeneric).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenerics()
		{
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics()
		{
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<>).GetSafeName());
		}

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForNonGeneric",
				typeof(Rocks.Tests.Extensions.MapForNonGeneric).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForNonGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<Guid>).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndContext()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("MapForGeneric",
				typeof(Rocks.Tests.Extensions.MapForGeneric<>).GetSafeName(
					typeof(IMapToDelegates).GetMethod(nameof(IMapToDelegates.MapForGeneric)), namespaces));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetSafeNameForPointerType()
		{
			Assert.AreEqual("Byte*", typeof(byte*).GetSafeName());
		}

		[Test]
		public void GetSafeNameForArrayType()
		{
			Assert.AreEqual("Byte[]", typeof(byte[]).GetSafeName());
		}

		[Test]
		public void GetSafeNameForArrayOfPointersType()
		{
			Assert.AreEqual("Byte*[]", typeof(byte*[]).GetSafeName());
		}

		public class SubnestedClass
		{
			public interface IAmSubnested { }
		}
	}

	public class NestedGenerics
	{
		public interface IHaveGenerics<T> { }
	}

	public interface IHaveGenerics<T> { }
}
