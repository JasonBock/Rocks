using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Configuration;
using Rocks.Exceptions;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocks.Builders.Make
{
	internal sealed class RockMakeBuilder
	{
		private readonly MockInformation information;
		private readonly ConfigurationValues configurationValues;

		internal RockMakeBuilder(MockInformation information, ConfigurationValues configurationValues)
		{
			(this.information, this.configurationValues) = 
				(information, configurationValues);
			(this.Diagnostics, this.Name, this.Text) = this.Build();
		}

		private (ImmutableArray<Diagnostic>, string, SourceText) Build()
		{
			var usings = new SortedSet<string>
			{
				$"using {typeof(Action).Namespace};",
				$"using {typeof(IMock).Namespace};",
				$"using {typeof(ExpectationException).Namespace};",
				$"using {typeof(List<>).Namespace};",
				$"using {typeof(ImmutableArray).Namespace};"
			};

			if(this.information.Events.Length > 0)
			{
				usings.Add($"using {typeof(EventArgs).Namespace};");
				usings.Add($"using {typeof(BindingFlags).Namespace};");
			}

			using var writer = new StringWriter();
			using var indentWriter = new IndentedTextWriter(writer,
				this.configurationValues.IndentStyle == IndentStyle.Tab ? "\t" : new string(' ', (int)this.configurationValues.IndentSize));

			if (!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				indentWriter.WriteLine($"namespace {this.information.TypeToMock.ContainingNamespace!.ToDisplayString()}");
				indentWriter.WriteLine("{");
				indentWriter.Indent++;
			}

			var namespaces = new NamespaceGatherer();

			MockExtensionsBuilder.Build(indentWriter, this.information, namespaces);

			foreach (var @namespace in namespaces.Values.Where(_ => _ is not null && !string.IsNullOrWhiteSpace(_)))
			{
				usings.Add($"using {@namespace};");
			}

			if (!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				indentWriter.Indent--;
				indentWriter.WriteLine("}");
			}

			var code = string.Join(Environment.NewLine,
				string.Join(Environment.NewLine, usings), string.Empty, "#nullable enable", writer.ToString());

			var text = SourceText.From(code, Encoding.UTF8);
			return (this.information.Diagnostics, $"{this.information.TypeToMock.GetName(TypeNameOption.Flatten)}_Rock_Make.g.cs", text);
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public string Name { get; private set; }
		public SourceText Text { get; private set; }
	}
}