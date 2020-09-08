using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class MethodMockableResult
		: MockableResult<IMethodSymbol>
	{
		public MethodMockableResult(IMethodSymbol value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			RequiresIsNewImplementation requiresIsNewImplementation) 
			: base(value, requiresExplicitInterfaceImplementation) =>
			this.RequiresNewImplementation = requiresIsNewImplementation;

		public RequiresIsNewImplementation RequiresNewImplementation { get; }
	}
}