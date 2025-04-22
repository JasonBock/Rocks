using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Models;

internal sealed record ConstructorModel
{
   internal ConstructorModel(IMethodSymbol constructor, ModelContext modelContext) =>
		this.Parameters = [..constructor.Parameters.Select(_ => new ParameterModel(_, modelContext))];

   internal EquatableArray<ParameterModel> Parameters { get; }
}