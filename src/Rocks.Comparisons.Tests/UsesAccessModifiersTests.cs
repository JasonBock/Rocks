using Moq;
using Moq.Protected;
using NSubstitute;
using NUnit.Framework;
using System.Reflection;

namespace Rocks.Comparisons.Tests;

internal static class UsesAccessModifiersTests
{
	[Test]
	public static void UseRocksSetExpectationOnInternal()
	{
		using var context = new RockContext();
		var expectations = context.Create<AccessModifiersCreateExpectations>();
		expectations.Methods.InternalWork().ReturnValue(3);

		var uses = new UsesAccessModifiers(expectations.Instance());
		Assert.That(uses.Execute(), Is.EqualTo(3));
	}

	[Test]
	public static void UseRocksSetExpectationOnProtected()
	{
		using var context = new RockContext();
		var expectations = context.Create<AccessModifiersCreateExpectations>();
		expectations.Methods.ProtectedWork().ReturnValue(3);

		var uses = new UsesAccessModifiers(expectations.Instance());
		Assert.That(uses.Execute(), Is.EqualTo(3));
	}

	// Moq requires you to put an InternalsVisibleToAttribute
	// IN the assembly that has the internal member.
	// Note that the code compiles whether you do that or not,
	// but it'll fail at runtime.
	// If you don't own that assembly, you're kind of screwed.
	// You'd have to create a "wrapper" type of sorts in the test assembly
	// (which needs internal visibility)
	// that exposes a public virtual member that delegates to the internal call.
	[Test]
	public static void UseMoqSetExpectationOnInternal()
	{
		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<AccessModifiers>();
		expectations.Setup(_ => _.InternalWork()).Returns(3);

		var uses = new UsesAccessModifiers(expectations.Object);
		Assert.That(uses.Execute(), Is.EqualTo(3));

		repository.VerifyAll();
	}

	// Moq requires you to call Protected() to access protected members.
	// However, it forces you to name the member with a string, which is fragile.
	// It also requires you to specify the parameters and the return type.
	// Moreover, this test is problematic because it will require a Setup()
	// with InternalWork().CallBase(), which it really shouldn't as that method is not abstract.
	[Test]
	public static void UseMoqSetExpectationOnProtected()
	{
		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<AccessModifiers>();
		expectations.Setup(_ => _.InternalWork()).CallBase();
		expectations.Protected()
			.Setup<int>("ProtectedWork").Returns(3);

		var uses = new UsesAccessModifiers(expectations.Object);
		Assert.That(uses.Execute(), Is.EqualTo(3));

		repository.VerifyAll();
	}

	// NSubstitute has the same limitation with internals that Moq has.
	[Test]
	public static void UseNSubstituteSetExpectationOnInternal()
	{
		var mock = Substitute.For<AccessModifiers>();
		mock.InternalWork().Returns(3);

		var uses = new UsesAccessModifiers(mock);
		Assert.That(uses.Execute(), Is.EqualTo(3));
	}

	// NSubstitute is even worse with protected members.
	// You have to use Refection to find and invoke the member.
	// But in this case, it's even worse, because NSubstitute just
	// doesn't seem to call our mocked implementation of "ProtectedWork".
	// We'd like for it to call ProtectedWork() if InternalWork() 
	// isn't handled by NSubstitute, but I can't get anything to work.
	[Test, Ignore("This will fail no matter what")]
	public static void UseNSubstituteSetExpectationOnProtected()
	{
		var mock = Substitute.For<AccessModifiers>();
		mock.GetType()!
			.GetMethod("ProtectedWork", BindingFlags.NonPublic | BindingFlags.Instance)!
			.Invoke(mock, []).Returns(_ => 3);

		var uses = new UsesAccessModifiers(mock);
		Assert.That(uses.Execute(), Is.EqualTo(3));
	}
}