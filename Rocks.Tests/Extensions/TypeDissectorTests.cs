using NUnit.Framework;
using Rocks.Extensions;
using System.Reflection;

namespace Rocks.Tests.Extensions
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

		public void TargetWithByRef(ref int a) { }
		public void TargetWithByRefArray(ref int[] a) { }
		public unsafe void TargetWithByRefPointer(ref int* a) { }
		public unsafe void TargetWithByRefPointerArray(ref int*[] a) { }
	}
}
