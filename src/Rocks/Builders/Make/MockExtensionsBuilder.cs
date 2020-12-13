using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, ImmutableHashSet<INamespaceSymbol>.Builder namespaces)
		{
			writer.WriteLine($"internal static class MakeExpectationsOf{information.TypeToMock.GetName(TypeNameOption.Flatten)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			namespaces.AddRange(information.Methods.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Constructors.SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(information.Properties.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Events.SelectMany(_ => _.Value.GetNamespaces()));

			MockConstructorExtensionsBuilder.Build(writer, information);
			writer.WriteLine();
			MockMakeBuilder.Build(writer, information, namespaces);

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}