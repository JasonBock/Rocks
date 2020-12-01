using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class EventMockableResult
	{
		public EventMockableResult(IEventSymbol value,
			RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride) =>
			(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
				(value, requiresExplicitInterfaceImplementation, requiresOverride);

		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public RequiresOverride RequiresOverride { get; }
		public IEventSymbol Value { get; }
	}
}