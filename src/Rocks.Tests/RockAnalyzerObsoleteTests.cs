using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerObsoleteTests
{
	[Test]
	public static async Task AnalyzeWhenAPropertyAndItsTypeAreObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
			
			[assembly: Rock(typeof(MockTests.IAmAlsoObsolete), BuildType.Create | BuildType.Make)]
			
			#nullable enable
			
			namespace MockTests
			{
				[Obsolete("Do not use this type", true)]
				public interface IAmObsolete
				{
					void Execute();
				}
			
				public interface IAmAlsoObsolete
				{
					[Obsolete("Do not use this method", true)]
					IAmObsolete Use { get; }
				}
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(7, 12, 7, 86).WithArguments("Use");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenAMethodAndItsPartsAreObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
									
			[assembly: Rock(typeof(MockTests.IAmAlsoObsolete), BuildType.Create | BuildType.Make)]
			
			#nullable enable
			
			namespace MockTests
			{
				[Obsolete("Do not use this type", true)]
				public interface IAmObsolete
				{
					void Execute();
				}
			
				public interface IAmAlsoObsolete
				{
					[Obsolete("Do not use this method", true)]
					void Use(IAmObsolete old);
				}
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(7, 12, 7, 86).WithArguments("Use");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenGenericContainsObsoleteTypeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections;
			using System.Collections.Generic;
			using System.Linq;
			
			[assembly: Rock(typeof(MockTests.JobStorage), BuildType.Create | BuildType.Make)]
			
			#nullable enable
			
			namespace MockTests
			{
				[Obsolete("Do not use this", true)]
				public interface IServerComponent
				{
					void Execute();
				}
			
				public abstract class JobStorage
				{
					public virtual IEnumerable<IServerComponent> GetComponents() =>
						Enumerable.Empty<IServerComponent>();
				}
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(7, 12, 7, 81).WithArguments("GetComponents");
		var compiler1Diagnostic = DiagnosticResult.CompilerError("CS0619")
			.WithSpan(21, 30, 21, 46).WithArguments("MockTests.IServerComponent", "Do not use this");
		var compiler2Diagnostic = DiagnosticResult.CompilerError("CS0619")
			.WithSpan(22, 21, 22, 37).WithArguments("MockTests.IServerComponent", "Do not use this");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code,
			[diagnostic, diagnostic, compiler1Diagnostic, compiler2Diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMemberUsesObsoleteTypeInConstructorAsync()
	{
		var code =
			"""
			using System; 
			using Rocks; 
			
			[assembly: Rock(typeof(UsesObsolete), BuildType.Create | BuildType.Make)] 
			
			[Obsolete("Old", error: true)]
			public class DoNotUse { } 
			
			public class UsesObsolete 
			{ 
				public UsesObsolete(DoNotUse use) { } 
				public virtual void Foo() { } 
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments(".ctor");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(11, 22, 11, 30);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMemberUsesObsoleteTypeInMethodParameterAsync()
	{
		var code =
			"""
			using System; 
			using Rocks; 
			
			[assembly: Rock(typeof(UsesObsolete), BuildType.Create | BuildType.Make)] 
			
			[Obsolete("Old", error: true)]
			public class DoNotUse { } 
			
			public class UsesObsolete 
			{ 
				public virtual void ObsoleteMethod(DoNotUse use) { } 
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments("ObsoleteMethod");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(11, 37, 11, 45);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMemberUsesObsoleteTypeInMethodReturnValueAsync()
	{
		var code =
			"""
			using System; 
			using Rocks; 
			
			[assembly: Rock(typeof(UsesObsolete), BuildType.Create | BuildType.Make)]
			
			[Obsolete("Old", error: true)]
			public class DoNotUse { } 
			
			public class UsesObsolete 
			{
				public virtual DoNotUse ObsoleteMethod() => default!; 
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments("ObsoleteMethod");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(11, 17, 11, 25);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMemberUsesObsoleteTypeInPropertyAsync()
	{
		var code =
			"""
			using System; 
			using Rocks; 
			
			[assembly: Rock(typeof(UsesObsolete), BuildType.Create | BuildType.Make)]
			
			[Obsolete("Old", error: true)]
			public class DoNotUse { } 
			
			public class UsesObsolete 
			{ 
				public virtual DoNotUse ObsoleteProperty { get; } 
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments("ObsoleteProperty");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(11, 17, 11, 25);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMemberUsesObsoleteTypeInIndexerAsync()
	{
		var code =
			"""
			using System;
			using Rocks;
			
			[assembly: Rock(typeof(UsesObsolete), BuildType.Create | BuildType.Make)]
			
			[Obsolete("Old", error: true)]
			public class DoNotUse { }
			
			public class UsesObsolete
			{
				public virtual int this[DoNotUse value] { get => 42; }
			}
			""";

		var diagnostic = new DiagnosticResult(MemberUsesObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments("this[]");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(11, 26, 11, 34);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenTypeIsObsoleteAndErrorIsTrueAsync()
	{
		var code =
			"""
			using System;
			using Rocks;

			[assembly: Rock(typeof(ObsoleteType), BuildType.Create | BuildType.Make)]

			[Obsolete("a", true)]
			public class ObsoleteType { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockObsoleteTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 73).WithArguments("ObsoleteType");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0619").WithSpan(4, 24, 4, 36);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenTypeIsObsoleteAndErrorIsSetToFalseAsync()
	{
		var code =
			"""
			using System;
			using Rocks;

			[assembly: Rock(typeof(ObsoleteType), BuildType.Create | BuildType.Make)]

			[Obsolete("a", false)]
			public class ObsoleteType { }
			""";

		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}
}