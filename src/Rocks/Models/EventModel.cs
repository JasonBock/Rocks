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
	/// <param name="event">The <see cref="IEventSymbol"/> to obtain information from.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="event"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="event"/> requires an override.</param>
	/// <param name="mockType">The mock type.</param>
	/// <param name="compilation">The compilation.</param>
	internal EventModel(IEventSymbol @event, TypeReferenceModel mockType, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride)
	{
		(this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			 (mockType, requiresExplicitInterfaceImplementation, requiresOverride);

		this.Name = @event.Name;
		this.Type = new TypeReferenceModel(@event.Type, compilation);
		this.ContainingType = new TypeReferenceModel(@event.ContainingType, compilation);

		this.AttributesDescription = @event.GetAttributes().GetDescription(compilation);

		if (this.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			this.OverridingCodeValue = @event.GetOverridingCodeValue(compilation.Assembly);
		}

		var argsType = "global::System.EventArgs";

		if (@event.Type is INamedTypeSymbol eventNamedType &&
			eventNamedType.DelegateInvokeMethod is not null &&
			eventNamedType.DelegateInvokeMethod.Parameters is { Length: 2 })
		{
			argsType = eventNamedType.DelegateInvokeMethod.Parameters[1].Type.GetFullyQualifiedName();
		}

		this.ArgsType = argsType;
	}

   internal string ArgsType { get; }
   internal string? OverridingCodeValue { get; }
   internal string Name { get; }

   /// <summary>
   /// Gets the type of the event.
   /// </summary>
   internal TypeReferenceModel Type { get; }
   /// <summary>
   /// Gets the type that defines the event.
   /// </summary>
   internal TypeReferenceModel ContainingType { get; }
   internal string AttributesDescription { get; }

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
