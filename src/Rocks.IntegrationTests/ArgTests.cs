using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IHaveArgument
{
	void Foo(int a);
	void Bar(int a = 3);
	string this[int a] { get; set; }
}

public static class ArgTests
{
	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentFromIndexerWithNull()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		Assert.Multiple(() =>
		{
			Assert.That(() => expectations.Indexers.Getters.This(null!), Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => expectations.Indexers.Setters.This("value", null!), Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => expectations.Indexers.Setters.This(null!, 1), Throws.TypeOf<ArgumentNullException>());
		});
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentFromMethodWithNull()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		Assert.That(() => expectations.Methods.Foo(null!), Throws.TypeOf<ArgumentNullException>());
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentWithValue()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		expectations.Methods.Foo(3);

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentWithIs()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		expectations.Methods.Foo(Arg.Is(3));

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentWithAny()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		expectations.Methods.Foo(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentWithValidate()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		expectations.Methods.Foo(Arg.Validate<int>(_ => _ > 20 && _ < 30));

		var mock = expectations.Instance();
		mock.Foo(25);

		expectations.Verify();
	}

	[Test]
	[RockCreate<IHaveArgument>]
	public static void DeclareArgumentWithDefault()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		expectations.Methods.Bar(Arg.IsDefault<int>());

		var mock = expectations.Instance();
		mock.Bar(3);

		expectations.Verify();
	}
}