using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

/// <summary>
/// Defines an event that can be mocked.
/// </summary>
internal sealed class EventMockableResult
{
	/// <summary>
	/// Creates a new <see cref="EventMockableResult"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IEventSymbol"/> to mock.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="value"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="value"/> requires an override.</param>
	internal EventMockableResult(IEventSymbol value,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride) =>
		(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			(value, requiresExplicitInterfaceImplementation, requiresOverride);

	/// <summary>
	/// Gets the <see cref="RequiresExplicitInterfaceImplementation"/> value that specifies if this result
	/// needs explicit implementation.
	/// </summary>
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	/// <summary>
	/// Gets the <see cref="RequiresOverride"/> value that specifies if this result
	/// needs an override.
	/// </summary>
	internal RequiresOverride RequiresOverride { get; }
	/// <summary>
	/// Gets the <see cref="IEventSymbol"/> value.
	/// </summary>
	internal IEventSymbol Value { get; }
}