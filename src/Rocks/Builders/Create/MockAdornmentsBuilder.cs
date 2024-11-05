using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockAdornmentsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFQN,
		HashSet<AdornmentsPipeline> adornmentsPipelineInformation)
	{
		// Create a class within, similar to Projection, like "internal static class Adornments"
		writer.WriteLine("internal static class Adornments");
		writer.WriteLine("{");
		writer.Indent++;

		var adornmentsFlattenedName = mockType.AdornmentsFlattenedName;

		// Create the intermediate interface.
		writer.WriteLines(
			 $$"""
			public interface IAdornmentsFor{{adornmentsFlattenedName}}<TAdornments>
				: global::Rocks.IAdornments<TAdornments>
				where TAdornments : IAdornmentsFor{{adornmentsFlattenedName}}<TAdornments>
			{ }
			""");
		writer.WriteLine();

		// Create each custom adornment type.
		foreach (var adornments in adornmentsPipelineInformation)
		{
			writer.WriteLines(
				$$"""
				public sealed class AdornmentsForHandler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}}
					: {{adornments.FullyQualifiedName}}, IAdornmentsFor{{adornmentsFlattenedName}}<AdornmentsForHandler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}}>{{adornments.Constraints}}
				{
					public AdornmentsForHandler{{adornments.MemberIdentifier}}({{expectationsFQN}}.Handler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}} handler)
						: base(handler) { }
				""");

			if (adornments.Method.ReturnType.RequiresProjectedArgument)
			{
				writer.WriteLines(
					$$"""
						public AdornmentsForHandler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}} ReturnValue({{adornments.Method.ReturnType.FullyQualifiedName}} returnValue)
						{
							this.handler.ReturnValue = returnValue;
							return this;
						}
					""");
			}

			writer.WriteLine("}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}