using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

/// <summary>
/// Defines constructors that can be invoked when a mock is instantiated.
/// </summary>
internal record ConstructorModel
{
   /// <summary>
   /// Creates a new <see cref="MethodMockableResult"/> instance.
   /// </summary>
   /// <param name="constructor">The <see cref="IMethodSymbol"/> to obtain information from.</param>
	/// <param name="mockType">The mock type.</param>
	/// <param name="compilation">The compilation.</param>
   internal ConstructorModel(IMethodSymbol constructor, TypeReferenceModel mockType, Compilation compilation) => 
		(this.Parameters, this.MockType) =
		   (constructor.Parameters.Select(_ => new ParameterModel(_, mockType, compilation)).ToImmutableArray(), mockType);

   internal EquatableArray<ParameterModel> Parameters { get; }
	internal TypeReferenceModel MockType { get; }
}