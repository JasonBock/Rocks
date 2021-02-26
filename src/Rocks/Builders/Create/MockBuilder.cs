using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MockBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces,
			Compilation compilation)
		{
			MockProjectedTypesBuilder.Build(writer, information, namespaces, compilation);

			writer.WriteLine($"internal static class {WellKnownNames.Create}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}{WellKnownNames.Extensions}");
			writer.WriteLine("{");
			writer.Indent++;

			namespaces.AddRange(information.Methods.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Constructors.SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(information.Properties.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Events.SelectMany(_ => _.Value.GetNamespaces()));

			MockMethodExtensionsBuilder.Build(writer, information);
			MockPropertyExtensionsBuilder.Build(writer, information);
			MockConstructorExtensionsBuilder.Build(writer, information);

			writer.WriteLine();
			MockTypeBuilder.Build(writer, information, compilation);

			writer.Indent--;
			writer.WriteLine("}");

			MethodExpectationsExtensionsBuilder.Build(writer, information);
			PropertyExpectationsExtensionsBuilder.Build(writer, information);
			EventExpectationsExtensionsBuilder.Build(writer, information);
		}
	}
}