using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class EventExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Methods.Length > 0)
			{
				EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Method");
			}

			if (information.Properties.Length > 0)
			{
				if (information.Properties.Any(_ => !_.Value.IsIndexer))
				{
					EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Property");
				}
				if (information.Properties.Any(_ => _.Value.IsIndexer))
				{
					EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Indexer");
				}
			}
		}

		private static void BuildAdornments(IndentedTextWriter writer, MockInformation information, string prefix)
		{
			static void BuildRaisesMethod(IndentedTextWriter writer, string extensionPrefix, string typeToMockName, EventMockableResult result, string argsType)
			{
				writer.WriteLine($"internal static {extensionPrefix}Adornments<{typeToMockName}> Raises{result.Value.Name}(this {extensionPrefix}Adornments<{typeToMockName}> self, {argsType} args)");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine($"self.Handler.AddRaiseEvent(new(\"{result.Value.Name}\", args));");
				writer.WriteLine($"return self;");
				writer.Indent--;
				writer.WriteLine("}");
			}

			var typeToMockName = information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			writer.WriteLine($"internal static class {prefix}AdornmentsOf{typeToMockName}Extensions");
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

				BuildRaisesMethod(writer, "Method", typeToMockName, result, argsType);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}