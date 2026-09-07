using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests.Exceptions;

public static class VerificationExceptionTests
{
	[Test]
	public static void ThrowWithFailures()
	{
		try
		{
			throw new VerificationException(["failure one", "failure two"]);
		}
		catch (VerificationException e)
		{
			Assert.That(e.Message, 
				Is.EqualTo(
				"""
				The following verification failure(s) occurred:
				
				failure one
				failure two
				"""));
				
		}
	}

	[Test]
	public static void CreateWithNullMessages() => 
		Assert.That(() => new VerificationException(null!),
			Throws.TypeOf<ArgumentNullException>());
}