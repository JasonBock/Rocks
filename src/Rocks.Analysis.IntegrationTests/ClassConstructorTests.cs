using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.IntegrationTests.ClassConstructorTestTypes;

public class ClassConstructorWithSpecialParameters
{
	public ClassConstructorWithSpecialParameters(int a, ref string b, out string c, params string[] d)
	{
		c = "42";
		(this.A, this.B, this.C, this.D) = (a, b, c, d);
	}

	public virtual void Foo() { }

	public int A { get; }

	public string B { get; }

	public string C { get; }

#pragma warning disable CA1819 // Properties should not return arrays
	public string[] D { get; }
#pragma warning restore CA1819 // Properties should not return arrays
}

public class ClassConstructor
{
	protected ClassConstructor(string stringData) =>
		this.StringData = stringData;
	public ClassConstructor(int intData) =>
		this.IntData = intData;

	public virtual int NoParameters() => default;

	public int IntData { get; }
	public string? StringData { get; }
}

public class ClassRequiredConstructor
{
	[SetsRequiredMembers] 
	public ClassRequiredConstructor() { }
	
	public virtual void DoRequired() { }
}

public static class ClassConstructorTests
{
	[Test]
	public static void CreateRequiredConstructor()
	{
		var expectations = new ClassRequiredConstructorCreateExpectations();
		expectations.Methods.DoRequired();

		var mock = expectations.Instance();
		mock.DoRequired();

		expectations.Verify();
	}

	[Test]
	public static void CreateSpecialConstructor()
	{
		var bValue = "b";
		var d1Value = "d1";
		var d2Value = "d2";

		var expectations = new ClassConstructorWithSpecialParametersCreateExpectations();
		expectations.Methods.Foo();

		var mock = expectations.Instance(2, ref bValue, out var cValue, d1Value, d2Value);
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.A, Is.EqualTo(2));
			Assert.That(mock.B, Is.EqualTo("b"));
			Assert.That(mock.C, Is.EqualTo("42"));
			Assert.That(cValue, Is.EqualTo("42"));
			Assert.That(mock.D, Is.EquivalentTo(new[] { d1Value, d2Value }));
		});

		expectations.Verify();
	}

	[Test]
	public static void MakeSpecialConstructor()
	{
		var bValue = "b";
		var d1Value = "d1";
		var d2Value = "d2";

		var mock = new ClassConstructorWithSpecialParametersMakeExpectations().Instance(2, ref bValue, out var cValue, d1Value, d2Value);

		Assert.Multiple(() =>
		{
			Assert.That(mock.A, Is.EqualTo(2));
			Assert.That(mock.B, Is.EqualTo("b"));
			Assert.That(mock.C, Is.EqualTo("42"));
			Assert.That(cValue, Is.EqualTo("42"));
			Assert.That(mock.D, Is.EquivalentTo(new[] { d1Value, d2Value }));
		});
	}

	[Test]
	public static void CreateWithNoParametersAndPublicConstructor()
	{
		var expectations = new ClassConstructorCreateExpectations();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance(3);
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(mock.IntData, Is.EqualTo(3));
			Assert.That(mock.StringData, Is.Null);
		});
	}

	[Test]
	public static void MakeWithNoParametersAndPublicConstructor()
	{
		var mock = new ClassConstructorMakeExpectations().Instance(3);
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithNoParametersAndProtectedConstructor()
	{
		var expectations = new ClassConstructorCreateExpectations();
		expectations.Methods.NoParameters();

		var mock = expectations.Instance("b");
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(mock.IntData, Is.EqualTo(default(int)));
			Assert.That(mock.StringData, Is.EqualTo("b"));
		});
	}

	[Test]
	public static void MakeWithNoParametersAndProtectedConstructor()
	{
		var mock = new ClassConstructorMakeExpectations().Instance("b");
		var value = mock.NoParameters();

		Assert.That(value, Is.EqualTo(default(int)));
	}
}