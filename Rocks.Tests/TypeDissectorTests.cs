using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public sealed class TypeDissectorTests
	{
		[Test]
		public void DissectSimpleType()
		{
			var dissector = TypeDissector.Create(typeof(int));
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectArrayType()
		{
			var dissector = TypeDissector.Create(typeof(int[]));
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerArrayType()
		{
			var dissector = TypeDissector.Create(typeof(int*[]));
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerType()
		{
			var dissector = TypeDissector.Create(typeof(int*));
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRef)).GetParameters()[0].ParameterType;
			var dissector = TypeDissector.Create(type);
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefPointer)).GetParameters()[0].ParameterType;
			var dissector = TypeDissector.Create(type);
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefArrayType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefArray)).GetParameters()[0].ParameterType;
			var dissector = TypeDissector.Create(type);
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerArrayType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefPointerArray)).GetParameters()[0].ParameterType;
			var dissector = TypeDissector.Create(type);
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void GetSafeName() =>
			Assert.That(TypeDissector.Create(typeof(SubnestedClass.IAmSubnested)).SafeName,
				Is.EqualTo("TypeDissectorTests.SubnestedClass.IAmSubnested"));

		[Test]
		public void GetSafeNameWithOpenGenerics() =>
			Assert.That(TypeDissector.Create(typeof(IHaveGenerics<>)).SafeName,
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithClosedGenerics() =>
			Assert.That(TypeDissector.Create(typeof(IHaveGenerics<string>)).SafeName,
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedOpenGenerics() =>
			Assert.That(TypeDissector.Create(typeof(NestedGenerics.IHaveGenerics<>)).SafeName,
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedClosedGenerics() =>
			Assert.That(TypeDissector.Create(typeof(NestedGenerics.IHaveGenerics<string>)).SafeName,
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(TypeDissector.Create(typeof(RefTargetWithoutAGeneric)).SafeName,
				Is.EqualTo("RefTargetWithoutAGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndRefArguments() =>
			Assert.That(TypeDissector.Create(typeof(RefTargetWithAGeneric<>)).SafeName,
				Is.EqualTo("RefTargetWithAGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(TypeDissector.Create(typeof(TypeDissectorTests.RefTargetWithoutGeneric)).SafeName,
				Is.EqualTo("TypeDissectorTests.RefTargetWithoutGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithGenericsAndRefArguments() =>
			Assert.That(TypeDissector.Create(typeof(TypeDissectorTests.RefTargetWithGeneric<Guid>)).SafeName,
				Is.EqualTo("TypeDissectorTests.RefTargetWithGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics() =>
			Assert.That(TypeDissector.Create(typeof(MapForNonGeneric)).SafeName,
				Is.EqualTo("MapForNonGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenerics() =>
			Assert.That(TypeDissector.Create(typeof(MapForGeneric<Guid>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics() =>
			Assert.That(TypeDissector.Create(typeof(MapForGeneric<>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndNamespaces() =>
			Assert.That(TypeDissector.Create(typeof(MapForNonGeneric)).SafeName,
				Is.EqualTo("MapForNonGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenericsAndNamespaces() =>
			Assert.That(TypeDissector.Create(typeof(MapForGeneric<Guid>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndNamespaces() =>
			Assert.That(TypeDissector.Create(typeof(MapForGeneric<>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForBoolPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(bool)).SafeName, Is.EqualTo("bool"));

		[Test]
		public void GetSafeNameForBytePrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(byte)).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForSBytePrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(sbyte)).SafeName, Is.EqualTo("sbyte"));

		[Test]
		public void GetSafeNameForShortPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(short)).SafeName, Is.EqualTo("short"));

		[Test]
		public void GetSafeNameForUShortPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(ushort)).SafeName, Is.EqualTo("ushort"));

		[Test]
		public void GetSafeNameForIntPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(int)).SafeName, Is.EqualTo("int"));

		[Test]
		public void GetSafeNameForUIntPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(uint)).SafeName, Is.EqualTo("uint"));

		[Test]
		public void GetSafeNameForLongPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(long)).SafeName, Is.EqualTo("long"));

		[Test]
		public void GetSafeNameForULongPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(ulong)).SafeName, Is.EqualTo("ulong"));

		[Test]
		public void GetSafeNameForCharPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(char)).SafeName, Is.EqualTo("char"));

		[Test]
		public void GetSafeNameForDoublePrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(double)).SafeName, Is.EqualTo("double"));

		[Test]
		public void GetSafeNameForFloatPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(float)).SafeName, Is.EqualTo("float"));

		[Test]
		public void GetSafeNameForDecimalPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(decimal)).SafeName, Is.EqualTo("decimal"));

		[Test]
		public void GetSafeNameForStringPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(string)).SafeName, Is.EqualTo("string"));

		[Test]
		public void GetSafeNameForObjectPrimitiveType() =>
			Assert.That(TypeDissector.Create(typeof(object)).SafeName, Is.EqualTo("object"));

		[Test]
		public void GetSafeNameForPointerType() =>
			Assert.That(TypeDissector.Create(typeof(byte*)).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForArrayType() =>
			Assert.That(TypeDissector.Create(typeof(byte[])).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForArrayOfPointersType() =>
			Assert.That(TypeDissector.Create(typeof(byte*[])).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameWhenTypeNameCollidesWithRocksTypeName() =>
			Assert.That(TypeDissector.Create(typeof(TypeExtensionsNamespace.IMock)).SafeName,
				Is.EqualTo("TypeExtensionsNamespace.IMock"));

		public delegate void RefTargetWithoutGeneric(ref Guid a);
		public delegate void RefTargetWithGeneric<T>(ref T a);
		public void TargetWithByRef(ref int a) { }
		public void TargetWithByRefArray(ref int[] a) { }
		public unsafe void TargetWithByRefPointer(ref int* a) { }
		public unsafe void TargetWithByRefPointerArray(ref int*[] a) { }

		public class SubnestedClass
		{
			public interface IAmSubnested { }
		}
	}

	public delegate void MapForNonGeneric(int a);
	public delegate void MapForGeneric<T>(T a);

	public delegate void RefTargetWithoutAGeneric(ref Guid a);
	public delegate void RefTargetWithAGeneric<T>(ref T a);

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
