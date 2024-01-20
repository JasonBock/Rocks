using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsBuilder
{
	internal static IEnumerable<ExpectationMapping> Build(IndentedTextWriter writer, TypeMockModel type, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		if (type.Methods.Length > 0)
		{
			writer.WriteLine();

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLines(
					$$"""
					internal sealed class MethodExpectations
					{
						internal MethodExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");

				writer.Indent++;
				var methodAdornmentsFQNNames = new List<string>();

				foreach (var method in type.Methods.Where(
					_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					MethodExpectationsMethodBuilder.Build(writer, method, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
					writer.WriteLine();
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				yield return new($"{expectationsFullyQualifiedName}.MethodExpectations", "Methods");
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
						internal sealed class ExplicitMethodExpectationsFor{{containingTypeName}}
						{
							internal ExplicitMethodExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
								this.Expectations = expectations;
						
						""");

					writer.Indent++;

					foreach (var method in typeGroup)
					{
						MethodExpectationsMethodBuilder.Build(writer, method, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
						writer.WriteLine();
					}

					writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
					writer.Indent--;
					writer.WriteLine("}");

					yield return new($"{expectationsFullyQualifiedName}.ExplicitMethodExpectationsFor{containingTypeName}", $"ExplicitMethodsFor{containingTypeName}");
				}
			}
		}
	}
}