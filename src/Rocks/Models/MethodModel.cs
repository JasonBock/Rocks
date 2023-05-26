using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

/// <summary>
/// Defines a method that can be mocked.
/// </summary>
internal record MethodModel
{
	/// <summary>
	/// Creates a new <see cref="MethodMockableResult"/> instance.
	/// </summary>
	/// <param name="method">The <see cref="IMethodSymbol"/> to obtain information from.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="method"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="method"/> requires an override.</param>
	/// <param name="memberIdentifier">The member identifier.</param>
	internal MethodModel(IMethodSymbol method,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
		RequiresOverride requiresOverride, uint memberIdentifier)
	{
		(this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
			 (requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

		if(requiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
		{
			this.ContainingTypeFullyQualifiedName = method.ContainingType.GetFullyQualifiedName();
			this.ContainingTypeFlattenedName = method.ContainingType.GetName(TypeNameOption.Flatten);
		}

		this.Parameters = method.Parameters.Select(_ => new ParameterModel(_)).ToImmutableArray();
		this.ReturnsVoid = method.ReturnsVoid;
	}

   internal string? ContainingTypeFlattenedName { get; }
	internal string? ContainingTypeFullyQualifiedName { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
	internal bool ReturnsVoid { get; }

	/// <summary>
	/// Gets the member identifier.
	/// </summary>
	internal uint MemberIdentifier { get; }

	/// <summary>
	/// Gets the <see cref="RequiresExplicitInterfaceImplementation"/> value that specifies if this result
	/// needs explicit implementation.
	/// </summary>
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }

	/// <summary>
	/// Gets the <see cref="RequiresOverride"/> value that specifies if this result
	/// needs an override.
	/// </summary>
	internal RequiresOverride RequiresOverride { get; }
}
