using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Rocks.Comparisons.Tests;

internal static unsafe class IHavePointersTests
{
	[Test]
	public static void UseRocks()
	{
		var value = 10;
		var pValue = &value;

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerParameter(new(pValue));

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);
	}

	// This will compile, but will fail at runtime:
	// System.ArgumentException : Type must not be a pointer type (Parameter 'type')
	[Test, Ignore("Will fail at runtime")]
	public static void UseMoq()
	{
		var value = 10;
		var pValue = &value;

		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<IHavePointers>();
		expectations.Setup(_ => _.PointerParameter(pValue));

		var mock = expectations.Object;
		mock.PointerParameter(pValue);

		repository.VerifyAll();
	}

	// NSubstitute has an issue with validation and pointers.
	// Uncommenting the line of code with the Received() call
	// causes very bad things to happen with the test run.
	// I had to run "dotnet test" and view the log file to see what happened:
	// Fatal error. Internal CLR error. (0x80131506)
	// Note: to see the stack trace from the CLR error,
	// you need to run the test using "dotnet test"
	// and read the log file created during the test run.
	[Test]
	public static void UseNSubstitute()
	{
		var value = 10;
		var pValue = &value;

		var mock = Substitute.For<IHavePointers>();
		mock.PointerParameter(pValue);

		//mock.Received().PointerParameter(pValue);
	}
}