using NUnit.Framework;

namespace Rocks.IntegrationTests;

#pragma warning disable CA1012 // Abstract types should not have public constructors
public abstract class AbstractClassConstructor
#pragma warning restore CA1012 // Abstract types should not have public constructors
{
	protected AbstractClassConstructor(string stringData) =>
		this.StringData = stringData;
	public AbstractClassConstructor(int intData) =>
		this.IntData = intData;

	public abstract int NoParameters();

	public int IntData { get; }
	public string? StringData { get; }
}

public static class AbstractClassConstructorTests
{
	[Test]
	public static void CreateWithNoParametersAndPublicConstructor()
	{
		var rock = Rock.Create<AbstractClassConstructor>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance(3);
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(chunk.IntData, Is.EqualTo(3));
			Assert.That(chunk.StringData, Is.Null);
		});
	}

	[Test]
	public static void MakeWithNoParametersAndPublicConstructor()
	{
		var chunk = Rock.Make<AbstractClassConstructor>().Instance(3);
		var value = chunk.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithNoParametersAndProtectedConstructor()
	{
		var rock = Rock.Create<AbstractClassConstructor>();
		rock.Methods().NoParameters();

		var chunk = rock.Instance("b");
		var value = chunk.NoParameters();

		rock.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(chunk.IntData, Is.EqualTo(default(int)));
			Assert.That(chunk.StringData, Is.EqualTo("b"));
		});
	}

	[Test]
	public static void MakeWithNoParametersAndProtectedConstructor()
	{
		var chunk = Rock.Make<AbstractClassConstructor>().Instance("b");
		var value = chunk.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}
}