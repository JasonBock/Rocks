using NUnit.Framework;
using System;
using System.Reflection;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class TypeDissectorTests
	{
		[Test]
		public void DissectSimpleType()
		{
			var dissector = new TypeDissector(typeof(int));
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectArrayType()
		{
			var dissector = new TypeDissector(typeof(int[]));
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerArrayType()
		{
			var dissector = new TypeDissector(typeof(int*[]));
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerType()
		{
			var dissector = new TypeDissector(typeof(int*));
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.False, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefType()
		{
			var type = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithByRef)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerType()
		{
			var type = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithByRefPointer)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.That(dissector.IsArray, Is.False, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefArrayType()
		{
			var type = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithByRefArray)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.False, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerArrayType()
		{
			var type = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithByRefPointerArray)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.That(dissector.IsArray, Is.True, nameof(dissector.IsArray));
			Assert.That(dissector.IsByRef, Is.True, nameof(dissector.IsByRef));
			Assert.That(dissector.IsPointer, Is.True, nameof(dissector.IsPointer));
			Assert.That(dissector.RootType, Is.EqualTo(typeof(int)), nameof(dissector.RootType));
			Assert.That(dissector.SafeName, Is.EqualTo("int"), nameof(dissector.SafeName));
		}

		[Test]
		public void GetSafeName() =>
			Assert.That(new TypeDissector(typeof(SubnestedClass.IAmSubnested)).SafeName,
		Is.EqualTo("TypeDissectorTests.SubnestedClass.IAmSubnested"));

		[Test]
		public void GetSafeNameWithOpenGenerics() =>
			Assert.That(new TypeDissector(typeof(IHaveGenerics<>)).SafeName,
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithClosedGenerics() =>
			Assert.That(new TypeDissector(typeof(IHaveGenerics<string>)).SafeName,
				Is.EqualTo("IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedOpenGenerics() =>
			Assert.That(new TypeDissector(typeof(NestedGenerics.IHaveGenerics<>)).SafeName,
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameWithNestedClosedGenerics() =>
			Assert.That(new TypeDissector(typeof(NestedGenerics.IHaveGenerics<string>)).SafeName,
				Is.EqualTo("NestedGenerics.IHaveGenerics"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(new TypeDissector(typeof(RefTargetWithoutAGeneric)).SafeName,
				Is.EqualTo("RefTargetWithoutAGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndRefArguments() =>
			Assert.That(new TypeDissector(typeof(RefTargetWithAGeneric<>)).SafeName,
				Is.EqualTo("RefTargetWithAGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithNoGenericsAndRefArguments() =>
			Assert.That(new TypeDissector(typeof(TypeDissectorTests.RefTargetWithoutGeneric)).SafeName,
				Is.EqualTo("TypeDissectorTests.RefTargetWithoutGeneric"));

		[Test]
		public void GetSafeNameForNestedDelegateWithGenericsAndRefArguments() =>
			Assert.That(new TypeDissector(typeof(TypeDissectorTests.RefTargetWithGeneric<Guid>)).SafeName,
				Is.EqualTo("TypeDissectorTests.RefTargetWithGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenerics() =>
			Assert.That(new TypeDissector(typeof(MapForNonGeneric)).SafeName,
				Is.EqualTo("MapForNonGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenerics() =>
			Assert.That(new TypeDissector(typeof(MapForGeneric<Guid>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenerics() =>
			Assert.That(new TypeDissector(typeof(MapForGeneric<>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithNoGenericsAndNamespaces() =>
			Assert.That(new TypeDissector(typeof(MapForNonGeneric)).SafeName,
				Is.EqualTo("MapForNonGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithSpecifiedGenericsAndNamespaces() =>
			Assert.That(new TypeDissector(typeof(MapForGeneric<Guid>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForDelegateWithoutSpecifiedGenericsAndNamespaces() =>
			Assert.That(new TypeDissector(typeof(MapForGeneric<>)).SafeName,
				Is.EqualTo("MapForGeneric"));

		[Test]
		public void GetSafeNameForBoolPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(bool)).SafeName, Is.EqualTo("bool"));

		[Test]
		public void GetSafeNameForBytePrimitiveType() =>
			Assert.That(new TypeDissector(typeof(byte)).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForSBytePrimitiveType() =>
			Assert.That(new TypeDissector(typeof(sbyte)).SafeName, Is.EqualTo("sbyte"));

		[Test]
		public void GetSafeNameForShortPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(short)).SafeName, Is.EqualTo("short"));

		[Test]
		public void GetSafeNameForUShortPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(ushort)).SafeName, Is.EqualTo("ushort"));

		[Test]
		public void GetSafeNameForIntPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(int)).SafeName, Is.EqualTo("int"));

		[Test]
		public void GetSafeNameForUIntPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(uint)).SafeName, Is.EqualTo("uint"));

		[Test]
		public void GetSafeNameForLongPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(long)).SafeName, Is.EqualTo("long"));

		[Test]
		public void GetSafeNameForULongPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(ulong)).SafeName, Is.EqualTo("ulong"));

		[Test]
		public void GetSafeNameForCharPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(char)).SafeName, Is.EqualTo("char"));

		[Test]
		public void GetSafeNameForDoublePrimitiveType() =>
			Assert.That(new TypeDissector(typeof(double)).SafeName, Is.EqualTo("double"));

		[Test]
		public void GetSafeNameForFloatPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(float)).SafeName, Is.EqualTo("float"));

		[Test]
		public void GetSafeNameForDecimalPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(decimal)).SafeName, Is.EqualTo("decimal"));

		[Test]
		public void GetSafeNameForStringPrimitiveType() =>
			Assert.That(new TypeDissector(typeof(string)).SafeName, Is.EqualTo("string"));

		[Test]
		public void GetSafeNameForPointerType() =>
			Assert.That(new TypeDissector(typeof(byte*)).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForArrayType() =>
			Assert.That(new TypeDissector(typeof(byte[])).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameForArrayOfPointersType() =>
			Assert.That(new TypeDissector(typeof(byte*[])).SafeName, Is.EqualTo("byte"));

		[Test]
		public void GetSafeNameWhenTypeNameCollidesWithRocksTypeName() =>
			Assert.That(new TypeDissector(typeof(TypeExtensionsNamespace.IMock)).SafeName,
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
