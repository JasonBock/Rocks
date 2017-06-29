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
		public void GetSafeName() =>
			Assert.That(typeof(SubnestedClass.IAmSubnested).GetSafeName(),
				Is.EqualTo("TypeExtensionsGetSafeNameTests.SubnestedClass.IAmSubnested"));

		[Test]
		public void GetSafeNameWithOpenGenerics() =>
			Assert.That(typeof(IHaveGenerics<>).GetSafeName(),
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithClosedGenerics() =>
			Assert.That(typeof(IHaveGenerics<string>).GetSafeName(),
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedOpenGenerics() =>
			Assert.That(typeof(NestedGenerics.IHaveGenerics<>).GetSafeName(),
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedClosedGenerics() =>
			Assert.That(typeof(NestedGenerics.IHaveGenerics<string>).GetSafeName(),
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(typeof(RefTargetWithoutGeneric).GetSafeName(),
				Is.EqualTo("RefTargetWithoutGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndRefArguments() =>
			Assert.That(typeof(RefTargetWithGeneric<>).GetSafeName(),
				Is.EqualTo("RefTargetWithGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(typeof(TypeExtensionsTests.RefTargetWithoutGeneric).GetSafeName(),
				Is.EqualTo("TypeExtensionsTests.RefTargetWithoutGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithGenericsAndRefArguments() =>
			Assert.That(typeof(TypeExtensionsTests.RefTargetWithGeneric<Guid>).GetSafeName(),
				Is.EqualTo("TypeExtensionsTests.RefTargetWithGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics() =>
			Assert.That(typeof(MapForNonGeneric).GetSafeName(),
				Is.EqualTo("MapForNonGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenerics() =>
			Assert.That(typeof(MapForGeneric<Guid>).GetSafeName(),
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics() =>
			Assert.That(typeof(MapForGeneric<>).GetSafeName(),
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndNamespaces()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(typeof(MapForNonGeneric).GetSafeName(namespaces),
				Is.EqualTo("MapForNonGeneric"));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(MapForNonGeneric).Namespace), Is.True);
		}

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenericsAndNamespaces()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(typeof(MapForGeneric<Guid>).GetSafeName(namespaces),
				Is.EqualTo("MapForGeneric"));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(MapForGeneric<Guid>).Namespace), Is.True);
		}

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndNamespaces()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(typeof(MapForGeneric<>).GetSafeName(namespaces),
				Is.EqualTo("MapForGeneric"));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(typeof(MapForGeneric<>).Namespace), Is.True);
		}

		[Test]
		public void GetSafeNameForBoolPrimitiveType() =>
			Assert.That(typeof(bool).GetSafeName(), Is.EqualTo("bool"));

		[Test]
		public void GetSafeNameForBytePrimitiveType() =>
			Assert.That(typeof(byte).GetSafeName(), Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForSBytePrimitiveType() =>
			Assert.That(typeof(sbyte).GetSafeName(), Is.EqualTo("sbyte"));

		[Test]
		public void GetSafeNameForShortPrimitiveType() =>
			Assert.That(typeof(short).GetSafeName(), Is.EqualTo("short"));

		[Test]
		public void GetSafeNameForUShortPrimitiveType() =>
			Assert.That(typeof(ushort).GetSafeName(), Is.EqualTo("ushort"));

		[Test]
		public void GetSafeNameForIntPrimitiveType() =>
			Assert.That(typeof(int).GetSafeName(), Is.EqualTo("int"));

		[Test]
		public void GetSafeNameForUIntPrimitiveType() =>
			Assert.That(typeof(uint).GetSafeName(), Is.EqualTo("uint"));

		[Test]
		public void GetSafeNameForLongPrimitiveType() =>
			Assert.That(typeof(long).GetSafeName(), Is.EqualTo("long"));

		[Test]
		public void GetSafeNameForULongPrimitiveType() =>
			Assert.That(typeof(ulong).GetSafeName(), Is.EqualTo("ulong"));

		[Test]
		public void GetSafeNameForCharPrimitiveType() =>
			Assert.That(typeof(char).GetSafeName(), Is.EqualTo("char"));

		[Test]
		public void GetSafeNameForDoublePrimitiveType() =>
			Assert.That(typeof(double).GetSafeName(), Is.EqualTo("double"));

		[Test]
		public void GetSafeNameForFloatPrimitiveType() =>
			Assert.That(typeof(float).GetSafeName(), Is.EqualTo("float"));

		[Test]
		public void GetSafeNameForDecimalPrimitiveType() =>
			Assert.That(typeof(decimal).GetSafeName(), Is.EqualTo("decimal"));

		[Test]
		public void GetSafeNameForStringPrimitiveType() =>
			Assert.That(typeof(string).GetSafeName(), Is.EqualTo("string"));

		[Test]
		public void GetSafeNameForPointerType() =>
			Assert.That(typeof(byte*).GetSafeName(), Is.EqualTo("byte*"));

		[Test]
		public void GetSafeNameForArrayType() =>
			Assert.That(typeof(byte[]).GetSafeName(), Is.EqualTo("byte[]"));

		[Test]
		public void GetSafeNameForArrayOfPointersType() =>
			Assert.That(typeof(byte*[]).GetSafeName(), Is.EqualTo("byte*[]"));

		[Test]
		public void GetSafeNameWhenTypeNameCollidesWithRocksTypeName() =>
			Assert.That(typeof(TypeExtensionsNamespace.IMock).GetSafeName(),
				Is.EqualTo("TypeExtensionsNamespace.IMock"));

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

namespace TypeExtensionsNamespace
{
	public interface IMock { }
}