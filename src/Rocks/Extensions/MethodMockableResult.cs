using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class MethodMockableResult
	{
		public MethodMockableResult(IMethodSymbol value, ITypeSymbol mockType, 
			RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, 
			RequiresOverride requiresOverride, uint memberIdentifier) =>
			(this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
				(value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

		public uint MemberIdentifier { get; }
		public ITypeSymbol MockType { get; }
		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public RequiresOverride RequiresOverride { get; }
		public IMethodSymbol Value { get; }
	}
}