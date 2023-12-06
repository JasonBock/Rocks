using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Diagnostics;

namespace Rocks.Discovery;

[DebuggerDisplay("Value = {Value}")]
internal sealed class MockablePropertyResult
{
   internal MockablePropertyResult(IPropertySymbol value, ITypeSymbol mockType,
	   RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride,
	   uint memberIdentifier) =>
	   (this.Value, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.Accessors, this.MemberIdentifier) =
		   (value, mockType, requiresExplicitInterfaceImplementation, requiresOverride, value.GetAccessors(), memberIdentifier);

   internal PropertyAccessor Accessors { get; }
   internal uint MemberIdentifier { get; }
   internal ITypeSymbol MockType { get; }
   internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
   internal RequiresOverride RequiresOverride { get; }
   internal IPropertySymbol Value { get; }
}