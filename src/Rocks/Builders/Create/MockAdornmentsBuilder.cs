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

		// Create the intermediate interface.
		writer.WriteLines(
			$$"""
			public interface IAdornmentsFor{{mockType.Type.FlattenedName}}<TAdornments>
				: global::Rocks.IAdornments<TAdornments>
				where TAdornments : IAdornmentsFor{{mockType.Type.FlattenedName}}<TAdornments>
			{ }
			""");
		writer.WriteLine();

		// Create each custom adornment type.
		foreach (var adornments in adornmentsPipelineInformation)
		{
			writer.WriteLines(
				$$"""
				public sealed class AdornmentsForHandler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}}
					: {{adornments.FullyQualifiedName}}, IAdornmentsFor{{mockType.Type.FlattenedName}}<AdornmentsForHandler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}}>{{adornments.Constraints}}
				{ 
					public AdornmentsForHandler{{adornments.MemberIdentifier}}({{expectationsFQN}}.Handler{{adornments.MemberIdentifier}}{{adornments.TypeArguments}} handler)
						: base(handler) { }				
				}
				""");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}