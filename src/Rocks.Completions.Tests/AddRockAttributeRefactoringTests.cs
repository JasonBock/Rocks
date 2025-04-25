using NUnit.Framework;

namespace Rocks.Completions.Tests;

public static class AddRockAttributeRefactoringTests
{
	[Test]
	public static async Task RunAsync()
	{
		var source =
			"""
			public interface IAm[|M|]ockable
			{
				void Do();
			}
			""";

		var fixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(IAmMockable), BuildType.Create)]
			public interface IAmMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(source, fixedSource);
	}
}