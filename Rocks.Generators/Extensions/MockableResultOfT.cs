using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public class MockableResult<T>
		where T : ISymbol
	{
		public MockableResult(T value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation) =>
			(this.Value, this.RequiresExplicitInterfaceImplementation) = (value, requiresExplicitInterfaceImplementation);

		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public T Value { get; }
	}
}