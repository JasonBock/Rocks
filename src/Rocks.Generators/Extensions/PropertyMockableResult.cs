using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class PropertyMockableResult
	{
		public PropertyMockableResult(IPropertySymbol value, ITypeSymbol mockType, 
			RequiresOverride requiresOverride, PropertyAccessor accessors, uint memberIdentifier) =>
			(this.Value, this.MockType, this.RequiresOverride, this.Accessors, this.MemberIdentifier) = 
				(value, mockType, requiresOverride, accessors, memberIdentifier);

		public PropertyAccessor Accessors { get; }
		public uint MemberIdentifier { get; }
		public ITypeSymbol MockType { get; }
		public RequiresOverride RequiresOverride { get; }
		public IPropertySymbol Value { get; }
	}
}