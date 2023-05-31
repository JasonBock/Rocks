using Microsoft.CodeAnalysis.Text;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Builders.Create;

internal sealed class RockCreateBuilderV3
{
	internal RockCreateBuilderV3(TypeMockModel mockType)
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

		var requiredNamespaces = new SortedSet<string>
		{
			"using Rocks.Extensions;",
			"using System.Collections.Generic;",
			"using System.Collections.Immutable;",
		};

		var wereTypesProjected = MockBuilderV3.Build(indentWriter, this.MockType);

		if (wereTypesProjected)
		{
			requiredNamespaces.Add($"using {(mockNamespace.Length > 0 ? $"{mockNamespace}." : string.Empty)}ProjectionsFor{this.MockType.Type.FlattenedName};");
		}

		if (mockNamespace.Length > 0)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(
			string.Join(Environment.NewLine,
				string.Join(Environment.NewLine, requiredNamespaces), writer.ToString()), 
			Encoding.UTF8);
		return ($"{this.MockType.Type.FlattenedName}_Rock_Create.g.cs", text);
	}

	public string Name { get; private set; }
	public SourceText Text { get; private set; }
	private TypeMockModel MockType { get; }
}