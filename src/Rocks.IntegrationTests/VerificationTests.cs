using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public interface IData
{
	string Value { get; set; }
}

public static class VerificationTests
{
	[Test]
	public static void VerifyWhenPropertyExpectationIsNotMet()
	{
		var rock = Rock.Create<IData>();
		rock.Properties().Setters().Value("3");

		_ = rock.Instance();

		Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
	}
}