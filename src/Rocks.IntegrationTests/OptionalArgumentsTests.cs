using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IHaveOptionalArguments
{
	void Foo(int a, string b = "b", double c = 3.2);
	int this[int a, string b = "b"] { get; }
}

public interface IHaveOptionalStructDefaultArgument
{
	void Foo(OptionalDefault value = default);
}

public struct OptionalDefault { }

public static class OptionalArgumentsTests
{
	[Test]
	public static void CreateMembersWithOptionalDefaultStructArgument()
	{
		var expectations = Rock.Create<IHaveOptionalStructDefaultArgument>();
		expectations.Methods().Foo(Arg.IsDefault<OptionalDefault>());

		var mock = expectations.Instance();
		mock.Foo();

		expectations.Verify();
	}

	[Test]
	public static void CreateMembersWithOptionalArgumentsSpecified()
	{
		var returnValue = 3;
		var expectations = Rock.Create<IHaveOptionalArguments>();
		expectations.Methods().Foo(1, "b", 3.2);
		expectations.Indexers().Getters().This(1, "b").Returns(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithOptionalArguments()
	{
		var mock = Rock.Make<IHaveOptionalArguments>().Instance();
		var value = mock[1];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock.Foo(1), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateMembersWithOptionalArgumentsNotSpecified()
	{
		var returnValue = 3;
		var expectations = Rock.Create<IHaveOptionalArguments>();
		expectations.Methods().Foo(1, Arg.IsDefault<string>(), Arg.IsDefault<double>());
		expectations.Indexers().Getters().This(1, Arg.IsDefault<string>()).Returns(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}
}