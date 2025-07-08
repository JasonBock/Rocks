using NUnit.Framework;

namespace Rocks.Tests;

public static class RockContextTests
{
	[Test]
	public static void Use()
	{
		MyExpectations? expectations = null;

		using (var context = new RockContext())
		{
			expectations = context.Create<MyExpectations>();
		}

		Assert.That(expectations!.HasBeenVerified, Is.True);
	}

	[Test]
	public static void UseWithVerificationsDisabled()
	{
		MyExpectations? expectations = null;

		using (var context = new RockContext(DisableVerification.Yes))
		{
			expectations = context.Create<MyExpectations>();
		}

		Assert.That(expectations!.HasBeenVerified, Is.False);
	}

	[Test]
   public static void UseWithNoExpectationsCreated() => 
		Assert.DoesNotThrow(() => { using var context = new RockContext(); });

	[Test]
	public static void UseCreateAfterDisposing()
	{
		RockContext? context = null;
		using (context = new RockContext()) { }

		Assert.Throws<ObjectDisposedException>(() => context.Create<MyExpectations>());
	}
	[Test]
	public static void UseDisposeAfterDisposing()
	{
		RockContext? context = null;
		using (context = new RockContext()) { }

		Assert.Throws<ObjectDisposedException>(() => context.Dispose());
	}

}

internal sealed class MyExpectations
	: Expectations
{
	public override void Verify() =>
		this.HasBeenVerified = true;

	internal bool HasBeenVerified { get; private set; }
}