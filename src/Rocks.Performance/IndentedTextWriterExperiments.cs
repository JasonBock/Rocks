using BenchmarkDotNet.Attributes;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Performance;

#pragma warning disable CA1822 // Mark members as static
[MemoryDiagnoser]
public class IndentedTextWriterExperiments
{
	private static void WriteText(IndentedTextWriter writer)
	{
		// This should around 3K bytes
		for (var i = 0; i < 10; i++)
		{
			// 100 bytes
			writer.WriteLine("0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789");
		}

		writer.Indent++;
		for (var i = 0; i < 10; i++)
		{
			// 100 bytes
			writer.WriteLine("0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789");
		}
		writer.Indent--;

		for (var i = 0; i < 10; i++)
		{
			// 100 bytes
			writer.WriteLine("0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789");
		}
	}

	[Benchmark(Baseline = true)]
	public string Create()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

	  WriteText(indentWriter);

		return writer.ToString();
	}

	[Benchmark]
	[Arguments(20)]
	[Arguments(40)]
	[Arguments(100)]
	[Arguments(200)]
	[Arguments(400)]
	[Arguments(800)]
	[Arguments(1600)]
	[Arguments(3200)]
	[Arguments(6400)]
	[Arguments(12000)]
	public string CreateWithBuilderSetToCapacity(int capacity)
	{
		var builder = new StringBuilder(capacity);
		using var writer = new StringWriter(builder);
		using var indentWriter = new IndentedTextWriter(writer, "\t");

	  WriteText(indentWriter);

		return builder.ToString();
	}
}
#pragma warning restore CA1822 // Mark members as static