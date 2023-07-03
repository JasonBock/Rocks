using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		writer.WriteLine($"internal static class MakeExpectationsOf{mockType.Type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockConstructorExtensionsBuilder.Build(writer, mockType);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}