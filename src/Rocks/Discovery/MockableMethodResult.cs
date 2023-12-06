using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Rocks.Discovery;

[DebuggerDisplay("Value = {Value}")]
internal sealed class MockableMethodResult
{
   internal MockableMethodResult(IMethodSymbol value, ITypeSymbol mockType,
	   RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
	   RequiresOverride requiresOverride, uint memberIdentifier) =>
	   (this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
		   (value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

   internal uint MemberIdentifier { get; }
   internal ITypeSymbol MockType { get; }
   internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
   internal RequiresOverride RequiresOverride { get; }
   internal IMethodSymbol Value { get; }
}