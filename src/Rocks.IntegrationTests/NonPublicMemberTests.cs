using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests.NonPublicMemberTestTypes;

public abstract class HasProtectedMember
{
	protected HasProtectedMember() { }

	public void CallsDoSomething() => this.DoSomething(3);

	protected abstract void DoSomething(int data);
}

public static class NonPublicMemberTests
{
	[Test]
	public static void ThrowExpectedException()
	{
		var protectedMemberExpectations = new HasProtectedMemberCreateExpectations();

		var protectedMember = protectedMemberExpectations.Instance();

		Assert.That(() => protectedMember.CallsDoSomething(), 
			Throws.TypeOf<ExpectationException>()
				.With.Message.EqualTo("No handlers were found for Void DoSomething(Int32)"));
	}
}
