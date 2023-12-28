using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilderV4
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereTypesProjected = MockProjectedTypesBuilderV4.Build(writer, mockType);

		writer.WriteLines(
			$$"""
			internal sealed class {{mockType.Type.FlattenedName}}CreateExpectations
				: global::Rocks.Expectations.ExpectationsV4
			{
			""");
		writer.Indent++;

		var expectationsFullyQualifiedName = mockType.Type.Namespace == string.Empty ?
			$"global::{mockType.Type.FlattenedName}CreateExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}CreateExpectations";

		MockHandlerListBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);
		writer.WriteLine();

		MockExpectationsVerifyBuilderV4.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);

		var expectationMappings = new List<ExpectationMapping>();

		expectationMappings.AddRange(MethodExpectationsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName));

		if (expectationMappings.Count > 0)
		{
			writer.WriteLine();
		}

		var currentMappingCount = expectationMappings.Count;

		expectationMappings.AddRange(PropertyExpectationsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName));

		if (expectationMappings.Count != currentMappingCount)
		{
			writer.WriteLine();
		}

		// TODO: Add in as new V4s are created
		/*
		EventExpectationsExtensionsBuilder.Build(writer, mockType);
		*/

		foreach (var expectationMapping in expectationMappings) 
		{
			writer.WriteLine($"internal {expectationMapping.PropertyExpectationTypeName} {expectationMapping.PropertyName} {{ get; }}");
		}

		writer.WriteLine();

		MockConstructorExtensionsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName, expectationMappings);

		writer.Indent--;
		writer.Write("}");

		return wereTypesProjected;
	}
}