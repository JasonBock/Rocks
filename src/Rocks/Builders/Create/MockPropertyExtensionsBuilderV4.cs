using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockPropertyExtensionsBuilderV4
{
	internal static IEnumerable<string> Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		if (mockType.Properties.Length > 0)
		{
			foreach (var property in MockPropertyExtensionsBuilderV4.BuildProperties(writer, mockType, expectationsFullyQualifiedName))
			{
				yield return property;
			}

			foreach (var indexer in MockPropertyExtensionsBuilderV4.BuildIndexers(writer, mockType, expectationsFullyQualifiedName))
			{
				yield return indexer;
			}
		}
	}

	private static IEnumerable<string> BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal {expectationsFullyQualifiedName}.{mockType.Type.FlattenedName}IndexerExpectations Indexers {{ get; }}");
			yield return "Indexers";
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				_.IsIndexer)
			.GroupBy(_ => _.ContainingType))
		{
			var flattenedContainingTypeName = typeGroup.Key.FlattenedName;

			writer.WriteLine($"internal {expectationsFullyQualifiedName}.Explicit{flattenedContainingTypeName}IndexerExpectations ExplicitIndexersFor{flattenedContainingTypeName} {{ get; }}");
			yield return $"ExplicitIndexersFor{flattenedContainingTypeName}";
		}
	}

	private static IEnumerable<string> BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal {expectationsFullyQualifiedName}.{mockType.Type.FlattenedName}PropertyExpectations Properties {{ get; }}");
			yield return "Properties";
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				!_.IsIndexer)
			.GroupBy(_ => _.ContainingType))
		{
			var flattenedContainingTypeName = typeGroup.Key.FlattenedName;

			writer.WriteLine($"internal {expectationsFullyQualifiedName}.Explicit{flattenedContainingTypeName}PropertyExpectations ExplicitPropertiesFor{flattenedContainingTypeName} {{ get; }}");
			yield return $"ExplicitPropertiesFor{flattenedContainingTypeName}";
		}
	}
}