﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Reflection;
using System.ComponentModel;
using Rocks.CodeGenerationTest.Extensions;
using Microsoft.CodeAnalysis.Text;

namespace Rocks.CodeGenerationTest;

internal static class TestGenerator
{
	internal static readonly Dictionary<string, ReportDiagnostic> SpecificDiagnostics = new()
	{
		{ "CS1701", ReportDiagnostic.Info },
		{ "SYSLIB0001", ReportDiagnostic.Info },
		{ "SYSLIB0003", ReportDiagnostic.Info },
		{ "SYSLIB0017", ReportDiagnostic.Info }
	};

	internal static void Generate(IIncrementalGenerator generator, HashSet<Assembly> targetAssemblies, Type[] typesToLoadAssembliesFrom,
		Dictionary<Type, Dictionary<string, string>>? genericTypeMappings)
	{
		var discoveredTypes = new ConcurrentDictionary<Type, byte>();

		foreach (var assembly in targetAssemblies)
		{
			Parallel.ForEach(assembly.GetTypes()
				.Where(_ => _.IsPublic && !_.IsSealed), _ =>
				{
					if (_.IsValidTarget())
					{
						discoveredTypes.AddOrUpdate(_, 0, (_, _) => 0);
					}
				});
		}

		var types = discoveredTypes.Keys.ToArray();
		TestGenerator.Generate(generator, types, typesToLoadAssembliesFrom, genericTypeMappings);
	}

	internal static void Generate(IIncrementalGenerator generator, Type[] targetTypes, Type[] typesToLoadAssembliesFrom,
		Dictionary<Type, Dictionary<string, string>>? genericTypeMappings)
	{
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
		Console.WriteLine($"Number of types found: {targetTypes.Length}");
		Console.WriteLine($"Does generator compilation have any warning or error diagnostics? {driverHasDiagnostics}");

		if (driverHasDiagnostics)
		{
			var errors = diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Error)
				.Select(_ => new
				{
					_.Id,
					Description = _.ToString(),
					_.Location,
				})
				.GroupBy(_ => _.Id)
				.OrderBy(_ => _.Key).ToArray();

			var warnings = diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Warning)
				.Select(_ => new
				{
					_.Id,
					Description = _.ToString(),
					_.Location,
				})
				.GroupBy(_ => _.Id)
				.OrderBy(_ => _.Key).ToArray();

			Console.WriteLine();

			if (errors.Length > 0)
			{
				Console.WriteLine("Error Counts");

				var errorCount = 0;

				foreach (var errorGroup in errors)
				{
					var errorGroupCount = errorGroup.Count();
					errorCount += errorGroupCount;
					Console.WriteLine($"\tCode: {errorGroup.Key}, Count: {errorGroupCount}");
				}

				Console.WriteLine($"Total Error Count: {errorCount}");
				Console.WriteLine();
			}

			if (warnings.Length > 0)
			{
				Console.WriteLine("Warning Counts");

				var warningCount = 0;

				foreach (var warningGroup in warnings)
				{
					var warningGroupCount = warningGroup.Count();
					warningCount += warningGroupCount;
					Console.WriteLine($"\tCode: {warningGroup.Key}, Count: {warningGroupCount}");
				}

				Console.WriteLine($"Total Warning Count: {warningCount}");
				Console.WriteLine();
			}

			var mockCode = compilation.SyntaxTrees.ToArray()[^1];

			foreach (var errorGroup in errors)
			{
				foreach (var error in errorGroup)
				{
					var errorCode = error.Location != Location.None ?
						error.Location.SourceTree!.GetText().GetSubText(error.Location.SourceSpan) :
						null;
					Console.WriteLine(
						$$"""
						Error:

						ID: {{error.Id}}
						Description: {{error.Description}}
						Code:
						{{errorCode}}

						""");
				}
			}

