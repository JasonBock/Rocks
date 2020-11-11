using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class PropertyMockableResult
	{
		public PropertyMockableResult(IPropertySymbol value, ITypeSymbol mockType, 
			RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			PropertyAccessor accessors, uint memberIdentifier) =>
			(this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.Accessors, this.MemberIdentifier) = 
				(value, mockType, requiresExplicitInterfaceImplementation, accessors, memberIdentifier);

		public PropertyAccessor Accessors { get; }
		public uint MemberIdentifier { get; }
		public ITypeSymbol MockType { get; }
		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public IPropertySymbol Value { get; }
	}
}