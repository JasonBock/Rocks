using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsBuilder
{
	internal static IEnumerable<ExpectationMapping> Build(IndentedTextWriter writer, TypeMockModel type, string expectationsFullyQualifiedName)
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
						internal {{typeToMock}}MethodExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");

				writer.Indent++;

				foreach (var method in type.Methods.Where(
					_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsMethodBuilder.Build(writer, method, expectationsFullyQualifiedName);
					writer.WriteLine();
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				yield return new($"{expectationsFullyQualifiedName}.{typeToMock}MethodExpectations", "Methods");
			}

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in type.Methods
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.ContainingType.FlattenedName))
				{
					var containingTypeName = typeGroup.Key;

					writer.WriteLines(
						$$"""
						internal sealed class {{typeToMock}}ExplicitMethodExpectationsFor{{containingTypeName}}
						{
							internal {{typeToMock}}ExplicitMethodExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
								this.Expectations = expectations;
						
						""");

					writer.Indent++;

					foreach (var method in typeGroup)
					{
						MethodExpectationsMethodBuilder.Build(writer, method, expectationsFullyQualifiedName);
						writer.WriteLine();
					}

					writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
					writer.Indent--;
					writer.WriteLine("}");

					yield return new($"{expectationsFullyQualifiedName}.{typeToMock}ExplicitMethodExpectationsFor{containingTypeName}", $"ExplicitMethodsFor{containingTypeName}");
				}
			}
		}
	}
}