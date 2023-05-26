using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

/// <summary>
/// Defines an event that can be raised in a mock.
/// </summary>
internal record EventModel
{
	/// <summary>
	/// Creates a new <see cref="EventModel"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IEventSymbol"/> to obtain information from.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="value"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="value"/> requires an override.</param>
	internal EventModel(IEventSymbol value,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride) =>
		(this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			(requiresExplicitInterfaceImplementation, requiresOverride);

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
}
