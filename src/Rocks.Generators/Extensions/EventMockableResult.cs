using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class EventMockableResult
	{
		public EventMockableResult(IEventSymbol value,
			RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
			MustBeImplemented mustBeImplemented, RequiresOverride requiresOverride) =>
			(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MustBeImplemented) =
				(value, requiresExplicitInterfaceImplementation, requiresOverride, mustBeImplemented);

		public MustBeImplemented MustBeImplemented { get; }
		public RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
		public RequiresOverride RequiresOverride { get; }
		public IEventSymbol Value { get; }
	}
}