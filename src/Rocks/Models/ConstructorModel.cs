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
	internal ConstructorModel(IMethodSymbol constructor) 
	{
		this.Parameters = constructor.Parameters.Select(_ => new ParameterModel(_)).ToImmutableArray();
	}

	internal EquatableArray<ParameterModel> Parameters { get; }
}