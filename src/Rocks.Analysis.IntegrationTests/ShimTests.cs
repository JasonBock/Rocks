using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ShimTestTypes;

public interface IAuditTrail
{
	void Work() { }
}

public interface IAuditTrail<T>
	: IAuditTrail
{
	void Perform() { }
}

internal static class ShimTests
{
	[Test]
	public static void Create()
	{
		using var repository = new RockContext();
		var expectations = repository.Create<IAuditTrailCreateExpectations<string>>();

		var mock = expectations.Instance();
		mock.Perform();
		mock.Work();
	}
}