using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record EventModel
{
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
		 this.OverridingCodeValue = @event.GetAccessibilityValue(compilation.Assembly);
	  }

	  var argsType = "global::System.EventArgs";

	  if (@event.Type is INamedTypeSymbol eventNamedType &&
		  eventNamedType.DelegateInvokeMethod?.Parameters is { Length: 2 })
	  {
		 argsType = eventNamedType.DelegateInvokeMethod.Parameters[1].Type.GetFullyQualifiedName(compilation);
	  }

	  this.ArgsType = argsType;
   }

   internal string ArgsType { get; }
   internal string AttributesDescription { get; }
   internal TypeReferenceModel ContainingType { get; }
   internal TypeReferenceModel MockType { get; }
   internal string Name { get; }
   internal string? OverridingCodeValue { get; }
   internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
   internal RequiresOverride RequiresOverride { get; }
   internal TypeReferenceModel Type { get; }
}
