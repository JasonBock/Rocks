using NUnit.Framework;
using System;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsIsUnsafeToMockTests
	{
		[Test]
		public void IsUnsafeToMockWithSafeInterfaceWithSafeMembers()
		{
			Assert.IsFalse(typeof(ISafeMembers).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeReturnValue()
		{
			Assert.IsTrue(typeof(IUnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(IUnsafeMethodWithUnsafeArguments).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafePropertyType()
		{
			Assert.IsTrue(typeof(IUnsafePropertyWithUnsafePropertyType).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeIndexer()
		{
			Assert.IsTrue(typeof(IUnsafePropertyWithUnsafeIndexer).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeInterfaceWithUnsafeEventArgs()
		{
			Assert.IsFalse(typeof(ISafeEventWithUnsafeEventArgs).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeClassWithSafeMembers()
		{
			Assert.IsFalse(typeof(SafeMembers).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeReturnValue()
		{
			Assert.IsTrue(typeof(UnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(UnsafeMethodWithUnsafeArguments).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafePropertyType()
		{
			Assert.IsTrue(typeof(UnsafePropertyWithUnsafePropertyType).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithUnsafeClassWithUnsafeIndexer()
		{
			Assert.IsTrue(typeof(UnsafePropertyWithUnsafeIndexer).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithSafeClassWithUnsafeEventArgs()
		{
			Assert.IsFalse(typeof(SafeEventWithUnsafeEventArgs).IsUnsafeToMock());
		}
	}

	public interface ISafeMembers
	{
		void Target();
		int TargetReturn();
		int TargetProperty { get; set; }
		int this[int a] { get; set; }
		event EventHandler MyEvent;
	}

	public unsafe class UnsafeByteEventArgs : EventArgs
	{
		public byte* Value { get; set; }
	}

	public unsafe interface IUnsafeMethodWithUnsafeReturnValue
	{
		byte* Target();
	}

	public unsafe interface IUnsafeMethodWithUnsafeArguments
	{
		void Target(byte* a);
	}

	public unsafe interface IUnsafePropertyWithUnsafePropertyType
	{
		byte* Target { get; set; }
	}

	public unsafe interface IUnsafePropertyWithUnsafeIndexer
	{
		int this[byte* a] { get; set; }
	}

	public interface ISafeEventWithUnsafeEventArgs
	{
		event EventHandler<UnsafeByteEventArgs> Target;
	}

	public class SafeMembers
	{
		public virtual void Target() { }
		public virtual int TargetReturn() { return 0; }
		public virtual int TargetProperty { get; set; }
		public virtual int this[int a] { get { return 0; } set { } }
#pragma warning disable 67
		public virtual event EventHandler MyEvent;
#pragma warning restore 67
	}

	public unsafe class UnsafeMethodWithUnsafeReturnValue
	{
		public virtual byte* Target() { return default(byte*); }
	}

	public unsafe class UnsafeMethodWithUnsafeArguments
	{
		public virtual void Target(byte* a) { }
	}

	public unsafe class UnsafePropertyWithUnsafePropertyType
	{
		public virtual byte* Target { get; set; }
	}

	public unsafe class UnsafePropertyWithUnsafeIndexer
	{
		public virtual int this[byte* a] { get { return 0; } set { } }
	}

	public class SafeEventWithUnsafeEventArgs
	{
#pragma warning disable 67
		public virtual event EventHandler<UnsafeByteEventArgs> Target;
#pragma warning restore 67
	}
}
