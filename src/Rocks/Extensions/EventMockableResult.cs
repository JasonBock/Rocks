using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Rocks.Extensions;

[DebuggerDisplay("Value = {Value}")]
internal sealed class EventMockableResult
{
	internal EventMockableResult(IEventSymbol value,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride) =>
		(this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			(value, requiresExplicitInterfaceImplementation, requiresOverride);

	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal IEventSymbol Value { get; }
}