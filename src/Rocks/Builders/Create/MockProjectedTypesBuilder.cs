using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Rocks.Builders.Create;

// Projected types are any types that need to be generated for Rocks to work correctly.
// E.g. methods that have ref/out parameters or they return pointer types,
// other types are gen'd to support "esoteric" types.
internal static class MockProjectedTypesBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces,
		Compilation compilation)
	{
		using var projectedWriter = new StringWriter();
		using var projectedIndentWriter = new IndentedTextWriter(projectedWriter,
			MockProjectedTypesBuilder.GetTabString(writer));

		MockProjectedDelegateBuilder.Build(projectedIndentWriter, information, compilation);
		MockProjectedArgTypeBuilder.Build(projectedIndentWriter, information, namespaces);
		MockProjectedTypesAdornmentsBuilder.Build(projectedIndentWriter, information, namespaces);

		var projectedCode = projectedWriter.ToString();

		if(!string.IsNullOrWhiteSpace(projectedCode))
		{
			var projectionsNamespace = $"ProjectionsFor{information.TypeToMock!.FlattenedName}";
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

			var containingNamespace = information.TypeToMock!.Type.ContainingNamespace;

			if(containingNamespace is not null && !containingNamespace.IsGlobalNamespace)
			{
				namespaces.Add($"{containingNamespace.ToDisplayString()}.{projectionsNamespace}");
			}
			else
			{
				namespaces.Add(projectionsNamespace);
			}
		}
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

	// See https://github.com/dotnet/runtime/issues/68726
	// This is a bit "dangerous" but I made it as safe as possible.
	private static string GetTabString(IndentedTextWriter writer)
	{
		var tabStringField = typeof(IndentedTextWriter).GetField("_tabString", BindingFlags.NonPublic | BindingFlags.Instance);

		if(tabStringField is not null && tabStringField.FieldType == typeof(string))
		{
			return (string)tabStringField.GetValue(writer);
		}

		return "\t";
	}
}