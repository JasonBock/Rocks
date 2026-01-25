using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ParamsTestTypes;

// TODO: when this issue is worked on,
// add a "params ReadOnlySpan<string>" -
// https://github.com/JasonBock/Rocks/issues/170
public interface IHaveParams
{
	void Foo(int a, params string[] b);
	int this[int a, params string[] b] { get; }
	void ParamsFoo(int a, params ReadOnlySpan<string> b);
}

internal static class ParamsTests
{
	[Test]
	public static void CreateMembersWithReadOnlySpanParamsArgumentsSpecified()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveParamsCreateExpectations>();
		expectations.Setups.ParamsFoo(1,
			new RefStructArgument<ReadOnlySpan<string>>(_ => _.Length == 2 && _[0] == "b" && _[1] == "c"));

		var mock = expectations.Instance();
		mock.ParamsFoo(1, "b", "c");
	}

	[Test]
	public static void CreateX()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveParamsCreateExpectations>();
		expectations.Setups.ParamsFoo(1, "b", "c");

		var mock = expectations.Instance();
		mock.ParamsFoo(1, "b", "c");
	}

	[Test]
	public static void MakeMembersWithReadOnlySpanParamsArgumentsSpecified()
	{
		var mock = new IHaveParamsMakeExpectations().Instance();
		mock.ParamsFoo(1, "b", "c");
	}

	[Test]
	public static void CreateMembersWithParamsArgumentsSpecified()
	{
		var returnValue = 3;
		using var context = new RockContext();
		var expectations = context.Create<IHaveParamsCreateExpectations>();
		expectations.Setups.Foo(1, new[] { "b" });
		expectations.Setups[1, new[] { "b" }].Gets().ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1, "b");
		var value = mock[1, "b"];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithParamsArgumentsSpecified()
	{
		var mock = new IHaveParamsMakeExpectations().Instance();
		var value = mock[1, "b"];

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.Default);
			Assert.That(() => mock.Foo(1, "b"), Throws.Nothing);
		}
	}

	[Test]
	public static void CreateMembersWithParamsArgumentsNotSpecified()
	{
		var returnValue = 3;
		using var context = new RockContext();
		var expectations = context.Create<IHaveParamsCreateExpectations>();
		expectations.Setups.Foo(1, Array.Empty<string>());
		expectations.Setups[1, Array.Empty<string>()].Gets().ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithParamsArgumentsNotSpecified()
	{
		var mock = new IHaveParamsMakeExpectations().Instance();
		var value = mock[1];

		using (Assert.EnterMultipleScope())
		{
			Assert.That(value, Is.Default);
			Assert.That(() => mock.Foo(1), Throws.Nothing);
		}
	}
}