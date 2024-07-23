using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests.ExpectationExceptionTestTypes;

public interface ISetAfterMock
{
	void Work();
	string Data { get; set; }
	int this[string Index] { get; set; }
}

public static class ExpectationExceptionTests
{
	[Test]
	public static void SetMethodExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Methods.Work();

		var mock = expectations.Instance();

		Assert.That(() => expectations.Methods.Work(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertyGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Properties.Getters.Data();

		var mock = expectations.Instance();

		Assert.That(() => expectations.Properties.Getters.Data(), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertySetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Properties.Setters.Data("a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Properties.Setters.Data("a"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Indexers.Getters.This("a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Indexers.Getters.This("a"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerSetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Indexers.Setters.This(1, "a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Indexers.Setters.This(1, "a"), Throws.TypeOf<ExpectationException>());
	}
}