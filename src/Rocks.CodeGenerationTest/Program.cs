using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Rocks;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Rocks.CodeGenerationTest;

var targetTypes = new Type[] { typeof(object), typeof(Dictionary<,>), typeof(ImmutableArray), typeof(HttpMessageHandler) };

Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
GenerateForBaseClassLibrary(new RockCreateGenerator(), targetTypes);
Console.WriteLine();
Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
GenerateForBaseClassLibrary(new RockMakeGenerator(), targetTypes);

static void GenerateForBaseClassLibrary(IIncrementalGenerator generator, Type[] targetTypes)
{
	var isCreate = generator is RockCreateGenerator;
	var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();

	var discoveredTypes = new ConcurrentDictionary<Type, byte>();

	foreach (var assembly in assemblies)
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
	//var types = new Type[] { typeof(System.Collections.Hashtable) };
	//var types = new Type[] { Type.GetType("System.Diagnostics.DebugProvider")! };
	var code = GetCode(types, isCreate);
	var syntaxTree = CSharpSyntaxTree.ParseText(code);
	var references = AppDomain.CurrentDomain.GetAssemblies()
		.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
		.Select(_ => MetadataReference.CreateFromFile(_.Location))
		.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)))
		.Concat(new[]
		{
			MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
		});
	var compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
		references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

	var driver = CSharpGeneratorDriver.Create(generator);
	driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

	Console.WriteLine($"Number of types found: {types.Length}");
	Console.WriteLine(
		$"Does generator compilation have any warning or error diagnostics? {diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning)}");

	using var outputStream = new MemoryStream();
	var result = outputCompilation.Emit(outputStream);

	Console.WriteLine($"Was emit successful? {result.Success}");

	var errorDiagnostics = result.Diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Error).ToArray();

	Console.WriteLine($"How many error diagnostics were reported from emit? {errorDiagnostics.Length}");

	var errors = result.Diagnostics
		.Where(_ => _.Severity == DiagnosticSeverity.Error)
		.Select(_ => new
		{
			_.Id,
			Description = _.ToString(),
		})
		.OrderBy(_ => _.Id);

	foreach (var error in errors)
	{
		Console.WriteLine(
			$"Error - Id: {error.Id}, Description: {error.Description}");
	}

	var ignoredWarnings = new[] { "CS0618", "SYSLIB0001", "CS1701" };
	var warnings = result.Diagnostics
		.Where(_ => _.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id))
		.Select(_ => new
		{
			_.Id,
			Description = _.ToString(),
		})
		.OrderBy(_ => _.Id);

	foreach (var warning in warnings)
	{
		Console.WriteLine(
			$"Warning - Id: {warning.Id}, Description: {warning.Description}");
	}

	/*
	Assert.Multiple(() =>
	{
		Assert.That(types, Is.Not.Empty);
		Assert.That(diagnostics.Any(
			_ => _.Severity == DiagnosticSeverity.Error || _.Severity == DiagnosticSeverity.Warning), Is.False);

		using var outputStream = new MemoryStream();
		var result = outputCompilation.Emit(outputStream);

		Assert.That(result.Success, Is.True);

		var errorDiagnostics = result.Diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Error).ToArray();
		Assert.That(errorDiagnostics, Is.Empty);

		var ignoredWarnings = new[] { "CS0618", "SYSLIB0001", "CS1701" };
		var warningGroups = result.Diagnostics
			.Where(_ => _.Severity == DiagnosticSeverity.Warning && !ignoredWarnings.Contains(_.Id))
			.GroupBy(_ => _.Id)
			.Select(_ => new
			{
				Id = _.Key,
				Count = _.Count(),
				_.ToArray()[0].Descriptor.Title,
			})
			.OrderByDescending(_ => _.Count);

		foreach (var warningGroup in warningGroups)
		{
			TestContext.WriteLine(
				$"Id: {warningGroup.Id}, Count: {warningGroup.Count}, Description: {warningGroup.Title}");
		}
	});
	*/
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