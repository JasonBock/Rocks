using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ArgTestTypes;

public interface IHaveArgument
{
	void Foo(int a);
	void Bar(int a = 3);
	string this[int a] { get; set; }
}

internal static class ArgTests
{
	[Test]
	public static void DeclareArgumentFromIndexerWithNull()
	{
		var expectations = new IHaveArgumentCreateExpectations();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(() => expectations.Setups[null!].Gets(), Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => expectations.Setups[null!].Sets("value"), Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => expectations.Setups[1].Sets(null!), Throws.TypeOf<ArgumentNullException>());
		};
	}

	[Test]
	public static void DeclareArgumentFromMethodWithNull()
	{
		var expectations = new IHaveArgumentCreateExpectations();
		Assert.That(() => expectations.Setups.Foo(null!), Throws.TypeOf<ArgumentNullException>());
	}

	[Test]
	public static void DeclareArgumentWithValue()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveArgumentCreateExpectations>();
		expectations.Setups.Foo(3);

		var mock = expectations.Instance();
		mock.Foo(3);
	}

	[Test]
	public static void DeclareArgumentWithIs()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveArgumentCreateExpectations>();
		expectations.Setups.Foo(Arg.Is(3));

		var mock = expectations.Instance();
		mock.Foo(3);
	}

	[Test]
	public static void DeclareArgumentWithAny()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveArgumentCreateExpectations>();
		expectations.Setups.Foo(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Foo(3);
	}

	[Test]
	public static void DeclareArgumentWithValidate()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveArgumentCreateExpectations>();
		expectations.Setups.Foo(Arg.Validate<int>(_ => _ is > 20 and < 30));

		var mock = expectations.Instance();
		mock.Foo(25);
	}

	[Test]
	public static void DeclareArgumentWithDefault()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveArgumentCreateExpectations>();
		expectations.Setups.Bar(Arg.IsDefault<int>());

		var mock = expectations.Instance();
		mock.Bar(3);
	}
}