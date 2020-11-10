using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Rocks.Builders;
using Rocks.Exceptions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace Rocks
{
	internal sealed class RockCreateBuilder
	{
		private readonly MockInformation information;

		internal RockCreateBuilder(MockInformation information)
		{
			this.information = information;
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

			if (!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				usings.Add($"using {this.information.TypeToMock.ContainingNamespace!.ToDisplayString()};");
			}

			using var writer = new StringWriter();
			// TODO:
			// Can we read .editorconfig to figure out the space/tab + indention
			using var indentWriter = new IndentedTextWriter(writer, "	");

			if (!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				indentWriter.WriteLine($"namespace {this.information.TypeToMock.ContainingNamespace!.ToDisplayString()}");
				indentWriter.WriteLine("{");
				indentWriter.Indent++;
			}

			ExtensionsBuilder.Build(indentWriter, this.information, usings);

			if (!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				indentWriter.Indent--;
				indentWriter.WriteLine("}");
			}

			var code = string.Join(Environment.NewLine,
				string.Join(Environment.NewLine, usings), string.Empty, "#nullable enable", writer.ToString());

			var text = SourceText.From(code, Encoding.UTF8);
			return (this.information.Diagnostics, $"{this.information.TypeToMock.Name}_Mock.g.cs", text);
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public string Name { get; private set; }
		public SourceText Text { get; private set; }
	}
}