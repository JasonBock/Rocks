using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class ExpectationExceptionBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method,
		VariablesNamingContext namingContext, string message, string expectationsPropertyName, string mockTypeName)
	{
		const string expectationMessageVariable = "expectationMessage";

		writer.WriteLines(
			$$""""
			{
				this.{{expectationsPropertyName}}.WasExceptionThrown = true;
				var @{{namingContext[expectationMessageVariable]}} =
					$"""
					{{message}} {typeof({{mockTypeName}}).GetMemberDescription({{method.MemberIdentifier}})}
			"""");

		writer.Indent += 3;

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

		writer.Indent -= 3;

		writer.WriteLines(
			$$""""
					""";
				this.{{expectationsPropertyName}}.ExpectationFailures.Add(@{{namingContext[expectationMessageVariable]}});
				throw new global::Rocks.Exceptions.ExpectationException(@{{namingContext[expectationMessageVariable]}});		
			}			
			"""");
	}
}