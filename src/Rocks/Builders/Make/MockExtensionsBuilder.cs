using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces,
		Compilation compilation)
	{
		writer.WriteLine($"internal static class Make{WellKnownNames.Expectations}Of{information.TypeToMock!.FlattenedName}{WellKnownNames.Extensions}");
		writer.WriteLine("{");
		writer.Indent++;

		namespaces.AddRange(information.Methods.SelectMany(_ => _.Value.GetNamespaces()));
		namespaces.AddRange(information.Constructors.SelectMany(_ => _.GetNamespaces()));
		namespaces.AddRange(information.Properties.SelectMany(_ => _.Value.GetNamespaces()));
		namespaces.AddRange(information.Events.SelectMany(_ => _.Value.GetNamespaces()));

		MockConstructorExtensionsBuilder.Build(writer, information, compilation);
		writer.WriteLine();
		MockMakeBuilder.Build(writer, information, namespaces, compilation);

		writer.Indent--;
		writer.WriteLine("}");
	}
}