using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockAdornmentsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFQN,
		HashSet<AdornmentsPipeline> adornmentsPipelineInformation)
	{
		// Create a class within, similar to Projection, like "internal static class Adornments"
		writer.WriteLines(
			$$"""
			{{mockType.Accessibility}} static class Adornments
			{
			""");
		writer.Indent++;

		var adornmentsFlattenedName = mockType.AdornmentsFlattenedName;

		// Create the intermediate interface.
		writer.WriteLines(
			$$"""
			{{mockType.Accessibility}} interface IAdornmentsFor{{adornmentsFlattenedName}}<TAdornments>
				: global::Rocks.IAdornments<TAdornments>
				where TAdornments : IAdornmentsFor{{adornmentsFlattenedName}}<TAdornments>
			{ }
			""");
		writer.WriteLine();

		var index = 0;

		// Create each custom adornment type.
		foreach (var adornments in adornmentsPipelineInformation)
		{
			writer.WriteLines(
				$$"""
				{{mockType.Accessibility}} sealed class {{adornments.Name}}{{adornments.TypeArguments}}
					: {{adornments.BaseTypeName}}, IAdornmentsFor{{adornmentsFlattenedName}}<{{adornments.Name}}{{adornments.TypeArguments}}>{{adornments.Constraints}}
				{
					{{mockType.Accessibility}} {{adornments.Name}}({{expectationsFQN}}.Handler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}} handler, global::Rocks.Expectations expectations)
						: base(handler, expectations) { }
				""");

			if (adornments.Method.ReturnType.RequiresProjectedArgument)
			{
				writer.WriteLines(
					$$"""
						{{mockType.Accessibility}} {{adornments.Name}}{{adornments.TypeArguments}} ReturnValue({{adornments.Method.ReturnType.FullyQualifiedName}} returnValue)
						{
							this.Handler.ReturnValue = returnValue;
							return this;
						}
					""");
			}

			writer.WriteLine("}");

			if (index != adornmentsPipelineInformation.Count - 1)
			{
				writer.WriteLine();
			}

			index++;
		}

		writer.Indent--;
		writer.WriteLine("}");

		foreach (var adornments in adornmentsPipelineInformation)
		{
			// Add the correct "remove" method based on
			// the type of the handler.
			var removeMethodName = adornments.Method.TypeArguments.Length == 0 ?
				"Remove" : "RemoveHandler";

			writer.WriteLines(
				$$"""

				{{mockType.Accessibility}} void Remove{{adornments.TypeArguments}}({{mockType.ExpectationsFullyQualifiedName}}.Adornments.{{adornments.Name}}{{adornments.TypeArguments}} adornments){{adornments.Constraints}}
				{
					adornments.{{removeMethodName}}(this.@handlers{{adornments.MemberIdentifier}});
					if (this.@handlers{{adornments.MemberIdentifier}}?.Count == 0) { this.@handlers{{adornments.MemberIdentifier}} = null; }
				}
				""");
		}
	}
}