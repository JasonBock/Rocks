using System.Reflection;

namespace Rocks.Extensions
{
	internal sealed class MethodMockableResult
		: MockableResult<MethodInfo>
	{
		internal MethodMockableResult(MethodInfo value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			RequiresIsNewImplementation requiresIsNewImplementation) 
			: base(value, requiresExplicitInterfaceImplementation)
		{
			this.RequiresNewImplementation = requiresIsNewImplementation;
		}

		internal RequiresIsNewImplementation RequiresNewImplementation { get; }
	}
}
