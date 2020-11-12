using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders
{
	internal static class PropertyExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			writer.WriteLine($"internal static class PropertyExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties)
			{
				PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}