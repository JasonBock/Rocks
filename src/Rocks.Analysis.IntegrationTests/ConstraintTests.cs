using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ConstraintTestTypes;

public class BaseForConstraintCase { }

public abstract class BaseForConstraintCase<T>
	: BaseForConstraintCase where T : class
{
#pragma warning disable CA1716 // Identifiers should not match keywords
	public abstract BaseForConstraintCase<TTarget> As<TTarget>() where TTarget : class;
#pragma warning restore CA1716 // Identifiers should not match keywords
}

internal static class ConstraintTests
{
	[Test]
	public static void Create()
	{
		using var context = new RockContext();
		var expectations = context.Create<BaseForConstraintCaseCreateExpectations<string>>();
		expectations.Setups.As<string>();

		var mock = expectations.Instance();
		mock.As<string>();
	}
}