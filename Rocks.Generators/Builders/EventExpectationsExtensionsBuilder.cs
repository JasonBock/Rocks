using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;

namespace Rocks.Builders
{
	internal static class EventExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			writer.WriteLine($"internal static class MethodAdornmentsOf{typeToMockName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Events)
			{
				// TODO: need to figure out how to grab the args type, maybe look at the "Add" method?
				writer.WriteLine($"internal static MethodAdornments<{typeToMockName}> Raises{result.Value.Name}(this MethodAdornments<{typeToMockName}> self, EventArgs args)");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine($"self.Handler.AddRaiseEvent(new(\"{result.Value.Name}\", args));");
				writer.WriteLine($"return self;");
				writer.Indent--;
				writer.WriteLine("}");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}