using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
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
			// TODO:
			// Can we read .editorconfig to figure out the space/tab + indention
			var usings = new SortedSet<string>
			{
				"using Rocks;",
				"using System.Collections.Generic;"
			};

			if(!this.information.TypeToMock.ContainingNamespace?.IsGlobalNamespace ?? false)
			{
				usings.Add($"using {this.information.TypeToMock.ContainingNamespace!.ToDisplayString()};");
			}

			var text = SourceText.From(
$@"public static class ExpectationsOf{this.information.TypeToMock.Name}Extensions
{{
}}", Encoding.UTF8);
			return (this.information.Diagnostics, $"{this.information.TypeToMock.Name}_Mock.g.cs", text);
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public string Name { get; private set; }
		public SourceText Text { get; private set; }
	}
}