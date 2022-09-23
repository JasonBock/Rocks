using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Reflection;
using System.ComponentModel;

namespace Rocks.CodeGenerationTest;

internal static class TestGenerator
{
	internal static void Generate(IIncrementalGenerator generator, HashSet<Assembly> targetAssemblies, params Type[] typesToLoadAssembliesFrom)
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
		Generate(generator, types, typesToLoadAssembliesFrom);
	}

	internal static void Generate(IIncrementalGenerator generator, Type[] targetTypes, params Type[] typesToLoadAssembliesFrom)
	{
		var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();
		var isCreate = generator is RockCreateGenerator;
		var code = GetCode(targetTypes, isCreate);

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
			references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

		var driver = CSharpGeneratorDriver.Create(generator);
		driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

		Console.WriteLine($"Number of types found: {targetTypes.Length}");
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

		var ignoredWarnings = new[] { "CS0618", "SYSLIB0001", "CS1701" };
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

	internal static void Generate(IIncrementalGenerator generator, string code, params Type[] typesToLoadAssembliesFrom)
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
			references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

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

		var ignoredWarnings = new[] { "CS0618", "SYSLIB0001", "CS1701" };
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

	static string GetCode(Type[] types, bool isCreate)
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
			indentWriter.WriteLine($"var r{i} = Rock.{(isCreate ? "Create" : "Make")}<{types[i].GetTypeDefinition()}>();");
		}

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		return writer.ToString();
	}
}