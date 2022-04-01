using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class SimpleGeneration
{
	private readonly CSharpCompilation compilation;
	private readonly CSharpGeneratorDriver driver;

	public SimpleGeneration()
	{
		var code =
@"using Rocks;

public interface ITest
{
	void Foo();
}

public static class MockTest
{
	public static void Create() => Rock.Create<ITest>();
}";
		var tree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(new[]
			{
				MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location),
			});
		this.compilation = CSharpCompilation.Create("generator", new[] { tree },
			references, new(OutputKind.DynamicallyLinkedLibrary));
		this.driver = CSharpGeneratorDriver.Create(new RockCreateGenerator());
	}

	[Benchmark]
	public Compilation RunGenerator()
	{
		this.driver.RunGeneratorsAndUpdateCompilation(this.compilation, out var outputCompilation, out var _);
		return outputCompilation;
	}
}