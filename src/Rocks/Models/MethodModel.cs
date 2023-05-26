using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

/// <summary>
/// Defines a method that can be mocked.
/// </summary>
internal record MethodModel
{
   /// <summary>
   /// Creates a new <see cref="MethodMockableResult"/> instance.
   /// </summary>
   /// <param name="method">The <see cref="IMethodSymbol"/> to obtain information from.</param>
   /// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="method"/> requires explicit implementation.</param>
   /// <param name="requiresOverride">Specifies if <paramref name="method"/> requires an override.</param>
   /// <param name="memberIdentifier">The member identifier.</param>
   internal MethodModel(IMethodSymbol method,
	   RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
	   RequiresOverride requiresOverride, uint memberIdentifier) => 
		(this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
		   (requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

   /// <summary>
   /// Gets the member identifier.
   /// </summary>
   internal uint MemberIdentifier { get; }

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
