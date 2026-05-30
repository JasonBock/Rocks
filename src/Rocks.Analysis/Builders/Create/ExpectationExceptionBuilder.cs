using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class ExpectationExceptionBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method,
		string message, string expectationsPropertyName, string mockTypeName)
	{
		writer.WriteLines(
			$$""""
			this.{{expectationsPropertyName}}.WasExceptionThrown = true;
			throw new global::Rocks.Exceptions.ExpectationException(
				$"""
				{{message}} {typeof({{mockTypeName}}).GetMemberDescription({{method.MemberIdentifier}})}
			"""");

		writer.Indent += 2;

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

		writer.Indent -= 2;

		writer.WriteLines(
			$$""""
				""");
			"""");
	}
}