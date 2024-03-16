using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExpectationBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var typeArguments = mockType.Type.IsOpenGeneric ?
			$"<{string.Join(", ", mockType.Type.TypeArguments)}>" : string.Empty;

		writer.WriteLine($"internal sealed class {mockType.Type.FlattenedName}MakeExpectations{typeArguments}");

		if (mockType.Type.Constraints.Length > 0)
		{
			writer.Indent++;

			foreach (var constraint in mockType.Type.Constraints)
			{
				writer.WriteLine(constraint);
			}

			writer.Indent--;
		}

		writer.WriteLine("{");
		writer.Indent++;

		var expectationsFullyQualifiedName = mockType.Type.Namespace is null ?
			$"global::{mockType.Type.FlattenedName}MakeExpectations{typeArguments}" :
			$"global::{mockType.Type.Namespace}.{mockType.Type.FlattenedName}MakeExpectations{typeArguments}";

		MockConstructorExpectationsBuilder.Build(writer, mockType, expectationsFullyQualifiedName);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}