using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var adornments = new HashSet<AdornmentsPipeline>();
		var adornmentsPipeline = (AdornmentsPipeline adornmentsPipelineInformation) => { adornments.Add(adornmentsPipelineInformation); };

		var expectationsFQN = mockType.ExpectationsFullyQualifiedName;

		var isUnsafe = 
			mockType.Methods.Any(
				_ => _.ReturnType.IsPointer || _.Parameters.Any(_ => _.Type.IsPointer)) ||
			mockType.Properties.Any(
				_ => _.Type.IsPointer || _.Parameters.Any(_ => _.Type.IsPointer)) ?
				"unsafe " : string.Empty;
			
		var isPartial = mockType.IsPartial ? "partial " : string.Empty;
		var isSealed = mockType.ExpectationsIsSealed ? "sealed " : string.Empty;

		writer.WriteLines(
			$$"""
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			{{mockType.Accessibility}} {{isUnsafe}}{{isSealed}}{{isPartial}}class {{mockType.ExpectationsName}}
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

		MockMembersExpectationsBuilder.Build(writer, mockType, expectationsFQN, adornmentsPipeline);

		MockHandlerListBuilder.Build(writer, mockType, expectationsFQN);

		MockExpectationsVerifyBuilder.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilder.Build(writer, mockType, expectationsFQN);

		writer.WriteLine();
		MockConstructorExtensionsBuilder.Build(writer, mockType, expectationsFQN);

		writer.WriteLine();
		MockAdornmentsBuilder.Build(writer, mockType, expectationsFQN, adornments);

		writer.Indent--;
		writer.WriteLine("}");

		MockEventExtensionsBuilder.Build(writer, mockType, expectationsFQN);
	}
}