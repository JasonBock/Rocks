using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Configuration;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Builders.Make;

internal sealed class RockMakeBuilder
{
	private readonly Compilation compilation;
	private readonly MockInformation information;
	private readonly ConfigurationValues configurationValues;

	internal RockMakeBuilder(MockInformation information, ConfigurationValues configurationValues, Compilation compilation)
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

		if (!this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
		{
			indentWriter.WriteLine($"namespace {this.information.TypeToMock!.Type.ContainingNamespace!.ToDisplayString()}");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;
		}

		MockExtensionsBuilder.Build(indentWriter, this.information, this.compilation);

		if (!this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var text = SourceText.From(writer.ToString(), Encoding.UTF8);
		return (this.information.Diagnostics, $"{this.information.TypeToMock!.FlattenedName}_Rock_Make.g.cs", text);
	}

	public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	public string Name { get; private set; }
	public SourceText Text { get; private set; }
}