using NUnit.Framework;

namespace Rocks.Completions.Tests;

public static class AddRockAttributeRefactoringTests
{
	[Test]
	public static async Task RunWhenBuildPropertyIsAddedAsync()
	{
		var source =
			"""
			public [|c|]lass Mockable
			{
				public virtual void Do() { }
			}
			""";

		var sourceMockDefinitions = "";

		var fixedSource =
			"""
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedMockDefinitionsSource =
			"""
			[assembly: Rocks.Runtime.Rock(typeof(Mockable), Rocks.Runtime.BuildType.Create)]

			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source), ("MockDefinitions.cs", sourceMockDefinitions)], 
			[("Source.cs", fixedSource), ("MockDefinitions.cs", fixedMockDefinitionsSource)], 0, true);
	}

	[Test]
	public static async Task RunWithTargetInMultiplePartNamespaceAsync()
	{
		var source =
			"""
			namespace Inner.Middle.Outer;

			public [|c|]lass Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Runtime.Rock(typeof(Inner.Middle.Outer.Mockable), Rocks.Runtime.BuildType.Create)]

			namespace Inner.Middle.Outer;
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false);
	}

	[TestCase("Mockable<T1>", "Mockable<>")]
	[TestCase("Mockable<T1, T2>", "Mockable<,>")]
	[TestCase("Mockable<T1, T2, T3>", "Mockable<,,>")]
	public static async Task RunWithGenericTypeAsync(string targetType, string expectedName)
	{
		var source =
			$$"""
			public abstract [|c|]lass {{targetType}}
			{
				public abstract T1 Do();
			}
			""";

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof({{expectedName}}), Rocks.Runtime.BuildType.Create)]

			public abstract class {{targetType}}
			{
				public abstract T1 Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunClassDeclarationFixAsync(string buildType, int codeActionIndex)
	{
		var source =
			"""
			public [|c|]lass Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(Mockable), {{buildType}})]

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunIdentifierNameFixAsync(string buildType, int codeActionIndex)
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

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(Mockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunInterfaceDeclarationFixAsync(string buildType, int codeActionIndex)
	{
		var source =
			"""
			public [|i|]nterface IAmMockable
			{
				void Do();
			}
			""";

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(IAmMockable), {{buildType}})]

			public interface IAmMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunRecordDeclarationFixAsync(string buildType, int codeActionIndex)
	{
		var source =
			"""
			public [|r|]ecord Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(Mockable), {{buildType}})]

			public record Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunParameterFixAsync(string buildType, int codeActionIndex)
	{
		var source =
			"""
			public static class MockableUser
			{
				public static void Use(IAmMockable [|m|]ockable) { }
			}

			public interface IAmMockable
			{
				void Do();
			}
			""";

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(IAmMockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
	}

	[TestCase("Rocks.Runtime.BuildType.Create", 0)]
	[TestCase("Rocks.Runtime.BuildType.Make", 1)]
	[TestCase("Rocks.Runtime.BuildType.Create | Rocks.Runtime.BuildType.Make", 2)]
	public static async Task RunObjectCreationFixAsync(string buildType, int codeActionIndex)
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

		var fixedSource =
			$$"""
			[assembly: Rocks.Runtime.Rock(typeof(Mockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false);
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
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false);
	}
}