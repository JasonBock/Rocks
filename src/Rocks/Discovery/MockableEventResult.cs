using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Rocks.Discovery;

[DebuggerDisplay("Value = {Value}")]
internal sealed class MockableEventResult
{
   internal MockableEventResult(IEventSymbol value,
	   RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride) =>
	   (this.Value, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
		   (value, requiresExplicitInterfaceImplementation, requiresOverride);

   internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
   internal RequiresOverride RequiresOverride { get; }
   internal IEventSymbol Value { get; }
}