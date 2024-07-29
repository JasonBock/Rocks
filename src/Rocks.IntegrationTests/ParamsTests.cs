using NUnit.Framework;

namespace Rocks.IntegrationTests.ParamsTestTypes;

// TODO: when this issue is worked on,
// add a "params ReadOnlySpan<string>" -
// https://github.com/JasonBock/Rocks/issues/170
public interface IHaveParams
{
	void Foo(int a, params string[] b);
	int this[int a, params string[] b] { get; }
	void ParamsFoo(int a, params ReadOnlySpan<string> b);
}

public static class ParamsTests
{
	[Test]
	public static void CreateMembersWithReadOnlySpanParamsArgumentsSpecified()
	{
		var expectations = new IHaveParamsCreateExpectations();
		expectations.Methods.ParamsFoo(1, 
			new(_ => _.Length == 2 && _[0] == "b" && _[1] == "c"));

		var mock = expectations.Instance();
		mock.ParamsFoo(1, "b", "c");

		expectations.Verify();
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
		var expectations = new IHaveParamsCreateExpectations();
		expectations.Methods.Foo(1, new[] { "b" });
		expectations.Indexers.Getters.This(1, new[] { "b" }).ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1, "b");
		var value = mock[1, "b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithParamsArgumentsSpecified()
	{
		var mock = new IHaveParamsMakeExpectations().Instance();
		var value = mock[1, "b"];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock.Foo(1, "b"), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateMembersWithParamsArgumentsNotSpecified()
	{
		var returnValue = 3;
		var expectations = new IHaveParamsCreateExpectations();
		expectations.Methods.Foo(1, Array.Empty<string>());
		expectations.Indexers.Getters.This(1, Array.Empty<string>()).ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithParamsArgumentsNotSpecified()
	{
		var mock = new IHaveParamsMakeExpectations().Instance();
		var value = mock[1];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock.Foo(1), Throws.Nothing);
		});
	}
}