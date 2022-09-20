using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Configuration;
using Rocks.Exceptions;
using Rocks.Expectations;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
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
		var usings = new SortedSet<string>
		{
			$"using {typeof(Action).Namespace};",
			$"using {typeof(Unsafe).Namespace};",
			$"using {typeof(HandlerInformation).Namespace};",
			$"using {typeof(ExpectationException).Namespace};",
			$"using {typeof(List<>).Namespace};",
			$"using {typeof(ImmutableArray).Namespace};",
			$"using {typeof(IExpectations).Namespace};",
		};

		if (this.information.Events.Length > 0)
		{
			usings.Add($"using {typeof(EventArgs).Namespace};");
			usings.Add($"using {typeof(BindingFlags).Namespace};");
		}

		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer,
			this.configurationValues.IndentStyle == IndentStyle.Tab ? "\t" : new string(' ', (int)this.configurationValues.IndentSize));

		if (!this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
		{
			indentWriter.WriteLine($"namespace {this.information.TypeToMock!.Type.ContainingNamespace!.ToDisplayString()}");
			indentWriter.WriteLine("{");
			indentWriter.Indent++;
		}

		var namespaces = new NamespaceGatherer();

		MockBuilder.Build(indentWriter, this.information, namespaces, this.compilation);

		foreach (var @namespace in namespaces.Values)
		{
			usings.Add($"using {@namespace};");
		}

		if (!this.information.TypeToMock!.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
		{
			indentWriter.Indent--;
			indentWriter.WriteLine("}");
		}

		var code = string.Join(Environment.NewLine,
			string.Join(Environment.NewLine, usings), string.Empty, "#nullable enable", writer.ToString());

		var text = SourceText.From(code, Encoding.UTF8);
		return (this.information.Diagnostics, $"{this.information.TypeToMock!.FlattenedName}_Rock_Create.g.cs", text);
	}

	public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	public string Name { get; private set; }
	public SourceText Text { get; private set; }
}