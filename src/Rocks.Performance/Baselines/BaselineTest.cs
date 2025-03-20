using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public abstract class BaselineTest
{
	private readonly CSharpCompilation compilation;
	private readonly CSharpGeneratorDriver driver =
		CSharpGeneratorDriver.Create(new RockGenerator());

	protected BaselineTest(string code)
	{
		var tree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
			]);
		this.compilation = CSharpCompilation.Create("generator", [tree],
			references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

		var compilationDiagnostics = this.compilation.GetDiagnostics();

		if (compilationDiagnostics.Length > 0)
		{
			throw new DiagnosticException(compilationDiagnostics);
		}
	}

	[Benchmark]
	public Compilation RunGenerator()
	{
		_ = this.driver.RunGeneratorsAndUpdateCompilation(
			this.compilation, out var outputCompilation, out var generatorDiagnostics);

		if (generatorDiagnostics.Length > 0)
		{
			throw new DiagnosticException(generatorDiagnostics);
		}

		if (this.compilation.SyntaxTrees.Length >= outputCompilation.SyntaxTrees.Count())
		{
			throw new DiagnosticException("Code was not generated.");
		}

		return outputCompilation;
	}
}