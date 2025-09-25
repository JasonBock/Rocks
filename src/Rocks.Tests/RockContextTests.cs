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

	[Test]
	public static void UseWhenExpectationThrewException()
	{
		MyExpectations? expectations = null;

		using (var context = new RockContext()) 
		{
			expectations = context.Create<MyExpectations>();
			expectations.SetWasExceptionThrown(true);
		}

		Assert.That(expectations.HasBeenVerified, Is.False);
	}
}

internal sealed class MyExpectations
	: Expectations
{
	public void SetWasExceptionThrown(bool value) => this.WasExceptionThrown = value;

	public override void Verify() =>
		this.HasBeenVerified = true;

	internal bool HasBeenVerified { get; private set; }
}