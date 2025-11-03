using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.RequiredInitPropertyTestTypes;

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

internal static class RequiredInitPropertyTests
{
	[Test]
	public static void InitPropertiesWithCreate()
	{
		using var context = new RockContext();
		var expectations = context.Create<InitsCreateExpectations>();
		expectations.Setups.Foo();

		var mock = expectations.Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		}
	}

	[Test]
	public static void InitPropertiesWithMake()
	{
		var mock = new InitsMakeExpectations().Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		}
	}

	[Test]
	public static void InitPropertiesWithNullWithCreate()
	{
		using var context = new RockContext();
		var expectations = context.Create<InitsCreateExpectations>();
		expectations.Setups.Foo();

		var mock = expectations.Instance(null);
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.Zero);
			Assert.That(mock.NullableValueType, Is.Null);
			Assert.That(mock.NonNullableReferenceType, Is.Null);
			Assert.That(mock.NullableReferenceType, Is.Null);
		}
	}

	[Test]
	public static void InitPropertiesWithNullWithMake()
	{
		var mock = new InitsMakeExpectations().Instance(null);
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.Zero);
			Assert.That(mock.NullableValueType, Is.Null);
			Assert.That(mock.NonNullableReferenceType, Is.Null);
			Assert.That(mock.NullableReferenceType, Is.Null);
		}
	}

	[Test]
	public static void RequiredPropertiesWithCreate()
	{
		using var context = new RockContext();
		var expectations = context.Create<RequiredsCreateExpectations>();
		expectations.Setups.Foo();

		var mock = expectations.Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		}
	}

	[Test]
	public static void RequiredPropertiesWithMake()
	{
		var mock = new RequiredsMakeExpectations().Instance(
			new() { NonNullableValueType = 3, NullableValueType = 2, NonNullableReferenceType = "3", NullableReferenceType = "2" });
		mock.Foo();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(mock.NonNullableValueType, Is.EqualTo(3));
			Assert.That(mock.NullableValueType, Is.EqualTo(2));
			Assert.That(mock.NonNullableReferenceType, Is.EqualTo("3"));
			Assert.That(mock.NullableReferenceType, Is.EqualTo("2"));
		}
	}

	[Test]
	public static void RequiredPropertiesWithNullWithCreate()
	{
		var expectations = new RequiredsCreateExpectations();
		expectations.Setups.Foo();

		Assert.That(() => expectations.Instance(null!), Throws.TypeOf<ArgumentNullException>());
	}

	[Test]
	public static void RequiredPropertiesWithNullWithMake() =>
		Assert.That(() => new RequiredsMakeExpectations().Instance(null!), Throws.TypeOf<ArgumentNullException>());
}