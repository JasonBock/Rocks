using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsRequiresExplicitCastTests
	{
		[Test]
		public void RequiresExplicitCastForReferenceType()
		{
			Assert.IsFalse(typeof(string).RequiresExplicitCast());
		}

		[Test]
		public void RequiresExplicitCastForValueType()
		{
			Assert.IsTrue(typeof(int).RequiresExplicitCast());
		}

		[Test]
		public void RequiresExplicitCastForGenericTypeWithReferenceConstraint()
		{
			Assert.IsFalse(typeof(IRequireCasts<,>).GetMethod(nameof(IRequireCasts<string, int>.TargetWithConstraint)).ReturnType.RequiresExplicitCast());
		}

		[Test]
		public void RequiresExplicitCastForGenericTypeWithNoReferenceConstraint()
		{
			Assert.IsTrue(typeof(IRequireCasts<,>).GetMethod(nameof(IRequireCasts<string, int>.TargetWithNoConstraint)).ReturnType.RequiresExplicitCast());
		}
	}

	public interface IRequireCasts<T, U> where T : class
	{
		T TargetWithConstraint();
		U TargetWithNoConstraint();
	}
}