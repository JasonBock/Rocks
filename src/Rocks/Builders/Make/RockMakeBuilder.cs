using Microsoft.CodeAnalysis.Text;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Builders.Make;

internal sealed class RockMakeBuilder
{
	internal RockMakeBuilder(TypeMockModel mockType)
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

		MockExtensionsBuilder.Build(indentWriter, this.MockType);

		if (mockNamespace.Length > 0)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(writer.ToString(), Encoding.UTF8);
		var name = $"{this.MockType.Type.FullyQualifiedName
			.Replace("global::", string.Empty)
			.Replace(":", string.Empty)
			.Replace("<", string.Empty)
			.Replace(">", string.Empty)}_Rock_Make.g.cs"; 
		return (name, text);
	}

	public string Name { get; private set; }
	public SourceText Text { get; private set; }
	private TypeMockModel MockType { get; }
}