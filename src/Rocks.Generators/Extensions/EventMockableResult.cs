using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	public sealed class EventMockableResult
	{
		public EventMockableResult(IEventSymbol value, MustBeImplemented mustBeImplemented, RequiresOverride requiresOverride) =>
			(this.Value, this.RequiresOverride, this.MustBeImplemented) =
				(value, requiresOverride, mustBeImplemented);

		public MustBeImplemented MustBeImplemented { get; }
		public RequiresOverride RequiresOverride { get; }
		public IEventSymbol Value { get; }
	}
}