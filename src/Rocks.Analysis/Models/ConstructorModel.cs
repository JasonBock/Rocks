using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.Models;

internal sealed record ConstructorModel
{
	internal ConstructorModel(IMethodSymbol constructor, ModelContext modelContext)
	{
		var compilation = modelContext.SemanticModel.Compilation;
		var setsRequiredMembersAttribute = compilation.GetTypeByMetadataName(typeof(SetsRequiredMembersAttribute).FullName)!;

		this.RequiresSetsRequiredMembersAttribute = constructor.GetAttributes().Any(
			_ => _.AttributeClass!.Equals(setsRequiredMembersAttribute, SymbolEqualityComparer.Default));
		this.Parameters = [.. constructor.Parameters.Select(_ => new ParameterModel(_, modelContext))];
	}

	internal EquatableArray<ParameterModel> Parameters { get; }
	internal bool RequiresSetsRequiredMembersAttribute { get; }
}