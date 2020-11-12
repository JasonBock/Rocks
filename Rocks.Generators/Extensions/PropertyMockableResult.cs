using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class PropertyMockableResult
	{
		public PropertyMockableResult(IPropertySymbol value, ITypeSymbol mockType, 
			PropertyAccessor accessors, uint memberIdentifier) =>
			(this.Value, this.MockType, this.Accessors, this.MemberIdentifier) = 
				(value, mockType, accessors, memberIdentifier);

		public PropertyAccessor Accessors { get; }
		public uint MemberIdentifier { get; }
		public ITypeSymbol MockType { get; }
		public IPropertySymbol Value { get; }
	}
}