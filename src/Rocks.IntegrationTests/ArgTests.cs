using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IHaveArgument
{
	void Foo(int a);
	void Bar(int a = 3);
}

public static class ArgTests
{
	[Test]
	public static void DeclareArgumentWithNull()
	{
		var expectations = Rock.Create<IHaveArgument>();
		Assert.That(() => expectations.Methods().Foo(null!), Throws.TypeOf<ArgumentNullException>());
	}

	[Test]
	public static void DeclareArgumentWithValue()
	{
		var expectations = Rock.Create<IHaveArgument>();
		expectations.Methods().Foo(3);

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	public static void DeclareArgumentWithIs()
	{
		var expectations = Rock.Create<IHaveArgument>();
		expectations.Methods().Foo(Arg.Is(3));

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	public static void DeclareArgumentWithAny()
	{
		var expectations = Rock.Create<IHaveArgument>();
		expectations.Methods().Foo(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Foo(3);

		expectations.Verify();
	}

	[Test]
	public static void DeclareArgumentWithValidate()
	{
		var expectations = Rock.Create<IHaveArgument>();
		expectations.Methods().Foo(Arg.Validate<int>(_ => _ > 20 && _ < 30));

		var mock = expectations.Instance();
		mock.Foo(25);

		expectations.Verify();
	}

	[Test]
	public static void DeclareArgumentWithDefault()
	{
		var expectations = Rock.Create<IHaveArgument>();
		expectations.Methods().Bar(Arg.IsDefault<int>());

		var mock = expectations.Instance();
		mock.Bar(3);

		expectations.Verify();
	}
}