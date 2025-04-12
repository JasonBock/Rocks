using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.CodeGenerationTest.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Rocks.CodeGenerationTest;

internal static class TestGenerator
{
	private const LanguageVersion LanguageVersionOption = LanguageVersion.Preview;

	/* 
	These diagnostics relate to:

	* Obsolete members and types (CS0612, CS0618, CS0619, CS0672, CS0809, SYSLIB0017, SYSLIB0050, SYSLIB0051)
	* Nullability (CS8610, CS8765, CS8613)
	* Runtime policy (CS1701, CS1702)
	* UTF7 encoding (SYSLIB0001)
	* Code access security (SYSLIB0003)
	* EF "future" warnings (EF9100)
	* Aspire evaluation warnings (ASPIREPUBLISHERS001)

	These are warnings, and they should not cause errors. A user can decide to treat them
	as errors, but Rocks should still be able to create code that cause these warnings.
	*/
	internal static readonly Dictionary<string, ReportDiagnostic> SpecificDiagnostics = new()
	{
		{ "CS0612", ReportDiagnostic.Suppress },
		{ "CS0618", ReportDiagnostic.Suppress },
		{ "CS0619", ReportDiagnostic.Suppress },
		{ "CS0672", ReportDiagnostic.Suppress },
		{ "CS0809", ReportDiagnostic.Suppress },
		{ "CS1701", ReportDiagnostic.Info },
		{ "CS1702", ReportDiagnostic.Info },
		{ "CS8610", ReportDiagnostic.Suppress },
		{ "CS8613", ReportDiagnostic.Suppress },
		{ "CS8765", ReportDiagnostic.Suppress },
		{ "EF9100", ReportDiagnostic.Suppress },
		{ "FluentMultiSplitter", ReportDiagnostic.Suppress },
		{ "SYSLIB0001", ReportDiagnostic.Info },
		{ "SYSLIB0003", ReportDiagnostic.Info },
		{ "SYSLIB0017", ReportDiagnostic.Info },
		{ "SYSLIB0050", ReportDiagnostic.Info },
		{ "SYSLIB0051", ReportDiagnostic.Info },
		{ "ASPIREPUBLISHERS001", ReportDiagnostic.Suppress }
	};

	internal static ImmutableArray<Type> GetTargets(HashSet<Assembly> targetAssemblies, ImmutableArray<Type> typesToIgnore,
		 ImmutableArray<Type> typesToLoadAssembliesFrom, string[] aliases)
	{
		var initialTypes = targetAssemblies.SelectMany(_ => 
			_.GetTypes().Where(
				_ => _.IsPublic && 
				!_.IsSubclassOf(typeof(Delegate)) 
				&& !_.IsValueType 
				&& !_.IsSealed 
				&& !typesToIgnore.Contains(_)))
			.ToImmutableArray();
		return TestGenerator.GetMockableTypes(initialTypes, true, typesToLoadAssembliesFrom, aliases);
	}

	private static ImmutableArray<Type> GetMockableTypes(
		ImmutableArray<Type> types, bool isCreate, ImmutableArray<Type> typesToLoadAssembliesFrom, string[] aliases)
	{
		static string GetValidationCode(ImmutableArray<Type> types, string[] aliases)
		{
			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer, "\t");

			if (aliases.Length > 0)
			{
				foreach (var alias in aliases)
				{
					indentWriter.WriteLine($"extern alias {alias};");
				}

				indentWriter.WriteLine();
			}

			indentWriter.Write(
				 $$"""
				    using Rocks;
				    using System;

				    [assembly: CLSCompliant(false)]

				    internal static class Holder
				    {
				    """);

			for (var i = 0; i < types.Length; i++)
			{
				var type = types[i];
				indentWriter.WriteLine($"\tinternal static void Target{i}({type.GetTypeDefinition(aliases, false)} target) {{ }}");
			}

			writer.WriteLine("}");
			return writer.ToString();
		}

