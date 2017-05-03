using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ArgTests
	{
		[Test]
		public void CallIs()
		{
			var evaluation = new Func<string, bool>(_ => !string.IsNullOrWhiteSpace(_));
			Assert.That(Arg.Is<string>(evaluation), Is.EqualTo(default(string)), nameof(Arg.Is));
		}

		[Test]
		public void CallIsWithNullEvaluation() =>
			Assert.That(() => Arg.Is<string>(null), Throws.TypeOf<ArgumentNullException>(), nameof(Arg.Is));

		[Test]
		public void CallIsAny() =>
			Assert.That(Arg.IsAny<string>(), Is.EqualTo(default(string)), nameof(Arg.IsAny));

		[Test]
		public void CallIsDefault() =>
			Assert.That(Arg.IsDefault<string>(), Is.EqualTo(default(string)), nameof(Arg.IsDefault));
	}
}
