using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class CompilationExtensions
{
	internal static (string name, string nameNoGenerics, string nameFQN) GetExpectationsName(this Compilation self, TypeReferenceModel type, BuildType buildType)
	{
		var typeArgumentsTickCount = type.IsOpenGeneric ? $"`{type.TypeArguments.Length}" : string.Empty;

		var expectationsFQN = type.Namespace is null ?
			$"{type.FlattenedName}{buildType}Expectations{typeArgumentsTickCount}" :
			$"{type.Namespace}.{type.FlattenedName}{buildType}Expectations{typeArgumentsTickCount}";
		int? id = null;

		var existingType = self.GetTypeByMetadataName(expectationsFQN);

		while (existingType is not null) 
		{
			id = id is null ? 2 : id++;
			expectationsFQN = type.Namespace is null ?
				$"{type.FlattenedName}{buildType}Expectations{id}{typeArgumentsTickCount}" :
				$"{type.Namespace}.{type.FlattenedName}{buildType}Expectations{id}{typeArgumentsTickCount}";
			existingType = self.GetTypeByMetadataName(expectationsFQN);
		}

		var typeArguments = type.IsOpenGeneric ?
			$"<{string.Join(", ", type.TypeArguments)}>" : string.Empty;

		return ($"{type.FlattenedName}{buildType}Expectations{id}{typeArguments}",
			$"{type.FlattenedName}{buildType}Expectations{id}",
			type.Namespace is null ?
				$"global::{type.FlattenedName}{buildType}Expectations{id}{typeArguments}" :
				$"global::{type.Namespace}.{type.FlattenedName}{buildType}Expectations{id}{typeArguments}");
	}

	internal static ImmutableArray<string> GetAliases(this Compilation self) => 
		self.References.Where(_ => _.Properties.Aliases.Length > 0)
		   .Select(_ => _.Properties.Aliases[0])
		   .Distinct()
		   .OrderBy(_ => _).ToImmutableArray();
}