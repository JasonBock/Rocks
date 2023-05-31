using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilderV3
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereTypesProjected = MockProjectedTypesBuilderV3.Build(writer, mockType);

		writer.WriteLine($"internal static class CreateExpectationsOf{mockType.Type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilderV3.Build(writer, mockType);
		MockPropertyExtensionsBuilderV3.Build(writer, mockType);
		MockConstructorExtensionsBuilderV3.Build(writer, mockType);

		writer.WriteLine();
		MockTypeBuilderV3.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilderV3.Build(writer, mockType);
		PropertyExpectationsExtensionsBuilderV3.Build(writer, mockType);
		EventExpectationsExtensionsBuilderV3.Build(writer, mockType);

		return wereTypesProjected;
	}
}