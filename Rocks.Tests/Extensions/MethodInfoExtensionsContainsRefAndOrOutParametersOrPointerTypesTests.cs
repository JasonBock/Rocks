using NUnit.Framework;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsContainsRefAndOrOutParametersOrPointerTypesTests
	{
		[Test]
		public void ContainsRefArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithRefArgument)
				.GetMethod(nameof(IHaveMethodWithRefArgument.Target)).ContainsRefAndOrOutParametersOrPointerTypes());
		}

		[Test]
		public void ContainsOutArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithOutArgument)
				.GetMethod(nameof(IHaveMethodWithOutArgument.Target)).ContainsRefAndOrOutParametersOrPointerTypes());
		}

		[Test]
		public void ContainsByValArguments()
		{
			Assert.IsFalse(typeof(IHaveMethodWithByValArgument)
				.GetMethod(nameof(IHaveMethodWithByValArgument.Target)).ContainsRefAndOrOutParametersOrPointerTypes());
		}

		[Test]
		public void ContainsPointerTypeArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithPointerTypeArgument)
				.GetMethod(nameof(IHaveMethodWithPointerTypeArgument.Target)).ContainsRefAndOrOutParametersOrPointerTypes());
		}

		[Test]
		public void ContainsPointerTypeReturnType()
		{
			Assert.IsTrue(typeof(IHaveMethodWithPointerTypeReturnType)
				.GetMethod(nameof(IHaveMethodWithPointerTypeReturnType.Target)).ContainsRefAndOrOutParametersOrPointerTypes());
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
}
