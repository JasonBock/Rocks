using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilderV4
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, mockType);

		writer.WriteLines(
			$$"""
			internal sealed class {{mockType.Type.FlattenedName}}CreateExpectations");
				: Expectations
			{
			""");
		writer.Indent++;

		MockHandlerListBuilderV4.Build(writer, mockType);

		var expectationPropertyNames = new List<string>();

		expectationPropertyNames.AddRange(MockMethodExtensionsBuilderV4.Build(writer, mockType));
		MockPropertyExtensionsBuilder.Build(writer, mockType);
		MockConstructorExtensionsBuilder.Build(writer, mockType);

		writer.WriteLine();

		// TODO: I'll need to create a constructor for the expectations type
		// that initializes all the member expectation properties with "new(this)".

		MockTypeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilder.Build(writer, mockType);
		PropertyExpectationsExtensionsBuilder.Build(writer, mockType);
		EventExpectationsExtensionsBuilder.Build(writer, mockType);

		return wereTypesProjected;
	}
}