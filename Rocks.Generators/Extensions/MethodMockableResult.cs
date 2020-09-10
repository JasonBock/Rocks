using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class MethodMockableResult
	{
		public MethodMockableResult(IMethodSymbol value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			RequiresOverride requiresOverride) =>
			(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
				(value, requiresExplicitInterfaceImplementation, requiresOverride);

		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public RequiresOverride RequiresOverride { get; }
		public IMethodSymbol Value { get; }
	}
}