using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExpectationBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var typeArguments = mockType.Type.IsOpenGeneric ?
			$"<{string.Join(", ", mockType.Type.TypeArguments.Select(_ => _.FullyQualifiedName))}>" : string.Empty;

		writer.WriteLines(
			$"""
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class {mockType.ExpectationsName}
			""");

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

		MockConstructorExpectationsBuilder.Build(writer, mockType, mockType.ExpectationsFullyQualifiedName);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, mockType);

		writer.Indent--;
		writer.WriteLine("}");
	}
}