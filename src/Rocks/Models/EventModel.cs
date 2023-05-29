﻿using Microsoft.CodeAnalysis;
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
	/// <param name="mockType">The mock type.</param>
	/// <param name="compilation">The compilation.</param>
	internal EventModel(IEventSymbol value, TypeReferenceModel mockType, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride)
	{
		(this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			 (mockType, requiresExplicitInterfaceImplementation, requiresOverride);

		this.Name = value.Name;
		this.Type = new TypeReferenceModel(value.Type, compilation);
		this.ContainingType = new TypeReferenceModel(value.ContainingType, compilation);
	}

   internal string Name { get; }

   /// <summary>
   /// Gets the type of the event.
   /// </summary>
   internal TypeReferenceModel Type { get; }
   /// <summary>
   /// Gets the type that defines the event.
   /// </summary>
   internal TypeReferenceModel ContainingType { get; }
	/// <summary>
	/// Gets the mock type.
	/// </summary>
   internal TypeReferenceModel MockType { get; }
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
