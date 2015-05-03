using NUnit.Framework;
using System;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsContainsDelegateConditionsTests
	{
		[Test]
		public void ContainsRefArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithRefArgument)
				.GetMethod(nameof(IHaveMethodWithRefArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsOutArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithOutArgument)
				.GetMethod(nameof(IHaveMethodWithOutArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsByValArguments()
		{
			Assert.IsFalse(typeof(IHaveMethodWithByValArgument)
				.GetMethod(nameof(IHaveMethodWithByValArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsPointerTypeArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithPointerTypeArgument)
				.GetMethod(nameof(IHaveMethodWithPointerTypeArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsPointerTypeReturnType()
		{
			Assert.IsTrue(typeof(IHaveMethodWithPointerTypeReturnType)
				.GetMethod(nameof(IHaveMethodWithPointerTypeReturnType.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsArgIterator()
		{
			Assert.IsTrue(typeof(IHaveMethodWithArgIteratorArgument)
				.GetMethod(nameof(IHaveMethodWithArgIteratorArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsRuntimeArgumentHandle()
		{
			Assert.IsTrue(typeof(IHaveMethodWithRuntimeArgumentHandleArgument)
				.GetMethod(nameof(IHaveMethodWithRuntimeArgumentHandleArgument.Target)).ContainsDelegateConditions());
		}

		[Test]
		public void ContainsTypedReference()
		{
			Assert.IsTrue(typeof(IHaveMethodWithTypedReferenceArgument)
				.GetMethod(nameof(IHaveMethodWithTypedReferenceArgument.Target)).ContainsDelegateConditions());
		}
	}

	public interface IHaveMethodWithPointerTypeArgument
	{
		unsafe void Target(int* a);
	}

	public interface IHaveMethodWithPointerTypeReturnType
	{
		unsafe int* Target();
	}

	public interface IHaveMethodWithOutArgument
	{
		void Target(out int a);
	}

	public interface IHaveMethodWithRefArgument
	{
		void Target(ref int a);
	}

	public interface IHaveMethodWithByValArgument
	{
		void Target(int a);
	}

	public interface IHaveMethodWithArgIteratorArgument
	{
		void Target(ArgIterator a);
	}

	public interface IHaveMethodWithRuntimeArgumentHandleArgument
	{
		void Target(RuntimeArgumentHandle a);
	}

	public interface IHaveMethodWithTypedReferenceArgument
	{
		void Target(TypedReference a);
	}
}
