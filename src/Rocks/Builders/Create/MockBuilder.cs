using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var adornments = new HashSet<AdornmentsPipeline>();
		var adornmentsPipeline = (AdornmentsPipeline adornmentsPipelineInformation) => { adornments.Add(adornmentsPipelineInformation); };

		var expectationsFQN = mockType.ExpectationsFullyQualifiedName;

		writer.WriteLines(
			$$"""
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class {{mockType.ExpectationsName}}
				: global::Rocks.Expectations
			""");

		if (mockType.Type.Constraints.Length > 0)
		{
			writer.Indent++;

			foreach (var constraint in mockType.Type.Constraints)
			{
				writer.WriteLine(constraint);
			}

			writer.Indent--;
		}

		writer.WriteLine("{");

		writer.Indent++;

		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, mockType);

		MockHandlerListBuilder.Build(writer, mockType, expectationsFQN);
		writer.WriteLine();

		MockExpectationsVerifyBuilder.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilder.Build(writer, mockType, expectationsFQN);

		var expectationMappings = new List<ExpectationMapping>(mockType.MemberCount.TotalCount);

		expectationMappings.AddRange(MethodExpectationsBuilder.Build(writer, mockType, expectationsFQN, adornmentsPipeline));

		if (expectationMappings.Count > 0)
		{
			writer.WriteLine();
		}

		var currentMappingCount = expectationMappings.Count;

		expectationMappings.AddRange(PropertyExpectationsBuilder.Build(writer, mockType, expectationsFQN, adornmentsPipeline));

		if (expectationMappings.Count != currentMappingCount)
		{
			writer.WriteLine();
		}

		foreach (var expectationMapping in expectationMappings)
		{
			writer.WriteLine($"internal {expectationMapping.PropertyExpectationTypeName} {expectationMapping.PropertyName} {{ get; }}");
		}

		writer.WriteLine();

		MockConstructorExtensionsBuilder.Build(writer, mockType, expectationsFQN, expectationMappings);

		writer.WriteLine();
		MockAdornmentsBuilder.Build(writer, mockType, expectationsFQN, adornments);

		writer.Indent--;
		writer.WriteLine("}");

		MockEventExtensionsBuilder.Build(writer, mockType, expectationsFQN);

		return wereTypesProjected;
	}
}