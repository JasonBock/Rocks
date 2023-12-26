using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, string expectationsFullyQualifiedName)
	{
		if (type.Methods.Length > 0)
		{
			writer.WriteLine();
			var typeToMock = type.Type.FlattenedName;

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}MethodExpectations
					{
						private readonly {{expectationsFullyQualifiedName}} expectations;

						internal {{typeToMock}}MethodExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.expectations = expectations;
						
					""");

				writer.Indent++;

				foreach (var method in type.Methods.Where(
					_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsMethodBuilderV4.Build(writer, method, expectationsFullyQualifiedName);
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
					writer.WriteLine($"internal sealed class {typeToMock}ExplicitMethodExpectationsFor{containingTypeName}");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var method in typeGroup)
					{
						MethodExpectationsMethodBuilderV4.Build(writer, method, expectationsFullyQualifiedName);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}
		}
	}
}