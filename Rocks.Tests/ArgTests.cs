using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class ArgTests
	{
		[Test]
		public static void CallIs()
		{
			var evaluation = new Func<string, bool>(_ => !string.IsNullOrWhiteSpace(_));
			Assert.That(Arg.Is<string>(evaluation), Is.EqualTo(default(string)), nameof(Arg.Is));
		}

		[Test]
		public static void CallIsWithNullEvaluation() =>
			Assert.That(() => Arg.Is<string>(null), Throws.TypeOf<ArgumentNullException>(), nameof(Arg.Is));

		[Test]
		public static void CallIsAny() =>
			Assert.That(Arg.IsAny<string>(), Is.EqualTo(default(string)), nameof(Arg.IsAny));

		[Test]
		public static void CallIsDefault() =>
			Assert.That(Arg.IsDefault<string>(), Is.EqualTo(default(string)), nameof(Arg.IsDefault));
	}
}
