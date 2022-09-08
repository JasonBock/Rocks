using NUnit.Framework;

namespace Rocks.IntegrationTests;

public record MyRecord
{
	public virtual void Foo() { }
}

public static class RecordTests
{
	[Test]
	public static void Create()
	{
		var expectations = Rock.Create<MyRecord>();
		expectations.Methods().Foo();

		var mock = expectations.Instance();
		mock.Foo();

		expectations.Verify();
	}

	[Test]
	public static void Make()
	{
		var mock = Rock.Make<MyRecord>().Instance();
		Assert.That(mock.Foo, Throws.Nothing);
	}
}