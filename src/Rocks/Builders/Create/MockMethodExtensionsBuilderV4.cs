using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodExtensionsBuilderV4
{
	internal static IEnumerable<string> Build(IndentedTextWriter writer, TypeMockModel type)
	{
		if (type.Methods.Length > 0)
		{
			var typeToMockName = type.Type.FullyQualifiedName;

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				writer.WriteLine($"internal {type.Type.FlattenedName}MethodExpectations Methods {{ get; }}");
				yield return "Methods";
			}

			if (type.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				foreach (var typeGroup in type.Methods
					.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => (_.ContainingType)))
				{
					var flattenedContainingTypeName = typeGroup.Key.FlattenedName;

					writer.WriteLine($"internal Explicit{flattenedContainingTypeName}MethodExpectations ExplicitMethodsFor{flattenedContainingTypeName} {{ get; }}");
					yield return $"ExplicitMethodsFor{flattenedContainingTypeName}";
				}
			}
		}
	}
}