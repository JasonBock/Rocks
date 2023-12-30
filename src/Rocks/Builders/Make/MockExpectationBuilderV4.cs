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

		var expectationsFullyQualifiedName = mockType.Type.Namespace == string.Empty ?
			$"global::{mockType.Type.FlattenedName}MakeExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}MakeExpectations";

		MockConstructorExpectationsBuilderV4.Build(writer, mockType, expectationsFullyQualifiedName);
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