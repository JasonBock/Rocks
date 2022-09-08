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
		var expectations = Rock.Create<AbstractClassConstructor>();
		expectations.Methods().NoParameters();

		var mock = expectations.Instance(3);
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(mock.IntData, Is.EqualTo(3));
			Assert.That(mock.StringData, Is.Null);
		});
	}

	[Test]
	public static void MakeWithNoParametersAndPublicConstructor()
	{
		var mock = Rock.Make<AbstractClassConstructor>().Instance(3);
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithNoParametersAndProtectedConstructor()
	{
		var expectations = Rock.Create<AbstractClassConstructor>();
		expectations.Methods().NoParameters();

		var mock = expectations.Instance("b");
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(mock.IntData, Is.EqualTo(default(int)));
			Assert.That(mock.StringData, Is.EqualTo("b"));
		});
	}

	[Test]
	public static void MakeWithNoParametersAndProtectedConstructor()
	{
		var mock = Rock.Make<AbstractClassConstructor>().Instance("b");
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}
}