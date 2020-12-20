using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class ArgumentTests
	{
		[Test]
		public static void CreateViaAny()
		{
			var arg = Arg.Any<int>();

			Assert.Multiple(() =>
			{
				Assert.That(arg.IsValid(33), Is.True);
			});
		}

		[Test]
		public static void CreateViaIs()
		{
			var arg = Arg.Is(3);

			Assert.Multiple(() =>
			{
				Assert.That(arg.IsValid(3), Is.True);
				Assert.That(arg.IsValid(5), Is.False);
			});
		}

		[Test]
		public static void CreateViaValidate()
		{
			var arg = Arg.Validate<int>(_ => _ % 2 == 0);

			Assert.Multiple(() =>
			{
				Assert.That(arg.IsValid(2), Is.True);
				Assert.That(arg.IsValid(3), Is.False);
			});
		}

		[Test]
		public static void CreateViaIsDefault()
		{
			var arg = Arg.IsDefault<int>();

			Assert.Multiple(() =>
			{
				Assert.That(() => arg.IsValid(2), Throws.TypeOf<NotSupportedException>());
			});
		}

		[Test]
		public static void TransformFromDefaultValueState()
		{
			var arg = Arg.IsDefault<int>();
			var transformedArg = arg.Transform(2);

			Assert.Multiple(() =>
			{
				Assert.That(arg, Is.Not.SameAs(transformedArg));
				Assert.That(() => transformedArg.IsValid(2), Is.True);
				Assert.That(() => transformedArg.IsValid(3), Is.False);
			});
		}

		[Test]
		public static void TransformFromNonDefaultValueState()
		{
			var arg = Arg.Is(3);
			var transformedArg = arg.Transform(2);

			Assert.Multiple(() =>
			{
				Assert.That(arg, Is.SameAs(transformedArg));
				Assert.That(() => transformedArg.IsValid(2), Is.False);
				Assert.That(() => transformedArg.IsValid(3), Is.True);
			});
		}
	}
}