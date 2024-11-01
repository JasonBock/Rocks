using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class CompilationExtensions
{
	internal static (string name, string nameNoGenerics, string nameFQN, string? nameNamespace) GetExpectationsName(
		this Compilation self, TypeReferenceModel type, BuildType buildType, bool isPartial)
	{
		var typeArgumentsTickCount = type.IsOpenGeneric ? $"`{type.TypeArguments.Length}" : string.Empty;
		var typeArguments = type.IsOpenGeneric ?
			$"<{string.Join(", ", type.TypeArguments)}>" : string.Empty;

		string expectationsName;
		string expectationsNameNoGenerics;
		string expectationsFQN;

		if (isPartial)
		{
			expectationsFQN = type.Namespace is null ?
				 $"global::{type.FlattenedName}{typeArguments}" :
				 $"global::{type.Namespace}.{type.FlattenedName}{typeArguments}";
			expectationsName = $"{type.FlattenedName}{typeArguments}";
			expectationsNameNoGenerics = $"{type.FlattenedName}";
		}
		else
		{
			expectationsFQN = type.Namespace is null ?
				 $"{type.FlattenedName}{buildType}Expectations{typeArgumentsTickCount}" :
				 $"{type.Namespace}.{type.FlattenedName}{buildType}Expectations{typeArgumentsTickCount}";
			var existingType = self.GetTypeByMetadataName(expectationsFQN);

			int? id = null;

			while (existingType is not null && existingType.CanBeSeenByContainingAssembly(self.Assembly))
			{
				id = id is null ? 2 : id++;
				expectationsFQN = type.Namespace is null ?
					$"{type.FlattenedName}{buildType}Expectations{id}{typeArgumentsTickCount}" :
					$"{type.Namespace}.{type.FlattenedName}{buildType}Expectations{id}{typeArgumentsTickCount}";
				existingType = self.GetTypeByMetadataName(expectationsFQN);
			}

			expectationsFQN = type.Namespace is null ?
				$"global::{type.FlattenedName}{buildType}Expectations{id}{typeArguments}" :
				$"global::{type.Namespace}.{type.FlattenedName}{buildType}Expectations{id}{typeArguments}";
			expectationsName = $"{type.FlattenedName}{buildType}Expectations{id}{typeArguments}";
			expectationsNameNoGenerics = $"{type.FlattenedName}{buildType}Expectations{id}";
		}

		return (expectationsName, expectationsNameNoGenerics,
			expectationsFQN, type.Namespace);
	}

	internal static ImmutableArray<string> GetAliases(this Compilation self) =>
		[.. self.References.Where(_ => _.Properties.Aliases.Length > 0)
			.Select(_ => _.Properties.Aliases[0])
			.Distinct()
			.OrderBy(_ => _)];
}