using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ErrorMessagesTests
	{
		[Test]
		public void GetCannotMockSealedType()
		{
			Assert.AreEqual("Cannot mock the sealed type a.", ErrorMessages.GetCannotMockSealedType("a"));
		}

		[Test]
		public void GetNoVirtualMembers()
		{
			Assert.AreEqual("No public virtual members found on type a.", ErrorMessages.GetNoVirtualMembers("a"));
		}

		[Test]
		public void GetVerificationFailed()
		{
			Assert.AreEqual("Type: a, method: b, message: c", ErrorMessages.GetVerificationFailed("a", "b", "c"));
		}
	}
}
