using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.CodeGenerationTest.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;

namespace Rocks.CodeGenerationTest;

internal static class TestGenerator
{
	internal static readonly Dictionary<string, ReportDiagnostic> SpecificDiagnostics = new()
	{
		{ "CS0612", ReportDiagnostic.Suppress },
		{ "CS0618", ReportDiagnostic.Suppress },
		{ "CS0619", ReportDiagnostic.Suppress },
		{ "CS0809", ReportDiagnostic.Suppress },
		{ "CS8610", ReportDiagnostic.Suppress },
		{ "CS8765", ReportDiagnostic.Suppress },
		{ "CS1701", ReportDiagnostic.Info },
		{ "SYSLIB0001", ReportDiagnostic.Info },
		{ "SYSLIB0003", ReportDiagnostic.Info },
		{ "SYSLIB0017", ReportDiagnostic.Info }
	};

	internal static Type[] GetDiscoveredTypes(HashSet<Assembly> targetAssemblies, 
		Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, Type[] typesToIgnore)
	{
		var discoveredTypes = new ConcurrentDictionary<Type, byte>();

		foreach (var assembly in targetAssemblies)
		{
			Parallel.ForEach(assembly.GetTypes()
				.Where(_ => _.IsPublic && !_.IsSealed && !typesToIgnore.Contains(_)), _ =>
				{
					if (_.IsValidTarget(genericTypeMappings))
					{
						discoveredTypes.AddOrUpdate(_, 0, (_, _) => 0);
					}
				});
		}

		return discoveredTypes.Keys.ToArray();
	}

	internal static ImmutableArray<Issue> Generate(IIncrementalGenerator generator, Type[] targetTypes, Type[] typesToLoadAssembliesFrom,
		Dictionary<Type, Dictionary<string, string>>? genericTypeMappings)
	{
		var issues = new List<Issue>();
		var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();
		var isCreate = generator is RockCreateGenerator;
		var code = GetCode(targetTypes, isCreate, genericTypeMappings);

		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			});

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			});
		}

		var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
			references, new(OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true,
				generalDiagnosticOption: ReportDiagnostic.Error,
				specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));

		var driver = CSharpGeneratorDriver.Create(generator);
		driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

		var driverHasDiagnostics = diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning);

		if (driverHasDiagnostics)
		{
			issues.AddRange(diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Error ||
					_.Severity == DiagnosticSeverity.Warning)
				.Select(_ => new Issue(_.Id, _.Severity, _.ToString(), _.Location)));
			var mockCode = compilation.SyntaxTrees.ToArray()[^1];
		}
		else
		{
			using var outputStream = new MemoryStream();
			var result = outputCompilation.Emit(outputStream);

			var ignoredWarnings = Array.Empty<string>();

			issues.AddRange(result.Diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Error ||
					(_.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id)))
				.Select(_ => new Issue(_.Id, _.Severity, _.ToString(), _.Location)));
			var mockCode = outputCompilation.SyntaxTrees.ToArray()[^1];
		}

		return issues.ToImmutableArray();
	}

	internal static void Generate(IIncrementalGenerator generator, string code, Type[] typesToLoadAssembliesFrom)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(InvalidEnumArgumentException).Assembly.Location),
			});

		foreach (var typeToLoadAssembliesFrom in typesToLoadAssembliesFrom)
		{
			references = references.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeToLoadAssembliesFrom.Assembly.Location)
			});
		}

		var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
			references, new(OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true,
				generalDiagnosticOption: ReportDiagnostic.Error,
				specificDiagnosticOptions: TestGenerator.SpecificDiagnostics));

		var driver = CSharpGeneratorDriver.Create(generator);
		driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

		Console.WriteLine(
			$"Does generator compilation have any warning or error diagnostics? {diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning)}");

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

	static string GetCode(Type[] types, bool isCreate, Dictionary<Type, Dictionary<string, string>>? genericTypeMappings)
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");
		indentWriter.WriteLine("using Rocks;");
		indentWriter.WriteLine("using System;");
		indentWriter.WriteLine();
		indentWriter.WriteLine("[assembly: CLSCompliant(false)]");
		indentWriter.WriteLine();
		indentWriter.WriteLine("public static class GenerateCode");
		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine("public static void Go()");
		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		for (var i = 0; i < types.Length; i++)
		{
			indentWriter.WriteLine($"var r{i} = Rock.{(isCreate ? "Create" : "Make")}<{types[i].GetTypeDefinition(genericTypeMappings)}>();");
		}

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		return writer.ToString();
	}
}