using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders
{
	internal static class MethodExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			writer.WriteLine($"internal static class MethodExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Methods)
			{
				MethodExpectationsExtensionsMethodBuilder.Build(writer, result);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}