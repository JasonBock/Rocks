using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		MockProjectedTypesBuilder.Build(writer, information, compilation);

		writer.WriteLine($"internal static class {WellKnownNames.Create}{WellKnownNames.Expectations}Of{information.TypeToMock!.FlattenedName}{WellKnownNames.Extensions}");
		writer.WriteLine("{");
		writer.Indent++;

		MockMethodExtensionsBuilder.Build(writer, information);
		MockPropertyExtensionsBuilder.Build(writer, information);
		MockConstructorExtensionsBuilder.Build(writer, information, compilation);

		writer.WriteLine();
		MockTypeBuilder.Build(writer, information, compilation);

		writer.Indent--;
		writer.WriteLine("}");

		MethodExpectationsExtensionsBuilder.Build(writer, information);
		PropertyExpectationsExtensionsBuilder.Build(writer, information);
		EventExpectationsExtensionsBuilder.Build(writer, information);
	}
}