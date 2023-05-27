using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilderV3
{
	internal static bool Build(IndentedTextWriter writer, TypeModel type, Compilation compilation)
	{
		// TODO: Uncomment as more progress is made.
		var wereTypesProjected = MockProjectedTypesBuilder.Build(writer, information, compilation);

		writer.WriteLine($"internal static class CreateExpectationsOf{type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilderV3.Build(writer, type);
		//MockPropertyExtensionsBuilder.Build(writer, information);
		//MockConstructorExtensionsBuilder.Build(writer, information, compilation);

		writer.WriteLine();
		MockTypeBuilderV3.Build(writer, type, compilation);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilder.Build(writer, information);
		//PropertyExpectationsExtensionsBuilder.Build(writer, information);
		//EventExpectationsExtensionsBuilder.Build(writer, information);

		return wereTypesProjected;
	}
}