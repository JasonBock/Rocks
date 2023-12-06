using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		if (type.Methods.Length > 0)
		{
			writer.WriteLine();
			var typeToMock = type.Type.FlattenedName;

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal static class MethodExpectationsOf{type.Type.FlattenedName}Extensions");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var method in type.Methods.Where(
					_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsExtensionsMethodBuilder.Build(writer, method);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in type.Methods
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.ContainingType.FlattenedName))
				{
					var containingTypeName = typeGroup.Key;
					writer.WriteLine($"internal static class ExplicitMethodExpectationsOf{typeToMock}For{containingTypeName}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var method in typeGroup)
					{
						MethodExpectationsExtensionsMethodBuilder.Build(writer, method);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}
		}
	}
}