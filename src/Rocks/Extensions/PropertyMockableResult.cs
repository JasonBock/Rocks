using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal sealed class PropertyMockableResult
{
	internal PropertyMockableResult(IPropertySymbol value, ITypeSymbol mockType,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride, 
		uint memberIdentifier) =>
		(this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.Accessors, this.MemberIdentifier) =
			(value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, value.GetAccessors(), memberIdentifier);

	internal PropertyAccessor Accessors { get; }
	internal uint MemberIdentifier { get; }
	internal ITypeSymbol MockType { get; }
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal IPropertySymbol Value { get; }
}