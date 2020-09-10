using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class MethodMockableResult
	{
		public MethodMockableResult(IMethodSymbol value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			RequiresIsNewImplementation requiresIsNewImplementation, RequiresOverride requiresOverride) =>
			(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresNewImplementation, this.RequiresOverride) =
				(value, requiresExplicitInterfaceImplementation, requiresIsNewImplementation, requiresOverride);

		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public RequiresIsNewImplementation RequiresNewImplementation { get; }
		public RequiresOverride RequiresOverride { get; }
		public IMethodSymbol Value { get; }
	}
}