		var code = GetValidationCode(types, aliases);
		var assemblies = types.Select(_ => _.Assembly).ToHashSet();
		var syntaxTree = CSharpSyntaxTree.ParseText(code, options: new CSharpParseOptions(languageVersion: TestGenerator.LanguageVersionOption));
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location).WithAliases(aliases)))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			]);

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(
			[
				MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			]);
		}

		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			 references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
				  allowUnsafe: true,
				  generalDiagnosticOption: ReportDiagnostic.Error,
				  specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var validTypes = new List<Type>();

		foreach (var methodNode in syntaxTree.GetRoot().DescendantNodes(_ => true)
			 .OfType<MethodDeclarationSyntax>())
		{
			var methodSymbol = model.GetDeclaredSymbol(methodNode)!;
			var typeSymbol = methodSymbol.Parameters[0].Type.OriginalDefinition;

			if (MockModel.Create(methodNode, typeSymbol, null, new(model), isCreate ? BuildType.Create : BuildType.Make, true).Information is not null)
			{
				var methodIndex = methodSymbol.Name.Replace("Target", string.Empty, StringComparison.InvariantCulture);
				var typeIndex = int.Parse(methodIndex, CultureInfo.InvariantCulture);
				validTypes.Add(types[typeIndex]);
			}
		}

		return [.. validTypes];
	}

	internal static (ImmutableArray<Issue> issues, Times times) Generate(
		IIncrementalGenerator generator, ImmutableArray<Type> targetTypes, ImmutableArray<Type> typesToLoadAssembliesFrom,
		 string[] aliases, BuildType buildType)
	{
		var issues = new List<Issue>();
		var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();
		var code = TestGenerator.GetCode(targetTypes, buildType, aliases);

		var parseOptions = new CSharpParseOptions(languageVersion: TestGenerator.LanguageVersionOption);
		var syntaxTree = CSharpSyntaxTree.ParseText(code, options: parseOptions);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location).WithAliases(aliases)))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			]);

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(
			[
				 MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			]);
		}

		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			references, new CSharpCompilationOptions(
				OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true,
				generalDiagnosticOption: ReportDiagnostic.Error,
				specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));

		var driver = CSharpGeneratorDriver.Create(
			generator).WithUpdatedParseOptions(parseOptions);
		var generatorWatch = Stopwatch.StartNew();
		_ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
		generatorWatch.Stop();

		var driverHasDiagnostics = diagnostics.Any(_ => _.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning);

		var emitWatch = new Stopwatch();

		if (driverHasDiagnostics)
		{
			issues.AddRange(diagnostics
				 .Where(_ => _.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
				 .Select(_ => new Issue(_.Id, _.Severity, _.ToString(), _.Location)));
			var mockCode = compilation.SyntaxTrees.ToArray()[^1];
		}
		else
		{
			using var outputStream = new MemoryStream();
			emitWatch.Start();
			var result = outputCompilation.Emit(outputStream);
			emitWatch.Stop();

			var ignoredWarnings = Array.Empty<string>();

			issues.AddRange(result.Diagnostics
				 .Where(_ => _.Severity == DiagnosticSeverity.Error ||
					  (_.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id)))
				 .Select(_ => new Issue(_.Id, _.Severity, _.ToString(), _.Location)));
			var mockCode = outputCompilation.SyntaxTrees.ToArray()[^1];
		}

		return ([.. issues], new(generatorWatch.Elapsed, emitWatch.Elapsed));
	}

	internal static (ImmutableArray<Issue> issues, Times times) GenerateNoEmit(
		IIncrementalGenerator generator, ImmutableArray<Type> targetTypes, ImmutableArray<Type> typesToLoadAssembliesFrom,
		 string[] aliases, BuildType buildType)
	{
		var issues = new List<Issue>();
		var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();
		var code = GetCode(targetTypes, buildType, aliases);

		var syntaxTree = CSharpSyntaxTree.ParseText(code, options: new CSharpParseOptions(languageVersion: TestGenerator.LanguageVersionOption));
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location).WithAliases(aliases)))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			]);

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(
			[
				 MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			]);
		}

		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			 references, new(OutputKind.DynamicallyLinkedLibrary,
				  allowUnsafe: true,
				  generalDiagnosticOption: ReportDiagnostic.Error,
				  specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));

		var driver = CSharpGeneratorDriver.Create(generator);
		var generatorWatch = Stopwatch.StartNew();
		_ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
		generatorWatch.Stop();

		var driverHasDiagnostics = diagnostics.Any(_ => _.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning);

		if (driverHasDiagnostics)
		{
			issues.AddRange(diagnostics
				.Where(_ => _.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
				.Select(_ => new Issue(_.Id, _.Severity, _.ToString(), _.Location)));
			var mockCode = compilation.SyntaxTrees.ToArray()[^1];
		}

		return ([.. issues], new(generatorWatch.Elapsed, default));
	}

	internal static void Generate(IIncrementalGenerator generator, string code, Type[] typesToLoadAssembliesFrom)
	{
		var parseOptions = new CSharpParseOptions(languageVersion: TestGenerator.LanguageVersionOption);
		var syntaxTree = CSharpSyntaxTree.ParseText(code, options: parseOptions);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			]);

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(
			[
				MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			]);
		}

		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			references, new(OutputKind.DynamicallyLinkedLibrary,
			allowUnsafe: true,
			generalDiagnosticOption: ReportDiagnostic.Error,
			specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));

		var driver = CSharpGeneratorDriver.Create(generator)
			.WithUpdatedParseOptions(parseOptions);
		_ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

		Console.WriteLine(
			 $"Does generator compilation have any warning or error diagnostics? {diagnostics.Any(_ => _.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)}");

		using var outputStream = new MemoryStream();
		var result = outputCompilation.Emit(outputStream);

		Console.WriteLine($"Was emit successful? {result.Success}");

		var errors = result.Diagnostics
			 .Where(_ => _.Severity == DiagnosticSeverity.Error)
			 .Select(_ => new
			 {
				 _.Id,
				 Description = _.ToString(),
			 })
			 .OrderBy(_ => _.Id).ToArray();

		var ignoredWarnings = Array.Empty<string>();
		var warnings = result.Diagnostics
			 .Where(_ => _.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id))
			 .Select(_ => new
			 {
				 _.Id,
				 Description = _.ToString(),
			 })
			 .OrderBy(_ => _.Id).ToArray();

		var mockCode = outputCompilation.SyntaxTrees.ToArray()[^1];

		Console.WriteLine($"{errors.Length} error{(errors.Length != 1 ? "s" : string.Empty)}, {warnings.Length} warning{(warnings.Length != 1 ? "s" : string.Empty)}");
		Console.WriteLine();

		foreach (var error in errors)
		{
			Console.WriteLine(
				 $"Error - Id: {error.Id}, Description: {error.Description}");
		}

		foreach (var warning in warnings)
		{
			Console.WriteLine(
				 $"Warning - Id: {warning.Id}, Description: {warning.Description}");
		}
	}

	internal static string GetCode(ImmutableArray<Type> types, BuildType buildType, string[] aliases)
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		if (aliases.Length > 0)
		{
			foreach (var alias in aliases)
			{
				indentWriter.WriteLine($"extern alias {alias};");
			}

			indentWriter.WriteLine();
		}

		indentWriter.Write(
			"""
			using Rocks;
			using System;

			[assembly: CLSCompliant(false)]
			""");
		indentWriter.WriteLine();

		for (var i = 0; i < types.Length; i++)
		{
			var type = types[i];
			var buildTypes = new List<string>();

			if (buildType.HasFlag(BuildType.Create))
			{
				buildTypes.Add("BuildType.Create");
			}

			if (buildType.HasFlag(BuildType.Make))
			{
				buildTypes.Add("BuildType.Make");
			}

			indentWriter.WriteLine($"[assembly: Rock(typeof({type.GetTypeDefinition(aliases, false)}), {string.Join(" | ", buildTypes)})]");
		}

		indentWriter.WriteLine();

		for (var i = 0; i < types.Length; i++)
		{
			var type = types[i];
			var buildTypes = new List<string>();

			if (buildType.HasFlag(BuildType.Create))
			{
				buildTypes.Add("BuildType.Create");
			}

			if (buildType.HasFlag(BuildType.Make))
			{
				buildTypes.Add("BuildType.Make");
			}

			if (type.Namespace is not null)
			{
				indentWriter.WriteLine($"namespace {type.Namespace}");
				indentWriter.WriteLine("{");
				indentWriter.Indent++;
			}

			indentWriter.WriteLine($"[RockPartial(typeof({type.GetTypeDefinition(aliases, false)}), {string.Join(" | ", buildTypes)})]");
			indentWriter.WriteLine($"public sealed partial class {type.Name.Split("`")[0]}PartialTarget{type.GetTypeDefinitionParameters()};");

			if (type.Namespace is not null)
			{
				indentWriter.Indent--;
				indentWriter.WriteLine("}");
			}

			indentWriter.WriteLine();
		}

		return writer.ToString();
	}
}