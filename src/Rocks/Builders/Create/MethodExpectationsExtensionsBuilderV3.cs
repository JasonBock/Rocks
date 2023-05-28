using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		if (type.Methods.Length > 0)
		{
			writer.WriteLine();
			var typeToMock = type.MockType.FlattenedName;

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static class MethodExpectationsOf{type.MockType.FlattenedName}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var method in type.Methods.Where(
					_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsExtensionsMethodBuilderV3.Build(writer, method);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (type.Methods.Results.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in type.Methods.Results
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