using Microsoft.CodeAnalysis;
using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class EventExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			static void WriteRaisesMethod(IndentedTextWriter writer, string prefix, string typeToMockName, Extensions.EventMockableResult result, string argsType)
			{
				writer.WriteLine($"internal static {prefix}Adornments <{typeToMockName}> Raises{result.Value.Name}(this {prefix}Adornments <{typeToMockName}> self, {argsType} args)");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine($"self.Handler.AddRaiseEvent(new(\"{result.Value.Name}\", args));");
				writer.WriteLine($"return self;");
				writer.Indent--;
				writer.WriteLine("}");
			}

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

				if(information.Methods.Length > 0)
				{
					WriteRaisesMethod(writer, "Method", typeToMockName, result, argsType);
				}

				if (information.Properties.Length > 0)
				{
					if(!information.Properties.Any(_ => _.Value.IsIndexer))
					{
						WriteRaisesMethod(writer, "Property", typeToMockName, result, argsType);
					}
					if (information.Properties.Any(_ => _.Value.IsIndexer))
					{
						WriteRaisesMethod(writer, "Indexer", typeToMockName, result, argsType);
					}
				}
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}