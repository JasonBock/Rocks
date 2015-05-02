using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeDissectorTests
	{
		[Test]
		public void DissectSimpleType()
		{
			var dissector = new TypeDissector(typeof(int));
			Assert.IsFalse(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsFalse(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsFalse(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectArrayType()
		{
			var dissector = new TypeDissector(typeof(int[]));
			Assert.IsTrue(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsFalse(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsFalse(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerArrayType()
		{
			var dissector = new TypeDissector(typeof(int*[]));
			Assert.IsTrue(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsFalse(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsTrue(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectPointerType()
		{
			var dissector = new TypeDissector(typeof(int*));
			Assert.IsFalse(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsFalse(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsTrue(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRef)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.IsFalse(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsTrue(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsFalse(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefPointer)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.IsFalse(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsTrue(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsTrue(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefArrayType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefArray)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.IsTrue(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsTrue(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsFalse(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		[Test]
		public void DissectByRefPointerArrayType()
		{
			var type = this.GetType().GetMethod(nameof(this.TargetWithByRefPointerArray)).GetParameters()[0].ParameterType;
			var dissector = new TypeDissector(type);
			Assert.IsTrue(dissector.IsArray, nameof(dissector.IsArray));
			Assert.IsTrue(dissector.IsByRef, nameof(dissector.IsByRef));
			Assert.IsTrue(dissector.IsPointer, nameof(dissector.IsPointer));
			Assert.AreEqual(typeof(int), dissector.RootType, nameof(dissector.RootType));
			Assert.AreEqual("Int32", dissector.SafeName, nameof(dissector.SafeName));
		}

		public void TargetWithByRef(ref int a) { }
		public void TargetWithByRefArray(ref int[] a) { }
		public unsafe void TargetWithByRefPointer(ref int* a) { }
		public unsafe void TargetWithByRefPointerArray(ref int*[] a) { }
	}
}
