using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassConstructorTestTypes;

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
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassConstructorCreateExpectations>();
		expectations.Setups.NoParameters();

		var mock = expectations.Instance(3);
		var value = mock.NoParameters();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.IntData, Is.EqualTo(3));
			Assert.That(mock.StringData, Is.Null);
		}
	}

	[Test]
	public static void MakeWithNoParametersAndPublicConstructor()
	{
		var mock = new AbstractClassConstructorMakeExpectations().Instance(3);
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithNoParametersAndProtectedConstructor()
	{
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassConstructorCreateExpectations>();
		expectations.Setups.NoParameters();

		var mock = expectations.Instance("b");
		var value = mock.NoParameters();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.IntData, Is.Default);
			Assert.That(mock.StringData, Is.EqualTo("b"));
		}
	}

	[Test]
	public static void MakeWithNoParametersAndProtectedConstructor()
	{
		var mock = new AbstractClassConstructorMakeExpectations().Instance("b");
		var value = mock.NoParameters();

		Assert.That(value, Is.Default);
	}
}