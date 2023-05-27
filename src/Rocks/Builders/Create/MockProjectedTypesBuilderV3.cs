using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

// Projected types are any types that need to be generated for Rocks to work correctly.
// E.g. methods that have ref/out parameters or they return pointer types,
// other types are gen'd to support "esoteric" types.
internal static class MockProjectedTypesBuilderV3
{
	internal static bool Build(IndentedTextWriter writer, TypeModel type, Compilation compilation)
	{
		var wereTypesProjected = false;

		using var projectedWriter = new StringWriter();
		using var projectedIndentWriter = new IndentedTextWriter(projectedWriter, "\t");

		MockProjectedDelegateBuilderV3.Build(projectedIndentWriter, type);
		MockProjectedArgTypeBuilder.Build(projectedIndentWriter, type);
		MockProjectedTypesAdornmentsBuilder.Build(projectedIndentWriter, type);

		var projectedCode = projectedWriter.ToString();

		if(!string.IsNullOrWhiteSpace(projectedCode))
		{
			var projectionsNamespace = $"ProjectionsFor{type.TypeToMock!.FlattenedName}";
			writer.WriteLine($"namespace {projectionsNamespace}");
			writer.WriteLine("{");
			writer.Indent++;

			foreach(var line in MockProjectedTypesBuilder.GetLines(projectedCode))
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
			if(i < lines.Length - 1)
			{
				var line = lines[i];
				yield return line;
			}
		}
	}
}