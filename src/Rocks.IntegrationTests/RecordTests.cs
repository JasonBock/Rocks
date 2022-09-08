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
		var rock = Rock.Create<MyRecord>();
		rock.Methods().Foo();

		var chunk = rock.Instance();
		chunk.Foo();

		rock.Verify();
	}

	[Test]
	public static void Make()
	{
		var chunk = Rock.Make<MyRecord>().Instance();
		Assert.That(chunk.Foo, Throws.Nothing);
	}
}