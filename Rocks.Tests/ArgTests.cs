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
			Assert.AreEqual(default(string), Arg.Is<string>(evaluation), nameof(Arg.Is));
		}

		[Test]
		public void CallIsWithNullEvaluation()
		{
			Assert.Throws<ArgumentNullException>(() => Arg.Is<string>(null), nameof(Arg.Is));
		}

		[Test]
		public void CallIsAny()
		{
			Assert.AreEqual(default(string), Arg.IsAny<string>(), nameof(Arg.IsAny));
		}

		[Test]
		public void CallIsDefault()
		{
			Assert.AreEqual(default(string), Arg.IsDefault<string>(), nameof(Arg.IsDefault));
		}
	}
}
