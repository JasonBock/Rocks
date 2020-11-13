using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class PropertyExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if(information.Properties.Any(_ => !_.Value.IsIndexer))
			{
				writer.WriteLine($"internal static class PropertyExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties.Where(_ => !_.Value.IsIndexer))
				{
					PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer))
			{
				writer.WriteLine($"internal static class IndexerExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties.Where(_ => _.Value.IsIndexer))
				{
					IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}
		}
	}
}