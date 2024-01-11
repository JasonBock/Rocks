using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

// Projected types are any types that need to be generated for Rocks to work correctly.
// E.g. methods that have ref/out parameters or they return pointer types,
// other types are gen'd to support "esoteric" types.
internal static class MockProjectedTypesBuilder
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel type)
	{
		var wereTypesProjected = false;

		using var projectedWriter = new StringWriter();
		using var projectedIndentWriter = new IndentedTextWriter(projectedWriter, "\t");

		MockProjectedDelegateBuilder.Build(projectedIndentWriter, type);
		MockProjectedArgTypeBuilder.Build(projectedIndentWriter, type);
		MockProjectedAdornmentsTypesBuilder.Build(projectedIndentWriter, type);

		var projectedCode = projectedWriter.ToString();

		if (!string.IsNullOrWhiteSpace(projectedCode))
		{
			writer.WriteLine($"internal static class Projections");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var line in MockProjectedTypesBuilder.GetLines(projectedCode))
			{
				writer.WriteLine(line);
			}

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();

			wereTypesProjected = true;
		}

		return wereTypesProjected;
	}

	private static IEnumerable<string> GetLines(string code)
	{
		var lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

		for (var i = 0; i < lines.Length; i++)
		{
			if (i < lines.Length - 1)
			{
				yield return lines[i];
			}
		}
	}
}