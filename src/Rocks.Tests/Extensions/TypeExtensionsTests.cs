using NUnit.Framework;
using Rocks.Extensions;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace Rocks.Tests.Extensions;

#pragma warning disable IDE0060 // Remove unused parameter
public class Identifiers
{
	private readonly int data = RandomNumberGenerator.GetInt32(int.MaxValue);

	[MemberIdentifier(1)]
	public static void Foo() { }

	[MemberIdentifier(2)]
   public static Guid Foo(string name, ImmutableArray<int> values) => Guid.NewGuid();

   [MemberIdentifier(3)]
	public static Guid Foo<TName, TValue>(TName name, ImmutableArray<TValue> values) => Guid.NewGuid();

	[MemberIdentifier(4, PropertyAccessor.Get)]
	[MemberIdentifier(5, PropertyAccessor.Set)]
	public static string? Data { get; set; }

	[MemberIdentifier(6)]
	public static Guid Foo(ref int name) => Guid.NewGuid();

	[MemberIdentifier(7)]
	protected int ProtectedFoo() => this.data;
}

public static class NoIdentifiers
{
	public static void Foo() { }
}

public static class TypeExtensionsTests
{
	[TestCase(1u, "Void Foo()")]
	[TestCase(2u, "System.Guid Foo(System.String, System.Collections.Immutable.ImmutableArray`1[System.Int32])")]
	[TestCase(3u, "System.Guid Foo[TName,TValue](TName, System.Collections.Immutable.ImmutableArray`1[TValue])")]
	[TestCase(4u, "System.String get_Data()")]
	[TestCase(5u, "Void set_Data(System.String)")]
	[TestCase(6u, "System.Guid Foo(Int32 ByRef)")]
	[TestCase(7u, "Int32 ProtectedFoo()")]
	public static void GetMemberDescription(uint value, string expectedDescription) =>
		Assert.That(typeof(Identifiers).GetMemberDescription(value), Is.EqualTo(expectedDescription));

	[Test]
	public static void GetMemberDescriptionWhenIdentifierIsNotFound() =>
		Assert.That(typeof(Identifiers).GetMemberDescription(0), Is.Null);

	[Test]
	public static void GetMemberDescriptionWhenAttributesDoNotExist() =>
		Assert.That(typeof(NoIdentifiers).GetMemberDescription(1), Is.Null);
}