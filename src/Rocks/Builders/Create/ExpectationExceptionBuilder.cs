using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ExpectationExceptionBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method, string message, uint memberIdentifier)
	{
		writer.WriteLines(
			$$""""
			throw new global::Rocks.Exceptions.ExpectationException(
				$"""
				{{message}} {this.GetType().GetMemberDescription({{memberIdentifier}})}
			"""");

		writer.Indent++;
		writer.Indent++;

		foreach (var parameter in method.Parameters)
		{
			var canFormatValue = !parameter.Type.RequiresProjectedArgument && 
				!(parameter.Type.IsRefLikeType || parameter.Type.AllowsRefLikeType);

			if (canFormatValue)
			{
				writer.WriteLine($$"""{{parameter.Name}}: {@{{parameter.Name}}.FormatValue()}""");
			}
			else
			{
				writer.WriteLine($$"""{{parameter.Name}}: <Not formattable>""");
			}
		}

		writer.Indent--;
		writer.Indent--;

		writer.WriteLines(
			$$""""
				""");
			"""");
	}
}