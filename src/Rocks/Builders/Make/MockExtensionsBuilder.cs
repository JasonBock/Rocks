using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		writer.WriteLine($"internal static class MakeExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockConstructorExtensionsBuilder.Build(writer, information, compilation);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, information, compilation);

		writer.Indent--;
		writer.WriteLine("}");
	}
}