			foreach (var warningGroup in warnings)
			{
				foreach (var warning in warningGroup)
				{
					var warningCode = warning.Location != Location.None ?
						warning.Location.SourceTree!.GetText().GetSubText(warning.Location.SourceSpan) :
						null;
					Console.WriteLine(
						$$"""
						Warning:

						ID: {{warning.Id}}
						Description: {{warning.Description}}
						Code:
						{{warningCode}}

						""");
				}
			}
		}
		else
		{
			using var outputStream = new MemoryStream();
			var result = outputCompilation.Emit(outputStream);

			Console.WriteLine($"Was emit successful? {result.Success}");

			var errors = result.Diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Error)
				.Select(_ => new
				{
					_.Id,
					Description = _.ToString(),
					_.Location,
				})
				.GroupBy(_ => _.Id)
				.OrderBy(_ => _.Key).ToArray();

			var ignoredWarnings = Array.Empty<string>();
			var warnings = result.Diagnostics
				.Where(_ => _.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id))
				.Select(_ => new
				{
					_.Id,
					Description = _.ToString(),
					_.Location,
				})
				.GroupBy(_ => _.Id)
				.OrderBy(_ => _.Key).ToArray();

			Console.WriteLine();

			if (errors.Length > 0)
			{
				Console.WriteLine("Error Counts");

				var errorCount = 0;

				foreach (var errorGroup in errors)
				{
					var errorGroupCount = errorGroup.Count();
					errorCount += errorGroupCount;
					Console.WriteLine($"\tCode: {errorGroup.Key}, Count: {errorGroupCount}");
				}

				Console.WriteLine($"Total Error Count: {errorCount}");
				Console.WriteLine();
			}

			if (warnings.Length > 0)
			{
				Console.WriteLine("Warning Counts");

				var warningCount = 0;

				foreach (var warningGroup in warnings)
				{
					var warningGroupCount = warningGroup.Count();
					warningCount += warningGroupCount;
					Console.WriteLine($"\tCode: {warningGroup.Key}, Count: {warningGroupCount}");
				}

				Console.WriteLine($"Total Warning Count: {warningCount}");
				Console.WriteLine();
			}

			var mockCode = outputCompilation.SyntaxTrees.ToArray()[^1];

			foreach (var errorGroup in errors)
			{
				foreach (var error in errorGroup)
				{
					var errorCode = error.Location != Location.None ?
						error.Location.SourceTree!.GetText().GetSubText(error.Location.SourceSpan) :
						null;
					Console.WriteLine(
						$$"""
						Error:

						ID: {{error.Id}}
						Description: {{error.Description}}
						Code:
						{{errorCode}}

						""");
				}
			}

			foreach (var warningGroup in warnings)
			{
				foreach (var warning in warningGroup)
				{
					var warningCode = warning.Location != Location.None ? 
						warning.Location.SourceTree!.GetText().GetSubText(warning.Location.SourceSpan) : 
						null;
					Console.WriteLine(
						$$"""
						Warning:

						ID: {{warning.Id}}
						Description: {{warning.Description}}
						Code:
						{{warningCode}}

						""");
				}
			}
		}
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

		// TODO: I should see if I can do a compilation with warnings as errors, then I wouldn't even try to compile stuff
		// that has things like obsolete warnings.

		// CS0612 - "'member' is obsolete"
		// https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0612

		// CS0618 - "A class member was marked with the Obsolete attribute, such that a warning will be issued when the class member is referenced."
		// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0618

		// CS1701 - "Assuming assembly reference "Assembly Name #1" matches "Assembly Name #2", you may need to supply runtime policy."
		// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs1701

		// SYSLIB0001 - "The UTF-7 encoding is no longer in wide use among applications, and many specs now forbid its use in interchange."
		// https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0001

		// SYSLIB0003 - "Code access security is not supported"
		// https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0003

		// SYSLIB0017 - "Strong-name signing is not supported and throws PlatformNotSupportedException"
		// https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0017
		//var ignoredWarnings = new[] { "CS0612", "CS0618", "CS1701", "SYSLIB0001", "SYSLIB0003", "SYSLIB0017" };
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