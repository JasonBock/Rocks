using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsRequiresExplicitCastTests
	{
		[Test]
		public void RequiresExplicitCastForReferenceType()
		{
			Assert.That(typeof(string).RequiresExplicitCast(), Is.False);
		}

		[Test]
		public void RequiresExplicitCastForValueType()
		{
			Assert.That(typeof(int).RequiresExplicitCast(), Is.True);
		}

		[Test]
		public void RequiresExplicitCastForGenericTypeWithReferenceConstraint()
		{
			Assert.That(typeof(IRequireCasts<,>).GetTypeInfo().GetMethod(
				nameof(IRequireCasts<string, int>.TargetWithConstraint)).ReturnType.RequiresExplicitCast(), Is.False);
		}

		[Test]
		public void RequiresExplicitCastForGenericTypeWithNoReferenceConstraint()
		{
			Assert.That(typeof(IRequireCasts<,>).GetTypeInfo().GetMethod(
				nameof(IRequireCasts<string, int>.TargetWithNoConstraint)).ReturnType.RequiresExplicitCast(), Is.True);
		}
	}

	public interface IRequireCasts<T, U> where T : class
	{
		T TargetWithConstraint();
		U TargetWithNoConstraint();
	}
}