using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

/// <summary>
/// Defines a property that can be mocked.
/// </summary>
internal record PropertyModel
{
	/// <summary>
	/// Creates a new <see cref="PropertyModel"/> instance.
	/// </summary>
	/// <param name="property">The <see cref="IPropertySymbol"/> to obtain information from.</param>
	/// <param name="compilation">The compilation.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="property"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="property"/> requires an override.</param>
	/// <param name="accessors">Specifies the accessors for this property.</param>
	/// <param name="memberIdentifier">The member identifier.</param>
	/// <param name="mockType">The mock type.</param>
	internal PropertyModel(IPropertySymbol property, TypeReferenceModel mockType, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride,
		PropertyAccessor accessors, uint memberIdentifier)
	{
		(this.Type, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.Accessors, this.MemberIdentifier) =
			(new TypeReferenceModel(property.Type, compilation), mockType, requiresExplicitInterfaceImplementation, requiresOverride, accessors, memberIdentifier);

		this.IsIndexer = property.IsIndexer;
		this.ReturnsByRef = property.ReturnsByRef;
		this.ReturnsByRefReadOnly = property.ReturnsByRefReadonly;
	}

	internal bool IsIndexer { get; }
   internal bool ReturnsByRef { get; }
	internal bool ReturnsByRefReadOnly { get; }
   internal TypeReferenceModel Type { get; }
	internal TypeReferenceModel MockType { get; }
	/// <summary>
	/// Gets the accessors.
	/// </summary>
	internal PropertyAccessor Accessors { get; }
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
