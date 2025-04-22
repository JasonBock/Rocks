using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Rocks.Analysis.Discovery;

[DebuggerDisplay("Value = {Value}")]
internal sealed class MockableMethodResult
{
   internal MockableMethodResult(IMethodSymbol value, ITypeSymbol mockType,
	   RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
	   RequiresOverride requiresOverride, RequiresHiding requiresHiding, uint memberIdentifier) =>
	   (this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.RequiresHiding, this.MemberIdentifier) =
		   (value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, requiresHiding, memberIdentifier);

   internal uint MemberIdentifier { get; }
   internal ITypeSymbol MockType { get; }
   internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresHiding RequiresHiding { get; }
	internal RequiresOverride RequiresOverride { get; }
   internal IMethodSymbol Value { get; }
}