using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.SuppressCA2000TestTypes;

public interface IAmDisposable
	: IDisposable
{
	void PerformWork();
}

internal static class SuppressCA2000Tests
{
	[Test]
	public static void Create()
	{
		using var context = new RockContext();
		var expectations = context.Create<IAmDisposableCreateExpectations>();
		expectations.Setups.Dispose();

		var mock = expectations.Instance();
		mock.Dispose();
	}
}