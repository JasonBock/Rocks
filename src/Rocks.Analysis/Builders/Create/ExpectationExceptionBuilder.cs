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

		if (method.Parameters.Length > 0 || method.TypeParameters.Length > 0)
		{
			writer.Indent += 3;

			if (method.Parameters.Length > 0)
			{
				writer.WriteLine("Parameters:");
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
			}

			if (method.TypeParameters.Length > 0)
			{
				writer.WriteLine("Type Parameters:");
				writer.Indent++;

				for (var i = 0; i < method.TypeParameters.Length; i++)
				{
					var typeParameter = method.TypeParameters[i];
					var typeArgument = method.TypeArguments[i];
					writer.WriteLine($$"""{{typeParameter.Name}}: {typeof(@{{typeParameter.FullyQualifiedName}}).FullName}""");
				}

				writer.Indent--;
			}

			writer.Indent -= 3;
		}

		writer.WriteLines(
			$$""""
					""";
				this.{{expectationsPropertyName}}.ExpectationFailures.Add(@{{namingContext[expectationMessageVariable]}});
				throw new global::Rocks.Exceptions.ExpectationException(@{{namingContext[expectationMessageVariable]}});		
			}			
			"""");
	}
}