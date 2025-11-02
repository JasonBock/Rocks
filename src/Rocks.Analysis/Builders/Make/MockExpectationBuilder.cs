using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Make;

internal static class MockExpectationBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		var isSealed = mockType.ExpectationsIsSealed ? "sealed " : string.Empty;
		var isPartial = mockType.IsPartial ? "partial " : string.Empty;

		writer.WriteLines(
			$"""
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			{mockType.Accessibility} {isSealed}{isPartial}class {mockType.ExpectationsName}
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