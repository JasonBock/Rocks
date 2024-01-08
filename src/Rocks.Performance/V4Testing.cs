using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class V4Testing
{
	public V4Testing()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ITarget>]

			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
					string Retrieve2(int value);
					string Retrieve3(int value);
					string Retrieve4(int value);
					void Work(Guid id);
					void Work2(Guid id);
					void Work3(Guid id);
					void Work4(Guid id);
			
					int Data { get; set; }
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
		this.DriverV4 = CSharpGeneratorDriver.Create(new RockAttributeGenerator());
	}

	[Benchmark(Baseline = true)]
	public int Drive()
	{
		this.Driver.RunGeneratorsAndUpdateCompilation(
			this.Compilation, out _, out var diagnostics);
		return diagnostics.Length;
	}

	[Benchmark]
	public int DriveV4()
	{
		this.DriverV4.RunGeneratorsAndUpdateCompilation(
			this.Compilation, out _, out var diagnostics);
		return diagnostics.Length;
	}

	private CSharpCompilation Compilation { get; }
   private CSharpGeneratorDriver Driver { get; }
	private CSharpGeneratorDriver DriverV4 { get; }
}