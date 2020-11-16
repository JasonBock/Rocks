using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class ExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, ImmutableHashSet<INamespaceSymbol>.Builder namespaces)
		{
			var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

			writer.WriteLine($"internal static class ExpectationsOf{typeToMockName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			namespaces.AddRange(information.Methods.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Constructors.SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(information.Properties.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Events.SelectMany(_ => _.Value.GetNamespaces()));

			if (information.Methods.Length > 0)
			{
				writer.WriteLine($"internal static MethodExpectations<{typeToMockName}> Methods(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new MethodExpectations<{typeToMockName}>(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => !_.Value.IsIndexer))
			{
				writer.WriteLine($"internal static PropertyExpectations<{typeToMockName}> Properties(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new PropertyExpectations<{typeToMockName}>(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer))
			{
				writer.WriteLine($"internal static IndexerExpectations<{typeToMockName}> Indexers(this Expectations<{typeToMockName}> self) =>");
				writer.Indent++;
				writer.WriteLine($"new IndexerExpectations<{typeToMockName}>(self);");
				writer.Indent--;
				writer.WriteLine();
			}

			if (information.Constructors.Length > 0)
			{
				foreach(var constructor in information.Constructors)
				{
					ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
						constructor.Parameters);
				}
			}
			else
			{
				ExpectationsExtensionsConstructorBuilder.Build(writer, information.TypeToMock,
					ImmutableArray<IParameterSymbol>.Empty);
			}

			writer.WriteLine();

			MockCreateBuilder.Build(writer, information);

			writer.Indent--;
			writer.WriteLine("}");

			if (information.Methods.Length > 0)
			{
				writer.WriteLine();
				MethodExpectationsExtensionsBuilder.Build(writer, information);
			}

			if (information.Properties.Length > 0)
			{
				writer.WriteLine();
				PropertyExpectationsExtensionsBuilder.Build(writer, information);
			}

			if (information.Events.Length > 0)
			{
				writer.WriteLine();
				EventExpectationsExtensionsBuilder.Build(writer, information);
			}
		}
	}
}