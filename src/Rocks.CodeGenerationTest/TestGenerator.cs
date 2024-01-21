using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.CodeGenerationTest.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Rocks.CodeGenerationTest;

internal static class TestGenerator
{
	/* 
	These diagnostics relate to:

	* Obsolete members and types (CS0612, CS0618, CS0619, CS0672, CS0809, SYSLIB0017, SYSLIB0050, SYSLIB0051)
	* Nullability (CS8610, CS8765, CS8613)
	* Runtime policy (CS1701, CS1702)
	* UTF7 encoding (SYSLIB0001)
	* Code access security (SYSLIB0003)

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
		{ "SYSLIB0001", ReportDiagnostic.Info },
		{ "SYSLIB0003", ReportDiagnostic.Info },
		{ "SYSLIB0017", ReportDiagnostic.Info },
		{ "SYSLIB0050", ReportDiagnostic.Info },
		{ "SYSLIB0051", ReportDiagnostic.Info },
	};

	internal static Type[] GetTargets(HashSet<Assembly> targetAssemblies, Type[] typesToIgnore,
		 Type[] typesToLoadAssembliesFrom,
		 Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases)
	{
		var initialTypes = targetAssemblies.SelectMany(
			 _ => _.GetTypes().Where(
				  _ => _.IsPublic && !_.IsSubclassOf(typeof(Delegate)) && !_.IsValueType && !_.IsSealed && !typesToIgnore.Contains(_))).ToArray();
		return TestGenerator.GetMockableTypes(initialTypes, true, typesToLoadAssembliesFrom, genericTypeMappings, aliases);
	}

	private static Type[] GetMockableTypes(Type[] types, bool isCreate, Type[] typesToLoadAssembliesFrom,
		  Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases)
	{
		static string GetValidationCode(Type[] types, Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases)
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
				indentWriter.WriteLine($"\tinternal static void Target{i}({type.GetTypeDefinition(genericTypeMappings, aliases)} target) {{ }}");
			}

			writer.WriteLine("}");
			return writer.ToString();
		}

		var code = GetValidationCode(types, genericTypeMappings, aliases);
		var assemblies = types.Select(_ => _.Assembly).ToHashSet();
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			 .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			 .Select(_ => MetadataReference.CreateFromFile(_.Location))
			 .Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location).WithAliases(aliases)))
			 .Concat(new[]
			 {
					 MetadataReference.CreateFromFile(typeof(RockAttributeGenerator).Assembly.Location),
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
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var validTypes = new List<Type>();

		foreach (var methodNode in syntaxTree.GetRoot().DescendantNodes(_ => true)
			 .OfType<MethodDeclarationSyntax>())
		{
			var methodSymbol = model.GetDeclaredSymbol(methodNode)!;
			var typeSymbol = methodSymbol.Parameters[0].Type;

			if (MockModel.Create(methodNode, typeSymbol, model, isCreate ? BuildType.Create : BuildType.Make, true).Information is not null)
			{
				var methodIndex = methodSymbol.Name.Replace("Target", string.Empty, StringComparison.InvariantCulture);
				var typeIndex = int.Parse(methodIndex, CultureInfo.InvariantCulture);
				validTypes.Add(types[typeIndex]);
			}
		}

		return [.. validTypes];
	}

	internal static ImmutableArray<Issue> Generate(IIncrementalGenerator generator, Type[] targetTypes, Type[] typesToLoadAssembliesFrom,
		 Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases, BuildType buildType)
	{
		var isCreate = buildType == BuildType.Create;
		var issues = new List<Issue>();
		var assemblies = targetTypes.Select(_ => _.Assembly).ToHashSet();
		var code = GetCode(targetTypes, isCreate, genericTypeMappings, aliases);

		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(assemblies.Select(_ => MetadataReference.CreateFromFile(_.Location).WithAliases(aliases)))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(RockAttributeGenerator).Assembly.Location),
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

		return [.. issues];
	}

	internal static void Generate(IIncrementalGenerator generator, string code, Type[] typesToLoadAssembliesFrom)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			 .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			 .Select(_ => MetadataReference.CreateFromFile(_.Location))
			 .Concat(new[]
			 {
					 MetadataReference.CreateFromFile(typeof(RockAttributeGenerator).Assembly.Location),
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

	static string GetCode(Type[] types, bool isCreate, Dictionary<Type, Dictionary<string, string>>? genericTypeMappings, string[] aliases)
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
			indentWriter.WriteLine($"[assembly: Rock{(isCreate ? "Create" : "Make")}<{types[i].GetTypeDefinition(genericTypeMappings, aliases)}>]");
		}

		return writer.ToString();
	}
}