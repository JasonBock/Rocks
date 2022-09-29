using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static bool Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, information, compilation);

		writer.WriteLine($"internal static class CreateExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilder.Build(writer, information);
		MockPropertyExtensionsBuilder.Build(writer, information);
		MockConstructorExtensionsBuilder.Build(writer, information, compilation);

		writer.WriteLine();
		MockTypeBuilder.Build(writer, information, compilation);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilder.Build(writer, information);
		PropertyExpectationsExtensionsBuilder.Build(writer, information);
		EventExpectationsExtensionsBuilder.Build(writer, information);

		return wereTypesProjected;
	}
}