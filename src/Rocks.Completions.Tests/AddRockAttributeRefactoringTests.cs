using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Completions.Tests;

public static class AddRockAttributeRefactoringTests
{
	[Test]
	public static async Task RunWhenTargetIsVariableAsync()
	{
		var source =
			"""
			using System;

			public class TargetUsage
			{ 
				public string Work()
				{
					var x = Guid.NewGuid();
					return [|x|].ToString() + this.ToString();
				}
			}
			""";

		var fixedSource =
			"""
			using System;
			
			[assembly: Rocks.Rock(typeof(System.Guid), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			
			public class TargetUsage
			{ 
				public string Work()
				{
					var x = Guid.NewGuid();
					return x.ToString() + this.ToString();
				}
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsThisAsync()
	{
		var source =
			"""
			using System;

			public class TargetUsage
			{ 
				public string Work()
				{
					var x = Guid.NewGuid();
					return x.ToString() + t[|h|]is.ToString();
				}
			}
			""";

		var fixedSource =
			"""
			using System;
			
			[assembly: Rocks.Rock(typeof(TargetUsage), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			
			public class TargetUsage
			{ 
				public string Work()
				{
					var x = Guid.NewGuid();
					return x.ToString() + this.ToString();
				}
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsInvocationAsync()
	{
		var source =
			"""
			using System;

			public static class TargetUsage
			{ 
				public static Guid Work() => Guid.NewGu[|i|]d();
			}
			""";

		var fixedSource =
			"""
			using System;
			
			[assembly: Rocks.Rock(typeof(System.Guid), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			
			public static class TargetUsage
			{ 
				public static Guid Work() => Guid.NewGuid();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsAttributeAsync()
	{
		var source =
			"""
			using System;

			[AttributeUsage(AttributeTargets.All)]
			public sealed class DataAttribute
				: Attribute
			{ }
			
			[D[|a|]ta]
			public static class TargetUsage
			{ }
			""";

		var fixedSource =
			"""
			using System;
			
			[assembly: Rocks.Rock(typeof(DataAttribute), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			[AttributeUsage(AttributeTargets.All)]
			public sealed class DataAttribute
				: Attribute
			{ }
			
			[Data]
			public static class TargetUsage
			{ }
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsPredefinedTypeAsync()
	{
		var source =
			"""
			public static class TargetUsage
			{
				public static void Run()
				{
					[|o|]bject o = new();
				}
			}
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Rock(typeof(System.Object), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			public static class TargetUsage
			{
				public static void Run()
				{
					object o = new();
				}
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsExplicitTypeAsync()
	{
		var source =
			"""
			public class Target
			{
				public virtual void Do() { }
			}

			public static class ITargetUsage
			{
				public static void Run()
				{
					[|T|]arget target = new();
				}
			}
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Rock(typeof(Target), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			public class Target
			{
				public virtual void Do() { }
			}
			
			public static class ITargetUsage
			{
				public static void Run()
				{
					Target target = new();
				}
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWhenTargetIsBaseTypeAsync()
	{
		var source =
			"""
			public abstract class BaseClass
			{
				protected BaseClass() { }
			}

			public class DerivedClass
				: [|B|]aseClass { }
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Rock(typeof(BaseClass), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			public abstract class BaseClass
			{
				protected BaseClass() { }
			}
			
			public class DerivedClass
				: BaseClass { }
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

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
			[assembly: Rocks.Rock(typeof(Mockable), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source), ("MockDefinitions.cs", sourceMockDefinitions)],
			[("Source.cs", fixedSource), ("MockDefinitions.cs", fixedMockDefinitionsSource)], 0, true, [], []);
	}

	[Test]
	public static async Task RunWithObjectCreationErrorNamespaceAsync()
	{
		var source =
			"""
			public static class MockableUser
			{
				public static void Use()
				{
					var expectations = new [|I|]Mockable
				}
			}

			public interface IMockable
			{
				void Do();
			}
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Rock(typeof(IMockable), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			
			public static class MockableUser
			{
				public static void Use()
				{
					var expectations = new [|I|]Mockable
				}
			}
			
			public interface IMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false,
			[
				new DiagnosticResult("CS0144", DiagnosticSeverity.Error).WithSpan("Source.cs", 5, 22, 6, 1),
				new DiagnosticResult("CS1002", DiagnosticSeverity.Error).WithSpan("Source.cs", 5, 35, 5, 35),
				new DiagnosticResult("CS1526", DiagnosticSeverity.Error).WithSpan("Source.cs", 5, 35, 5, 35)
			],
			[
				new DiagnosticResult("CS0144", DiagnosticSeverity.Error).WithSpan("Source.cs", 7, 22, 8, 1),
				new DiagnosticResult("CS1002", DiagnosticSeverity.Error).WithSpan("Source.cs", 7, 35, 7, 35),
				new DiagnosticResult("CS1526", DiagnosticSeverity.Error).WithSpan("Source.cs", 7, 35, 7, 35)
			]);
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
			[assembly: Rocks.Rock(typeof(Inner.Middle.Outer.Mockable), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			namespace Inner.Middle.Outer;
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[Test]
	public static async Task RunWithExistingTargetAsync()
	{
		var source =
			"""
			[assembly: Rocks.Rock(typeof(Inner.Middle.Outer.Mockable), Rocks.BuildType.Create | Rocks.BuildType.Make)]
			
			namespace Inner.Middle.Outer;

			public [|c|]lass Mockable
			{
				public virtual void Do() { }
			}
			""";

		var fixedSource =
			"""
			[assembly: Rocks.Rock(typeof(Inner.Middle.Outer.Mockable), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			namespace Inner.Middle.Outer;
			
			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
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
			[assembly: Rocks.Rock(typeof({{expectedName}}), Rocks.BuildType.Create | Rocks.BuildType.Make)]

			public abstract class {{targetType}}
			{
				public abstract T1 Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(Mockable), {{buildType}})]

			public class Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(Mockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(IAmMockable), {{buildType}})]

			public interface IAmMockable
			{
				void Do();
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(Mockable), {{buildType}})]

			public record Mockable
			{
				public virtual void Do() { }
			}
			""";

		await TestAssistants.RunRefactoringAsync<AddRockAttributeRefactoring>(
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(IAmMockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
	}

	[TestCase("Rocks.BuildType.Create | Rocks.BuildType.Make", 0)]
	[TestCase("Rocks.BuildType.Create", 1)]
	[TestCase("Rocks.BuildType.Make", 2)]
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
			[assembly: Rocks.Rock(typeof(Mockable), {{buildType}})]

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
			[("Source.cs", source)], [("Source.cs", fixedSource)], codeActionIndex, false, [], []);
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
			[("Source.cs", source)], [("Source.cs", fixedSource)], 0, false, [], []);
	}
}