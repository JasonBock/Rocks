using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class MethodMockableResult
	{
		public MethodMockableResult(IMethodSymbol value, ITypeSymbol mockType, 
			RequiresOverride requiresOverride, uint memberIdentifier) =>
			(this.Value, this.MockType, this.RequiresOverride, this.MemberIdentifier) =
				(value, mockType, requiresOverride, memberIdentifier);

		public uint MemberIdentifier { get; }
		public ITypeSymbol MockType { get; }
		public RequiresOverride RequiresOverride { get; }
		public IMethodSymbol Value { get; }
	}
}