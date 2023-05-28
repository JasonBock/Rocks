using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Rocks.Builders.Create;

internal sealed class RockCreateBuilderV3
{
	// TODO: I'm wondering if there really should be a need
	// for the compilation when I'm building the code.
	// At this point, all of the needed information should arguably be
	// in the models.
	internal RockCreateBuilderV3(TypeMockModel information, Compilation compilation)
	{
		(this.Type, this.Compilation) = (information, compilation);
		(this.Name, this.Text) = this.Build();
	}

	private (string, SourceText) Build()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		indentWriter.WriteLine("#nullable enable");
		indentWriter.WriteLine();

		var mockNamespace = this.Type.Namespace;

		if (mockNamespace is not null)
		{
			indentWriter.WriteLine($"namespace {this.Type.Namespace}");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;
		}

		// TODO: I'm not sure I need all three here, unless
		// they are all here for extension methods in these namespaces.
		var requiredNamespaces = new SortedSet<string>
		{
			"using Rocks.Extensions;",
			"using System.Collections.Generic;",
			"using System.Collections.Immutable;",
		};

		var wereTypesProjected = MockBuilderV3.Build(indentWriter, this.Type, this.Compilation);

		if (wereTypesProjected)
		{
			requiredNamespaces.Add($"using {(mockNamespace is not null ? $"{mockNamespace}." : string.Empty)}ProjectionsFor{this.Type.FlattenedName};");
		}

		if (mockNamespace is not null)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(
			string.Join(Environment.NewLine,
				string.Join(Environment.NewLine, requiredNamespaces), writer.ToString()), 
			Encoding.UTF8);
		return ($"{this.Type.FlattenedName}_Rock_Create.g.cs", text);
	}

	private Compilation Compilation { get;  }
	public string Name { get; private set; }
	public SourceText Text { get; private set; }
	private TypeMockModel Type { get; }
}