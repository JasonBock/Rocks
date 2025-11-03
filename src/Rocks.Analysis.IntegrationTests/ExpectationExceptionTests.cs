using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.ExpectationExceptionTestTypes;

public interface ISetAfterMock
{
	void Work();
	void WorkWithData(string data1, int data2, char[] data3);
	string Data { get; set; }
	int this[string Index] { get; set; }
}

internal static class ExpectationExceptionTests
{
	[Test]
	public static void GetExceptionMessage()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups.WorkWithData("a", 2, new[] { 'c', 'd' });

		var mock = expectations.Instance();
		Assert.That(() => mock.WorkWithData("b", 3, ['e', 'f', 'g']),
			Throws.TypeOf<ExpectationException>()
				.With.Message.EqualTo(
					"""
					No handlers match for Void WorkWithData(System.String, Int32, Char[])
						data1: b
						data2: 3
						data3: System.Char[], Count = 3
					"""));
	}

	[Test]
	public static void SetMethodExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups.Work();

		var mock = expectations.Instance();

		Assert.That(() => expectations.Setups.Work(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertyGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups.Data.Gets();

		var mock = expectations.Instance();

		Assert.That(() => expectations.Setups.Data.Gets(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertySetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups.Data.Sets("a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Setups.Data.Sets("a"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups["a"].Gets();

		var mock = expectations.Instance();

		Assert.That(() => expectations.Setups["a"].Gets(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerSetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Setups["a"].Sets(1);

		var mock = expectations.Instance();

		Assert.That(() => expectations.Setups["a"].Sets(1), Throws.TypeOf<ExpectationException>());
	}
}