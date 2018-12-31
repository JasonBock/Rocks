using NUnit.Framework;

namespace Rocks.Tests
{
	public static class ErrorMessagesTests
	{
		[Test]
		public static void GetCannotMockSealedType() =>
			Assert.That(ErrorMessages.GetCannotMockSealedType("a"),
				Is.EqualTo("Cannot mock the sealed type a."));

		[Test]
		public static void GetVerificationFailed() =>
			Assert.That(ErrorMessages.GetVerificationFailed("a", "b", "c"),
				Is.EqualTo("Type: a, method: b, message: c"));
	}
}
