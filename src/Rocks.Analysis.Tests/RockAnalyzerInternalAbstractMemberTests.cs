using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using Microsoft.CodeAnalysis.CSharp;

namespace Rocks.Analysis.Tests;

public static class RockAnalyzerInternalAbstractMemberTests
{
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", false)]
	public static async Task AnalyzeCreateAsync(string internalCode, bool hasDiagnostic)
	{
		var internalCompilation = GetInternalCompilation(internalCode);

		var code =
			$$"""
			using Rocks;

			[assembly: Rock(typeof(InternalTargets), BuildType.Create)]
			""";

		if (hasDiagnostic)
		{
			var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
				.WithSpan(3, 12, 3, 59).WithArguments("InternalTargets");
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
		else
		{
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
	}

	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", true)]
	public static async Task AnalyzeCreateMultipleDiagnosticAsync(string internalCode, bool hasDiagnostic)
	{
		var internalCompilation = GetInternalCompilation(internalCode);

		var code =
			$$"""
			using Rocks;

			[assembly: Rock(typeof(InternalTargets), BuildType.Create)]
			""";

		if (hasDiagnostic)
		{
			var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
				.WithSpan(3, 12, 3, 59).WithArguments("InternalTargets");
			var noDiagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
				.WithSpan(3, 12, 3, 59).WithArguments("InternalTargets");
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, noDiagnostic],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
		else
		{
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
	}

	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal abstract void Work(); }", true)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work(); }", true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal abstract string Work { get; } }", true)]
	[TestCase("public interface InternalTargets { string VisibleWork { get; } internal string Work { get; } }", true)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal abstract event EventHandler Work; }", true)]
	[TestCase("using System; public interface InternalTargets { event EventHandler VisibleWork; internal event EventHandler Work; }", true)]
	[TestCase("public abstract class InternalTargets { public abstract void VisibleWork(); internal virtual void Work() { } }", false)]
	[TestCase("public interface InternalTargets { void VisibleWork(); internal void Work() { } }", true)]
	[TestCase("public abstract class InternalTargets { public abstract string VisibleWork { get; } internal virtual string Work { get; } }", false)]
	[TestCase("using System; public abstract class InternalTargets { public abstract event EventHandler VisibleWork; internal virtual event EventHandler Work; }", true)]
	public static async Task AnalyzeMakeAsync(string internalCode, bool hasDiagnostic)
	{
		var internalCompilation = GetInternalCompilation(internalCode);

		var code =
			$$"""
			using Rocks;

			[assembly: Rock(typeof(InternalTargets), BuildType.Make)]
			""";

		if (hasDiagnostic)
		{
			var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
				.WithSpan(3, 12, 3, 57).WithArguments("InternalTargets");
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
		else
		{
			await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [],
				additionalReferences: [internalCompilation.ToMetadataReference()]);
		}
	}

	[Test]
	public static async Task AnalyzeWhenTargetIsInterfaceAndHasInternalMethodHasAsync()
	{
		var internalCode =
			"""
			public interface InternalTargets
			{ 
				void VisibleWork(); 
				internal void Work(); 
			}
			""";
		var internalCompilation = GetInternalCompilation(internalCode);

		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(InternalTargets), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 76).WithArguments("InternalTargets");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic],
			additionalReferences: [internalCompilation.ToMetadataReference()]);
	}

	private static CSharpCompilation GetInternalCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = Shared.References.Value
			.Concat([MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location)]);
		return CSharpCompilation.Create("generator", [syntaxTree],
			references, new CSharpCompilationOptions(
				OutputKind.DynamicallyLinkedLibrary, generalDiagnosticOption: ReportDiagnostic.Error));
	}
}