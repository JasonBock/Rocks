using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExtensionsBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		writer.WriteLine($"internal static class MakeExpectationsOf{mockType.Type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockConstructorExtensionsBuilderV3.Build(writer, mockType);
		writer.WriteLine();
		MockMakeBuilderV3.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}