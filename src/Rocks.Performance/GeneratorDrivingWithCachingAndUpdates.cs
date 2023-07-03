using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class GeneratorDrivingWithCachingAndUpdates
{
	public GeneratorDrivingWithCachingAndUpdates()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Target
				{
					public virtual string Retrieve(int value) => value.ToString();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Target>();
					}
				}
			}
			""");
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location)
			});

		this.Compilation = CSharpCompilation.Create("generator", new[] { syntaxTree },
			references, new(OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true,
				generalDiagnosticOption: ReportDiagnostic.Error));
		this.CompilationV3 = CSharpCompilation.Create("generator", new[] { syntaxTree },
			references, new(OutputKind.DynamicallyLinkedLibrary,
				allowUnsafe: true,
				generalDiagnosticOption: ReportDiagnostic.Error));

		this.Driver = CSharpGeneratorDriver.Create(
			generators: new ISourceGenerator[] { new RockCreateGenerator().AsSourceGenerator() },
			driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));
		this.DriverV3 = CSharpGeneratorDriver.Create(
			generators: new ISourceGenerator[] { new RockCreateGenerator().AsSourceGenerator() },
			driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));
	}

	[Benchmark(Baseline = true)]
	public IncrementalStepRunReason Drive()
	{
		var newDriver = this.Driver.RunGeneratorsAndUpdateCompilation(this.Compilation, out _, out _);
		var newCompilation = this.Compilation.ReplaceSyntaxTree(this.Compilation.SyntaxTrees[0],
			CSharpSyntaxTree.ParseText(
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Target
				{
					public virtual string Retrieve(int value) => value.ToString();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Target>();
					}
				}
			}
			"""));
		newDriver = newDriver.RunGeneratorsAndUpdateCompilation(newCompilation, out _, out _);
		var result = newDriver.GetRunResult().Results.Single();
		var outputs = result.TrackedSteps["RockCreate"].Single().Outputs;
		return outputs[0].Reason;
	}

	[Benchmark]
	public IncrementalStepRunReason DriveV3()
	{
		var newDriver = this.DriverV3.RunGeneratorsAndUpdateCompilation(this.CompilationV3, out _, out _);
		var newCompilation = this.CompilationV3.ReplaceSyntaxTree(this.CompilationV3.SyntaxTrees[0],
			CSharpSyntaxTree.ParseText(
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Target
				{
					public virtual string Retrieve(int value) => value.ToString();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Target>();
					}
				}
			}
			"""));
		newDriver = newDriver.RunGeneratorsAndUpdateCompilation(newCompilation, out _, out _);
		var result = newDriver.GetRunResult().Results.Single();
		var outputs = result.TrackedSteps["RockCreate"].Single().Outputs;
		return outputs[0].Reason;
	}

	private CSharpCompilation Compilation { get; }
	private CSharpCompilation CompilationV3 { get; }
	private CSharpGeneratorDriver Driver { get; }
	private CSharpGeneratorDriver DriverV3 { get; }
}