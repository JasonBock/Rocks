using Microsoft.CodeAnalysis;

namespace Rocks.Models;

internal sealed record ConstructorModel
{
   internal ConstructorModel(IMethodSymbol constructor, Compilation compilation) =>
		this.Parameters = [..constructor.Parameters.Select(_ => new ParameterModel(_, compilation))];

   internal EquatableArray<ParameterModel> Parameters { get; }
}