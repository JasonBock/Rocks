using Moq;
using NUnit.Framework;

namespace Rocks.Comparisons.Tests;

internal static class UsesSpanTests
{
	[Test]
	public static void UseRocks()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveSpanCreateExpectations>();
		expectations.Methods.Process(new()).ReturnValue(3);

		var uses = new UsesSpan(expectations.Instance());
		var buffer = new int[] { 3 };
		Assert.That(uses.Execute(new Span<int>(buffer)), Is.EqualTo(3));
	}

	// This won't work. Moq relies on expression trees and they haven't
	// been updated to work with ref structs.
	// You can't call Setup(), you'll get a CS9244 compiler error.
	// Whether the repository is set to Strict or Loose,
	// you'll get a very bad exception at runtime:
	// System.InvalidProgramException : Common Language Runtime detected an invalid program.
	[Test]
	[Ignore("Only used to illustrate mocking framework failure")]
	public static void UseMoq()
	{
		var repository = new MockRepository(MockBehavior.Loose);
		var expectations = repository.Create<IHaveSpan>();
		//expectations.Setup(_ => _.Process(It.IsAny<Span<int>>()));

		var uses = new UsesSpan(expectations.Object);
		var buffer = new int[] { 3 };
		Assert.That(uses.Execute(new Span<int>(buffer)), Is.EqualTo(3));
	}

	// Same behavior with NSubstitute as with Moq.
	// Setting up a call on Process() won't work with Arg.Any(),
	// and removing that with passing in a Span<int>
	// results in a CLR program error.
	[Test]
    [Ignore("Only used to illustrate mocking framework failure")]
    public static void UseNSubstitute()
	{
		//var expectations = Substitute.For<IHaveSpan>();
		//expectations.Process(NSubstitute.Arg.Any<Span<int>>());
	}
}