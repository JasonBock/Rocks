using Microsoft.CodeAnalysis.Text;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Builders.Make;

internal sealed class RockMakeBuilderV3
{
	internal RockMakeBuilderV3(TypeMockModel mockType)
	{
		this.MockType = mockType;
		(this.Name, this.Text) = this.Build();
	}

	private (string, SourceText) Build()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		indentWriter.WriteLine("#nullable enable");
		indentWriter.WriteLine();

		var mockNamespace = this.MockType.Type.Namespace;

		if (mockNamespace.Length > 0)
		{
			indentWriter.WriteLine($"namespace {mockNamespace}");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;
		}

		MockExtensionsBuilderV3.Build(indentWriter, this.MockType);

		if (mockNamespace.Length > 0)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(writer.ToString(), Encoding.UTF8);
		return ($"{this.MockType.Type.FlattenedName}_Rock_Make.g.cs", text);
	}

	public string Name { get; private set; }
	public SourceText Text { get; private set; }
	private TypeMockModel MockType { get; }
}