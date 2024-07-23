using NUnit.Framework;

namespace Rocks.IntegrationTests.RecordTestTypes;

public record MyRecord
{
	public virtual void Foo() { }
}

public static class RecordTests
{
	[Test]
	public static void Create()
	{
		var expectations = new MyRecordCreateExpectations();
		expectations.Methods.Foo();

		var mock = expectations.Instance();
		mock.Foo();

		expectations.Verify();
	}

	[Test]
	public static void Make()
	{
		var mock = new MyRecordMakeExpectations().Instance();
		Assert.That(mock.Foo, Throws.Nothing);
	}
}