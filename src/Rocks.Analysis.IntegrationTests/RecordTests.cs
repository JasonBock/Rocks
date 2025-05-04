using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.RecordTestTypes;

public record MyRecord
{
	public virtual void Foo() { }
}

public static class RecordTests
{
	[Test]
	public static void Create()
	{
		using var context = new RockContext();
		var expectations = context.Create<MyRecordCreateExpectations>();
		expectations.Methods.Foo();

		var mock = expectations.Instance();
		mock.Foo();
	}

	[Test]
	public static void Make()
	{
		var mock = new MyRecordMakeExpectations().Instance();
		Assert.That(mock.Foo, Throws.Nothing);
	}
}