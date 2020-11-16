using NUnit.Framework;
using System;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	public static class MethodInfoExtensionsContainsDelegateConditionsTests
	{
		[Test]
		public static void ContainsRefArguments() =>
			Assert.That(typeof(IHaveMethodWithRefArgument)
				.GetMethod(nameof(IHaveMethodWithRefArgument.Target))!.ContainsDelegateConditions(), Is.True);

		[Test]
		public static void ContainsOutArguments() =>
			Assert.That(typeof(IHaveMethodWithOutArgument)
				.GetMethod(nameof(IHaveMethodWithOutArgument.Target))!.ContainsDelegateConditions(), Is.True);

		[Test]
		public static void ContainsByValArguments() =>
			Assert.That(typeof(IHaveMethodWithByValArgument)
				.GetMethod(nameof(IHaveMethodWithByValArgument.Target))!.ContainsDelegateConditions(), Is.False);

		[Test]
		public static void ContainsPointerTypeArguments() =>
			Assert.That(typeof(IHaveMethodWithPointerTypeArgument)
				.GetMethod(nameof(IHaveMethodWithPointerTypeArgument.Target))!.ContainsDelegateConditions(), Is.True);

		[Test]
		public static void ContainsPointerTypeReturnType() =>
			Assert.That(typeof(IHaveMethodWithPointerTypeReturnType)
				.GetMethod(nameof(IHaveMethodWithPointerTypeReturnType.Target))!.ContainsDelegateConditions(), Is.True);

		[Test]
		public static void ContainsRuntimeArgumentHandle() =>
			Assert.That(typeof(IHaveMethodWithRuntimeArgumentHandleArgument)
				.GetMethod(nameof(IHaveMethodWithRuntimeArgumentHandleArgument.Target))!.ContainsDelegateConditions(), Is.True);

		[Test]
		public static void ContainsTypedReference() =>
			Assert.That(typeof(IHaveMethodWithTypedReferenceArgument)
				.GetMethod(nameof(IHaveMethodWithTypedReferenceArgument.Target))!.ContainsDelegateConditions(), Is.True);
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

	public interface IHaveMethodWithRuntimeArgumentHandleArgument
	{
		void Target(RuntimeArgumentHandle a);
	}

	public interface IHaveMethodWithTypedReferenceArgument
	{
		void Target(TypedReference a);
	}
}
