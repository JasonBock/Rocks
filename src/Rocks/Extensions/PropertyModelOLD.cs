using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

/// <summary>
/// Defines a property that can be mocked.
/// </summary>
internal sealed class PropertyModelOLD
{
	/// <summary>
	/// Creates a new <see cref="PropertyModelOLD"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IPropertySymbol"/> to mock.</param>
	/// <param name="mockType">The type to mock.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="value"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="value"/> requires an override.</param>
	/// <param name="accessors">Specifies the accessors for this property.</param>
	/// <param name="memberIdentifier">The member identifier.</param>
	internal PropertyModelOLD(IPropertySymbol value, ITypeSymbol mockType,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride,
		PropertyAccessor accessors, uint memberIdentifier) =>
		(this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.Accessors, this.MemberIdentifier) =
			(value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, accessors, memberIdentifier);

	/// <summary>
	/// Gets the accessors.
	/// </summary>
	internal PropertyAccessor Accessors { get; }
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
	/// Gets the <see cref="IPropertySymbol"/> value.
	/// </summary>
	internal IPropertySymbol Value { get; }
}