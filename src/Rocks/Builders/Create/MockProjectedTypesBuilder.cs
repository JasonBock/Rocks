using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

// Projected types are any types that need to be generated for Rocks to work correctly.
// E.g. methods that have ref/out parameters or they return pointer types,
// other types are gen'd to support "esoteric" types.
internal static class MockProjectedTypesBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces,
		Compilation compilation)
	{
		// TODO: Technically, we may not have any need for projections,
		// but for now, it's just easier to emit it.
		// At some point, we may want to do the work to figure out that we
		// actually created projected types, and in that case,
		// we'll create the namespace.
		// One way to do it would be to create a local IndentedTextWriter
		// that the builders would write to, and if we got any string content
		// out of that, we'd just put it into the writer.
		// However, while we can "reuse" the Indent value, we don't know
		// what the TabString value is, so we can't reliably pass that
		// into a local IndentedTextWriter.
		// See https://github.com/dotnet/runtime/issues/68726

		writer.WriteLine($"namespace ProjectionsFor{information.TypeToMock!.FlattenedName}");
		writer.WriteLine("{");
		writer.Indent++;

		MockProjectedDelegateBuilder.Build(writer, information, compilation);
		MockProjectedArgTypeBuilder.Build(writer, information, namespaces);
		MockProjectedTypesAdornmentsBuilder.Build(writer, information, namespaces);

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
	}
}