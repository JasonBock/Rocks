using NUnit.Framework;
using Rocks.Extensions;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsTests
	{
		[Test]
		public void ContainsOutInitializers() =>
			Assert.That(this.GetType().GetTypeInfo().GetMethod(
				nameof(this.TargetWithOutArgument)).GetOutInitializers(), 
				Is.EqualTo("a = default(int);"));

		[Test]
		public void ContainsOutInitializersWhenArgumentTypeIsArray() =>
			Assert.That(this.GetType().GetTypeInfo().GetMethod(
				nameof(this.TargetWithOutArrayArgument)).GetOutInitializers(),
				Is.EqualTo("a = default(int[]);"));

		public void TargetWithOutArgument(out int a) => a = 0; 
		public void TargetWithOutArrayArgument(out int[] a) => a = null; 
	}
}
