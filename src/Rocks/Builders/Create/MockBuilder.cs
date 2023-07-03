using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, mockType);

		writer.WriteLine($"internal static class CreateExpectationsOf{mockType.Type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilder.Build(writer, mockType);
		MockPropertyExtensionsBuilder.Build(writer, mockType);
		MockConstructorExtensionsBuilder.Build(writer, mockType);

		writer.WriteLine();
		MockTypeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilder.Build(writer, mockType);
		PropertyExpectationsExtensionsBuilder.Build(writer, mockType);
		EventExpectationsExtensionsBuilder.Build(writer, mockType);

		return wereTypesProjected;
	}
}