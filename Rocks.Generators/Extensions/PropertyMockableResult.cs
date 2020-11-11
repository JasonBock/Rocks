using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class PropertyMockableResult
	{
		public PropertyMockableResult(IPropertySymbol value, uint memberIdentifier) =>
			(this.Value, this.MemberIdentifier) = (value, memberIdentifier);

		public uint MemberIdentifier { get; }
		public IPropertySymbol Value { get; }
	}
}