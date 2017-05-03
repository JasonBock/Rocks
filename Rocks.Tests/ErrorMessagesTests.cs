using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ErrorMessagesTests
	{
		[Test]
		public void GetCannotMockSealedType() =>
			Assert.That(ErrorMessages.GetCannotMockSealedType("a"),
				Is.EqualTo("Cannot mock the sealed type a."));

		[Test]
		public void GetVerificationFailed() =>
			Assert.That(ErrorMessages.GetVerificationFailed("a", "b", "c"),
				Is.EqualTo("Type: a, method: b, message: c"));
	}
}
