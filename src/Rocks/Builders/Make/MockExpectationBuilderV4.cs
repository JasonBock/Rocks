using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExpectationBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		writer.WriteLine($"internal sealed class {mockType.Type.FlattenedName}MakeExpectations");
		writer.WriteLine("{");
		writer.Indent++;

		MockConstructorExpectationsBuilderV4.Build(writer, mockType);
		writer.WriteLine();
		MockMakeBuilderV4.Build(writer, mockType);

		writer.Indent--;

		if (mockType.Type.Namespace.Length > 0)
		{
			writer.WriteLine("}");
		}
		else
		{
			writer.Write("}");
		}
	}
}