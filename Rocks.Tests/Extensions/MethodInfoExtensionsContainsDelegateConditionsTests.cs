using NUnit.Framework;
using System;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsContainsDelegateConditionsTests
	{
		[Test]
		public void ContainsRefArguments() =>
			Assert.That(typeof(IHaveMethodWithRefArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithRefArgument.Target)).ContainsDelegateConditions(), Is.True);

		[Test]
		public void ContainsOutArguments() =>
			Assert.That(typeof(IHaveMethodWithOutArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithOutArgument.Target)).ContainsDelegateConditions(), Is.True);

		[Test]
		public void ContainsByValArguments() =>
			Assert.That(typeof(IHaveMethodWithByValArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithByValArgument.Target)).ContainsDelegateConditions(), Is.False);

		[Test]
		public void ContainsPointerTypeArguments() =>
			Assert.That(typeof(IHaveMethodWithPointerTypeArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithPointerTypeArgument.Target)).ContainsDelegateConditions(), Is.True);

		[Test]
		public void ContainsPointerTypeReturnType() =>
			Assert.That(typeof(IHaveMethodWithPointerTypeReturnType)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithPointerTypeReturnType.Target)).ContainsDelegateConditions(), Is.True);

		[Test]
		public void ContainsRuntimeArgumentHandle() =>
			Assert.That(typeof(IHaveMethodWithRuntimeArgumentHandleArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithRuntimeArgumentHandleArgument.Target)).ContainsDelegateConditions(), Is.True);

		[Test]
		public void ContainsTypedReference() =>
			Assert.That(typeof(IHaveMethodWithTypedReferenceArgument)
				.GetTypeInfo().GetMethod(nameof(IHaveMethodWithTypedReferenceArgument.Target)).ContainsDelegateConditions(), Is.True);
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
