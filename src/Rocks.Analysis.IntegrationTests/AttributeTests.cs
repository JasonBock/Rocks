#define NEVER

using NUnit.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.IntegrationTests.AttributeTestTypes;

public abstract class UsingMemberNotNullWhen
{
	protected string? Connection { get; set; }

	[MemberNotNullWhen(true, nameof(UsingMemberNotNullWhen.Connection))]
	public virtual bool IsConnectionProvided => this.Connection is not null;
}

public interface ITense
{
	[UnscopedRef]
	ReadOnlySpan<nint> Lengths { get; }
}

public class NotNullIfNotCases
{
	[return: NotNullIfNotNull(nameof(node))]
	public virtual object? VisitMethod(object? node) => default;
}

public class ConventionDispatcher
{
	[Conditional("NEVER")]
	public virtual void AssertNoScope() { }
}

public static class AttributeTests
{
	[Test]
	public static void CreateWithMemberNotNullWhen()
	{
		using var context = new RockContext();
		var expectations = context.Create<UsingMemberNotNullWhenCreateExpectations>();
		expectations.Properties.Getters.IsConnectionProvided().ReturnValue(true);

		var mock = expectations.Instance();
		_ = mock.IsConnectionProvided;
	}

	[Test]
	public static void MakeWithMemberNotNullWhen()
	{
		var mock = new UsingMemberNotNullWhenMakeExpectations().Instance();
		_ = mock.IsConnectionProvided;
	}

	[Test]
	public static void CreateWithUnscopedRef()
	{
		using var context = new RockContext();
		var expectations = context.Create<ITenseCreateExpectations>();
		expectations.Properties.Getters.Lengths().ReturnValue(() => []);

		var mock = expectations.Instance();
		_ = mock.Lengths;
	}

	[Test]
	public static void MakeWithUnscopedRef()
	{
		var mock = new ITenseMakeExpectations().Instance();
		_ = mock.Lengths;
	}

	[Test]
	public static void CreateWithNotNullIfNotNull()
	{
		var node = new object();
		var result = new object();

		using var context = new RockContext();
		var expectations = context.Create<NotNullIfNotCasesCreateExpectations>();
		expectations.Methods.VisitMethod(node).ReturnValue(result);

		var mock = expectations.Instance();
		var mockResult = mock.VisitMethod(node);

		Assert.That(mockResult, Is.SameAs(result));
	}

	[Test]
	public static void CreateWithConditional()
	{
		using var context = new RockContext();
		var expectations = context.Create<ConventionDispatcherCreateExpectations>();
		expectations.Methods.AssertNoScope();

		var mock = expectations.Instance();
		mock.AssertNoScope();
	}
}