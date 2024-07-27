using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Rocks.Tests;

public static class RockAnalyzerInaccessibleAbstractMembersTests
{
	[Test]
	public static async Task AnalyzeWithInaccessibleTypesUsedOnConstraintsAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(ArgumentMapper), BuildType.Create | BuildType.Make)]
			
			public abstract class ArgumentMapper
			{
				protected interface ISource { }
			
				protected abstract void NotUsingSource<T>()
					where T : struct, ISource;
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 75).WithArguments("ArgumentMapper");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWithInternalAbstractMemberInDifferentAssemblyAsync()
	{
		var internalCode =
			"""
			public abstract class InternalAbstractMember
			{
				internal abstract void CannotSee(string a);
				public virtual void See() { }
			}
			""";
		var internalCompilation = RockAnalyzerInaccessibleAbstractMembersTests.GetInternalCompilation(internalCode);

		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(InternalAbstractMember), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 83).WithArguments("InternalAbstractMember");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic],
			additionalReferences: [internalCompilation.ToMetadataReference()]);
	}

	[Test]
	public static async Task AnalyzeWithInternalAbstractMemberInDifferentAssemblyWithUnreferenceableNameAsync()
	{
		// The IL generates this code:
		/*
		public abstract class InternalAbstractInvalidMember
		{
			public abstract void Valid();
			internal abstract void 4gbnwbnbspeclmzqvzf8egt7ef2ytrtdMCa();
		}
		*/

		using var assembly = AssemblyDefinition.CreateAssembly(
			new AssemblyNameDefinition("Invalid", Version.Parse("1.0.0.0")),
			"Invalid", ModuleKind.Dll);

		var type = new TypeDefinition("", "InternalAbstractInvalidMember",
			TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.Public | TypeAttributes.Abstract,
			assembly.MainModule.TypeSystem.Object);
		assembly.MainModule.Types.Add(type);

		var constructor = new MethodDefinition(".ctor",
			MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName,
			assembly.MainModule.TypeSystem.Void);
		type.Methods.Add(constructor);
		var constructorILProcessor = constructor.Body.GetILProcessor();
		constructorILProcessor.Emit(OpCodes.Ldarg_0);
		constructorILProcessor.Emit(OpCodes.Call,
			assembly.MainModule.ImportReference(new MethodReference(".ctor", type.Module.TypeSystem.Void, type) { HasThis = true }));
		constructorILProcessor.Emit(OpCodes.Ret);

		var validMethod = new MethodDefinition("Valid",
			MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.NewSlot,
			assembly.MainModule.TypeSystem.Void);
		type.Methods.Add(validMethod);

		var invalidMethod = new MethodDefinition("4gbnwbnbspeclmzqvzf8egt7ef2ytrtdMCa",
			MethodAttributes.Assembly | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.NewSlot,
			assembly.MainModule.TypeSystem.Void);
		type.Methods.Add(invalidMethod);

		using var stream = new MemoryStream();
		assembly.Write(stream);
		stream.Position = 0;

		var reference = MetadataReference.CreateFromStream(stream);

		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(InternalAbstractInvalidMember), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(TypeHasInaccessibleAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 90).WithArguments("InternalAbstractInvalidMember");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic],
			additionalReferences: [reference]);
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