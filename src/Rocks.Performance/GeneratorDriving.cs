using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class GeneratorDriving
{
	public GeneratorDriving()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<ITarget>();
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

		this.Driver = CSharpGeneratorDriver.Create(new RockCreateGenerator());
		this.DriverV3 = CSharpGeneratorDriver.Create(new RockCreateGenerator());
	}

	[Benchmark(Baseline = true)]
	public int Drive()
	{
		this.Driver.RunGeneratorsAndUpdateCompilation(
			this.Compilation, out _, out var diagnostics);
		return diagnostics.Length;
	}

	[Benchmark]
	public int DriveV3()
	{
		this.DriverV3.RunGeneratorsAndUpdateCompilation(
			this.Compilation, out _, out var diagnostics);
		return diagnostics.Length;
	}

	private CSharpCompilation Compilation { get; }
   private CSharpGeneratorDriver Driver { get; }
	private CSharpGeneratorDriver DriverV3 { get; }
}