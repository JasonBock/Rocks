using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record ConstructorModel
{
   internal ConstructorModel(IMethodSymbol constructor, TypeReferenceModel mockType, Compilation compilation) =>
		(this.Parameters, this.MockType) =
		   (constructor.Parameters.Select(_ => new ParameterModel(_, mockType, compilation)).ToImmutableArray(), mockType);

   internal EquatableArray<ParameterModel> Parameters { get; }
   internal TypeReferenceModel MockType { get; }
}