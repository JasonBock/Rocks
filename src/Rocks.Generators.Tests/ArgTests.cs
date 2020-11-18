using NUnit.Framework;

namespace Rocks.Tests
{
	public static class ArgTests
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
	}
}