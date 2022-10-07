using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Methods.Results.Length > 0)
		{
			writer.WriteLine();
			var typeToMock = information.TypeToMock!.FlattenedName;

			if (information.Methods.Results.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static class MethodExpectationsOf{typeToMock}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Methods.Results.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsExtensionsMethodBuilder.Build(writer, result);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (information.Methods.Results.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in information.Methods.Results
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType))
				{
					var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
					writer.WriteLine($"internal static class ExplicitMethodExpectationsOf{typeToMock}For{containingTypeName}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in typeGroup)
					{
						MethodExpectationsExtensionsMethodBuilder.Build(writer, result);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}
		}
	}
}