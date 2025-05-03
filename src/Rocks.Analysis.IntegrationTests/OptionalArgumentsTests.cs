using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.OptionalArgumentsTestTypes;

#nullable disable
public class NeedNullableAnnotation
{
	public NeedNullableAnnotation(object initializationData = null) { }

	public virtual int IntReturn(object initializationData = null) => 
		initializationData is null ? 1 : 0;
	public virtual void VoidReturn(object initializationData = null) =>
		this.VoidReturnData = initializationData;
#nullable restore

	public object? VoidReturnData { get; private set; }
}

public interface IHaveOptionalArguments
{
	void Foo(int a, string b = "b", double c = 3.2);
	int this[int a, string b = "b"] { get; set; }
}

public interface IHaveOptionalStructDefaultArgument
{
	void Foo(OptionalDefault value = default);
}

public struct OptionalDefault { }

public static class OptionalArgumentsTests
{
	[Test]
	public static void CreateForcedNullableAnnotation()
	{
		var expectations = new NeedNullableAnnotationCreateExpectations();
		expectations.Methods.IntReturn(Arg.Is<object?>(null));
		expectations.Methods.VoidReturn(Arg.Is<object?>(null));

		var mock = expectations.Instance(Arg.Is<object?>(null));
		_ = mock.IntReturn();
		mock.VoidReturn();

		expectations.Verify();
	}

	[Test]
	public static void MakeForcedNullableAnnotation()
	{
		var mock = new NeedNullableAnnotationMakeExpectations().Instance(Arg.Is<object?>(null));
		_ = mock.IntReturn();
		mock.VoidReturn();
	}

	[Test]
	public static void CreateMembersWithOptionalDefaultStructArgument()
	{
		var expectations = new IHaveOptionalStructDefaultArgumentCreateExpectations();
		expectations.Methods.Foo(Arg.IsDefault<OptionalDefault>());

		var mock = expectations.Instance();
		mock.Foo();

		expectations.Verify();
	}

	[Test]
	public static void CreateMembersWithOptionalArgumentsSpecified()
	{
		var returnValue = 3;
		var expectations = new IHaveOptionalArgumentsCreateExpectations();
		expectations.Methods.Foo(1, "b", 3.2);
		expectations.Indexers.Getters.This(1, "b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeMembersWithOptionalArguments()
	{
		var mock = new IHaveOptionalArgumentsMakeExpectations().Instance();
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
		var expectations = new IHaveOptionalArgumentsCreateExpectations();
		expectations.Methods.Foo(1, Arg.IsDefault<string>(), Arg.IsDefault<double>());
		expectations.Indexers.Getters.This(1, Arg.IsDefault<string>()).ReturnValue(returnValue);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[1];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateMembersWithOptionalArgumentsNotSpecifiedUsingOverload()
	{
		var returnValue = 3;
		var expectations = new IHaveOptionalArgumentsCreateExpectations();
		expectations.Methods.Foo(1);
		expectations.Indexers.Getters.This(2).ReturnValue(returnValue);
		expectations.Indexers.Setters.This(52, 3);

		var mock = expectations.Instance();
		mock.Foo(1);
		var value = mock[2];
		mock[3] = 52;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}
}