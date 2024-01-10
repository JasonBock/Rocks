using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class Requireds
{
	public virtual void Foo() { }

	public required int NonNullableValueType { get; set; }
	public required int? NullableValueType { get; init; }
	public required string NonNullableReferenceType { get; init; }
	public required string? NullableReferenceType { get; init; }
}

public class Inits
{
	public virtual void Foo() { }

	public int NonNullableValueType { get; init; }
	public int? NullableValueType { get; init; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public string NonNullableReferenceType { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public string? NullableReferenceType { get; init; }
}

public static class RequiredInitPropertyTests
{
	[Test]
	[RockCreate<Inits>]
	public static void InitPropertiesWithCreate()
	{
		var expectations = new InitsCreateExpectations();
		expectations.Methods.Foo();

		var mock = expectations.Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		});

		expectations.Verify();
	}

	[Test]
	[RockMake<Inits>]
	public static void InitPropertiesWithMake()
	{
		var mock = new InitsMakeExpectations().Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		});
	}

	[Test]
	[RockCreate<Inits>]
	public static void InitPropertiesWithNullWithCreate()
	{
		var expectations = new InitsCreateExpectations();
		expectations.Methods.Foo();

		var mock = expectations.Instance(null);
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(0));
			Assert.That(mock.NullableValueType, Is.Null);
			Assert.That(mock.NonNullableReferenceType, Is.Null);
			Assert.That(mock.NullableReferenceType, Is.Null);
		});

		expectations.Verify();
	}

	[Test]
	[RockMake<Inits>]
	public static void InitPropertiesWithNullWithMake()
	{
		var mock = new InitsMakeExpectations().Instance(null);
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(0));
			Assert.That(mock.NullableValueType, Is.Null);
			Assert.That(mock.NonNullableReferenceType, Is.Null);
			Assert.That(mock.NullableReferenceType, Is.Null);
		});
	}

	[Test]
	[RockCreate<Requireds>]
	public static void RequiredPropertiesWithCreate()
	{
		var expectations = new RequiredsCreateExpectations();
		expectations.Methods.Foo();

		var mock = expectations.Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		});

		expectations.Verify();
	}

	[Test]
	[RockMake<Requireds>]
	public static void RequiredPropertiesWithMake()
	{
		var mock = new RequiredsMakeExpectations().Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		Assert.Multiple(() =>
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		});
	}

	[Test]
	[RockCreate<Requireds>]
	public static void RequiredPropertiesWithNullWithCreate()
	{
		var expectations = new RequiredsCreateExpectations();
		expectations.Methods.Foo();

		Assert.That(() => expectations.Instance(null!), Throws.TypeOf<ArgumentNullException>());
	}

	[Test]
	[RockMake<Requireds>]
	public static void RequiredPropertiesWithNullWithMake() => 
		Assert.That(() => new RequiredsMakeExpectations().Instance(null!), Throws.TypeOf<ArgumentNullException>());
}