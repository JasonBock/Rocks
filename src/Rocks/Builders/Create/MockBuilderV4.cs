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
				: global::Rocks.Expectations
			{
			""");
		writer.Indent++;

		var expectationsFullyQualifiedName = mockType.Type.Namespace == string.Empty ?
			$"global::{mockType.Type.FlattenedName}CreateExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}CreateExpectations";

		MockHandlerListBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);

		var expectationPropertyNames = new List<string>();
		expectationPropertyNames.AddRange(MockMethodExtensionsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName));
		expectationPropertyNames.AddRange(MockPropertyExtensionsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName));
		writer.WriteLine();

		MockConstructorExtensionsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName, expectationPropertyNames);
		writer.WriteLine();

		MockExpectationsVerifyBuilderV4.Build(writer, mockType);
		writer.WriteLine();

		MockTypeBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);

		MethodExpectationsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);
		// TODO: Add in as new V4s are created
		/*
		PropertyExpectationsExtensionsBuilder.Build(writer, mockType);
		EventExpectationsExtensionsBuilder.Build(writer, mockType);
		*/

		writer.Indent--;
		writer.Write("}");

		return wereTypesProjected;
	}
}