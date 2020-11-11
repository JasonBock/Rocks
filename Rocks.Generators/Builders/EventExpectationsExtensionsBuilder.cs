using Microsoft.CodeAnalysis;
using System;
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
				var argsType = nameof(EventArgs);

				if (result.Value.Type is INamedTypeSymbol namedSymbol &&
					namedSymbol?.DelegateInvokeMethod?.Parameters is { Length: 2 })
				{
					argsType = namedSymbol.DelegateInvokeMethod.Parameters[1].Type
						.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				}

				writer.WriteLine($"internal static MethodAdornments<{typeToMockName}> Raises{result.Value.Name}(this MethodAdornments<{typeToMockName}> self, {argsType} args)");
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