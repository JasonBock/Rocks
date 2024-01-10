using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, mockType);

		writer.WriteLines(
			$$"""
			internal sealed class {{mockType.Type.FlattenedName}}CreateExpectations
				: global::Rocks.Expectations
			{
			""");
		writer.Indent++;

		var expectationsFullyQualifiedName = mockType.Type.Namespace == string.Empty ?
			$"global::{mockType.Type.FlattenedName}CreateExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}CreateExpectations";

		MockHandlerListBuilder.Build(writer, mockType, expectationsFullyQualifiedName);
		writer.WriteLine();

		MockExpectationsVerifyBuilder.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilder.Build(writer, mockType, expectationsFullyQualifiedName);

		var expectationMappings = new List<ExpectationMapping>();

		expectationMappings.AddRange(MethodExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName));

		if (expectationMappings.Count > 0)
		{
			writer.WriteLine();
		}

		var currentMappingCount = expectationMappings.Count;

		expectationMappings.AddRange(PropertyExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName));

		if (expectationMappings.Count != currentMappingCount)
		{
			writer.WriteLine();
		}

		foreach (var expectationMapping in expectationMappings)
		{
			writer.WriteLine($"internal {expectationMapping.PropertyExpectationTypeName} {expectationMapping.PropertyName} {{ get; }}");
		}

		writer.WriteLine();

		MockConstructorExtensionsBuilder.Build(writer, mockType, expectationsFullyQualifiedName, expectationMappings);

		writer.Indent--;

		if (mockType.Type.Namespace.Length > 0)
		{
			writer.WriteLine("}");
		}
		else
		{
			writer.Write("}");
		}

		return wereTypesProjected;
	}
}