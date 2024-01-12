using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExpectationBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		writer.WriteLine($"internal sealed class {mockType.Type.FlattenedName}MakeExpectations");
		writer.WriteLine("{");
		writer.Indent++;

		var expectationsFullyQualifiedName = mockType.Type.Namespace == string.Empty ?
			$"global::{mockType.Type.FlattenedName}MakeExpectations" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}MakeExpectations";

		MockConstructorExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}