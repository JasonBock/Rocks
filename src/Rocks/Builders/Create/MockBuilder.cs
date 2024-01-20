using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var expectationsFullyQualifiedName = mockType.Type.Namespace is null ?
			$"global::{mockType.Type.FlattenedName}CreateExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}CreateExpectations";

		writer.WriteLines(
			$$"""
			internal sealed class {{mockType.Type.FlattenedName}}CreateExpectations
				: global::Rocks.Expectations
			{
			""");
		writer.Indent++;

		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, mockType);

		MockHandlerListBuilder.Build(writer, mockType, expectationsFullyQualifiedName);
		writer.WriteLine();

		MockExpectationsVerifyBuilder.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilder.Build(writer, mockType, expectationsFullyQualifiedName);

		var expectationMappings = new List<ExpectationMapping>();
		var adornmentsFQNs = new HashSet<(string adornmentFQN, string typeArguments, string constraints)>();
		var adornmentsFQNsPipeline = (string adornmentFQN, string typeArguments, string constraints) =>
			{
				if (mockType.Events.Length > 0)
				{
					adornmentsFQNs.Add((adornmentFQN, typeArguments, constraints));
				}
			};

		expectationMappings.AddRange(MethodExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));

		if (expectationMappings.Count > 0)
		{
			writer.WriteLine();
		}

		var currentMappingCount = expectationMappings.Count;

		expectationMappings.AddRange(PropertyExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));

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
		writer.WriteLine("}");

		if (adornmentsFQNs.Count > 0)
		{
			MockEventExtensionsBuilder.Build(writer, mockType, adornmentsFQNs.ToImmutableHashSet());
		}

		return wereTypesProjected;
	}
}