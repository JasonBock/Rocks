using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.UnionTestTypes;

public sealed record TypeA(string Description);

#pragma warning disable IDE0250 // Make struct 'readonly'
#pragma warning disable CA1815 // Override equals and operator equals on value types
public union Classifiers(TypeA);

public interface IClassifierUsage
{
	void UseClassifier(Classifiers classifiers);
}

internal static class UnionTests
{
	[Test]
	public static void Create()
	{
		using var repository = new RockContext();
		var expectations = repository.Create<IClassifierUsageCreateExpectations>();
		expectations.Setups.UseClassifier(new Classifiers(new TypeA("Value")));

		var mock = expectations.Instance();
		mock.UseClassifier(new TypeA("Value"));
	}

	[Test]
	public static void Make()
	{
		var make = new IClassifierUsageMakeExpectations().Instance();
		make.UseClassifier(new TypeA("Value"));
	}
}