using NUnit.Framework;

namespace Rocks.Completions.Tests;

public static class AddRockAttributeRefactoringTests
{
	[Test]
	public static async Task RunWhenCursorIsOnIdentifierNameAsync()
	{
		var source =
			"""
			public static class MockableUser
			{
				public static void Use()
				{
					[|M|]ockable m = new();
				}
			}

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(Mockable), BuildType.Create)]

			public static class MockableUser
			{
				public static void Use()
				{
					Mockable m = new();
				}
			}
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorIsOnClassDeclarationAsync()
	{
		var source =
			"""
			public class [|M|]ockable
			{
				public virtual void Do() { }
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(Mockable), BuildType.Create)]

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorIsOnInterfaceDeclarationAsync()
	{
		var source =
			"""
			public interface IAm[|M|]ockable
			{
				void Do();
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(IAmMockable), BuildType.Create)]

			public interface IAmMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorIsOnRecordDeclarationAsync()
	{
		var source =
			"""
			public record [|M|]ockable
			{
				public virtual void Do() { }
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(Mockable), BuildType.Create)]

			public record Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorIsOnParameterAsync()
	{
		var source =
			"""
			public static class MockableUser
			{
				public static void Use(IAm[|M|]ockable mockable) { }
			}

			public interface IAmMockable
			{
				void Do();
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(IAmMockable), BuildType.Create)]

			public static class MockableUser
			{
				public static void Use(IAmMockable mockable) { }
			}
			
			public interface IAmMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorIsOnObjectCreationAsync()
	{
		var source =
			"""
			public static class MockableUser
			{
				public static void Use() 
				{ 
					var x = [|new Mockable()|];
				}
			}

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		var createFixedSource =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(Mockable), BuildType.Create)]

			public static class MockableUser
			{
				public static void Use() 
				{ 
					var x = new Mockable();
				}
			}
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [createFixedSource]);
	}

	[Test]
	public static async Task RunWhenCursorWillNotFindValidParentAsync()
	{
		var source =
			"""
			namespa[|c|]e TestNamespace;

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedSource =
			"""
			namespace TestNamespace;
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			source, [fixedSource]);
	}
}