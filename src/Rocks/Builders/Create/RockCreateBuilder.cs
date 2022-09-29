using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Configuration;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Builders.Create;

internal sealed class RockCreateBuilder
{
	private readonly Compilation compilation;
	private readonly MockInformation information;
	private readonly ConfigurationValues configurationValues;

	internal RockCreateBuilder(MockInformation information, ConfigurationValues configurationValues, Compilation compilation)
	{
		(this.information, this.configurationValues, this.compilation) =
			(information, configurationValues, compilation);
		(this.Diagnostics, this.Name, this.Text) = this.Build();
	}

	private (ImmutableArray<Diagnostic>, string, SourceText) Build()
	{
		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer,
			this.configurationValues.IndentStyle == IndentStyle.Tab ? "\t" : new string(' ', (int)this.configurationValues.IndentSize));

		indentWriter.WriteLine("#nullable enable");
		indentWriter.WriteLine();

		var mockNamespace = this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false ?
			null : this.information.TypeToMock!.Type.ContainingNamespace!.ToDisplayString();

		if (mockNamespace is not null)
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

		var wereTypesProjected = MockBuilder.Build(indentWriter, this.information, this.compilation);

		if (wereTypesProjected)
		{
			requiredNamespaces.Add($"using {mockNamespace}.ProjectionsFor{this.information.TypeToMock!.FlattenedName};");
		}

		if (!this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(
			string.Join(Environment.NewLine,
				string.Join(Environment.NewLine, requiredNamespaces), writer.ToString()), 
			Encoding.UTF8);
		return (this.information.Diagnostics, $"{this.information.TypeToMock!.FlattenedName}_Rock_Create.g.cs", text);
	}

	public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	public string Name { get; private set; }
	public SourceText Text { get; private set; }
}