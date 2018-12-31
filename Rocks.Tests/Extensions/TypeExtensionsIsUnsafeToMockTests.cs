using NUnit.Framework;
using System;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsIsUnsafeToMockTests
	{
		[Test]
		public static void IsSpanLikeForSpan() =>
			Assert.That(typeof(Span<char>).IsSpanLike(), Is.True);

		[Test]
		public static void IsSpanLikeForReadOnlySpan() =>
			Assert.That(typeof(ReadOnlySpan<char>).IsSpanLike(), Is.True);

		[Test]
		public static void IsSpanLikeForNonSpanLikeType() =>
			Assert.That(typeof(Guid).IsSpanLike(), Is.False);

		[Test]
		public static void IsUnsafeToMockWithSafeInterfaceWithSafeMembers() =>
			Assert.That(typeof(ISafeMembers).IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeReturnValue() =>
			Assert.That(typeof(IUnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeMethodWithUnsafeArguments() =>
			Assert.That(typeof(IUnsafeMethodWithUnsafeArguments).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeInterfaceWithUnsafePropertyType() =>
			Assert.That(typeof(IUnsafePropertyWithUnsafePropertyType).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeInterfaceWithUnsafeIndexer() =>
			Assert.That(typeof(IUnsafePropertyWithUnsafeIndexer).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithSafeInterfaceWithUnsafeEventArgs() =>
			Assert.That(typeof(ISafeEventWithUnsafeEventArgs).IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeToMockWithSafeClassWithSafeMembers() =>
			Assert.That(typeof(SafeMembers).IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeReturnValue() =>
			Assert.That(typeof(UnsafeMethodWithUnsafeReturnValue).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeClassWithUnsafeMethodWithUnsafeArguments() =>
			Assert.That(typeof(UnsafeMethodWithUnsafeArguments).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeClassWithUnsafePropertyType() =>
			Assert.That(typeof(UnsafePropertyWithUnsafePropertyType).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithUnsafeClassWithUnsafeIndexer() =>
			Assert.That(typeof(UnsafePropertyWithUnsafeIndexer).IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeToMockWithSafeClassWithUnsafeEventArgs() =>
			Assert.That(typeof(SafeEventWithUnsafeEventArgs).IsUnsafeToMock(), Is.False);
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
		public virtual int TargetReturn() => 0;
		public virtual int TargetProperty { get; set; }
		public virtual int this[int a] { get => 0; set { } }
#pragma warning disable 67
		public virtual event EventHandler MyEvent;
#pragma warning restore 67
	}

	public unsafe class UnsafeMethodWithUnsafeReturnValue
	{
		public virtual byte* Target() => default; 
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
		public virtual int this[byte* a] { get => 0; set { } }
	}

	public class SafeEventWithUnsafeEventArgs
	{
#pragma warning disable 67
		public virtual event EventHandler<UnsafeByteEventArgs> Target;
#pragma warning restore 67
	}
}
