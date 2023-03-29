using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

/// <summary>
/// Defines a method that can be mocked.
/// </summary>
internal sealed class MethodMockableResult
{
	/// <summary>
	/// Creates a new <see cref="MethodMockableResult"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IMethodSymbol"/> to mock.</param>
	/// <param name="mockType">The type to mock.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="value"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="value"/> requires an override.</param>
	/// <param name="memberIdentifier">The member identifier.</param>
	internal MethodMockableResult(IMethodSymbol value, ITypeSymbol mockType,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
		RequiresOverride requiresOverride, uint memberIdentifier) =>
		(this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
			(value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

	/// <summary>
	/// Gets the member identifier.
	/// </summary>
	internal uint MemberIdentifier { get; }
	/// <summary>
	/// Gets the mock type.
	/// </summary>
	internal ITypeSymbol MockType { get; }
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
	/// <summary>
	/// Gets the <see cref="IMethodSymbol"/> value.
	/// </summary>
	internal IMethodSymbol Value { get; }
}