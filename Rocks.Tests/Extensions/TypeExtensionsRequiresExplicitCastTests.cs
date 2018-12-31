using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsRequiresExplicitCastTests
	{
		[Test]
		public static void RequiresExplicitCastForReferenceType() =>
			Assert.That(typeof(string).RequiresExplicitCast(), Is.False);

		[Test]
		public static void RequiresExplicitCastForValueType() =>
			Assert.That(typeof(int).RequiresExplicitCast(), Is.True);

		[Test]
		public static void RequiresExplicitCastForGenericTypeWithReferenceConstraint() =>
			Assert.That(typeof(IRequireCasts<,>).GetMethod(
				nameof(IRequireCasts<string, int>.TargetWithConstraint)).ReturnType.RequiresExplicitCast(), Is.False);

		[Test]
		public static void RequiresExplicitCastForGenericTypeWithNoReferenceConstraint() =>
			Assert.That(typeof(IRequireCasts<,>).GetMethod(
				nameof(IRequireCasts<string, int>.TargetWithNoConstraint)).ReturnType.RequiresExplicitCast(), Is.True);
	}

	public interface IRequireCasts<T, U> where T : class
	{
		T TargetWithConstraint();
		U TargetWithNoConstraint();
	}
}