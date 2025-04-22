using NUnit.Framework;
using Rocks.Runtime.Exceptions;

namespace Rocks.Runtime.Tests.Exceptions;

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
			Assert.That(e.Message, Is.EqualTo("The following verification failure(s) occured: failure one, failure two"));
		}
	}
}
