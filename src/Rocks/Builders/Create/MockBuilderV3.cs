using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilderV3
{
	internal static bool Build(IndentedTextWriter writer, TypeMockModel type, Compilation compilation)
	{
		// TODO: Uncomment as more progress is made.
		var wereTypesProjected = MockProjectedTypesBuilderV3.Build(writer, type);

		writer.WriteLine($"internal static class CreateExpectationsOf{type.MockType.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilderV3.Build(writer, type);
		//MockPropertyExtensionsBuilder.Build(writer, information);
		//MockConstructorExtensionsBuilder.Build(writer, information, compilation);

		writer.WriteLine();
		MockTypeBuilderV3.Build(writer, type, compilation);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilderV3.Build(writer, type);
		//PropertyExpectationsExtensionsBuilder.Build(writer, information);
		//EventExpectationsExtensionsBuilder.Build(writer, information);

		return wereTypesProjected;
	}
}