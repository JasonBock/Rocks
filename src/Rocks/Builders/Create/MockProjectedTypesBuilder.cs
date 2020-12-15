using System.CodeDom.Compiler;

namespace Rocks.Builders.Create
{
	// Projected types are any types that need to be generated for Rocks to work correctly.
	// E.g. methods that have ref/out parameters or they return pointer types,
	// other types are gen'd to support "esoteric" types.
	internal static class MockProjectedTypesBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			MockDelegateBuilder.Build(writer, information);
			MockArgTypeBuilder.Build(writer, information);
			MockProjectedTypesAdornmentsBuilder.Build(writer, information);
		}
	}
}