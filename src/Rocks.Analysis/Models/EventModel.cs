using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Models;

internal sealed record EventModel
{
	internal EventModel(IEventSymbol @event, ModelContext modelContext,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride)
	{
		(this.RequiresExplicitInterfaceImplementation, this.RequiresOverride) =
			 (requiresExplicitInterfaceImplementation, requiresOverride);

		this.Name = @event.Name;
		this.Type = modelContext.CreateTypeReference(@event.Type);
		this.ContainingType = modelContext.CreateTypeReference(@event.ContainingType);

		this.AttributesDescription = @event.GetAttributes().GetDescription(modelContext.SemanticModel.Compilation);

		if (this.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			this.OverridingCodeValue = @event.GetAccessibilityValue(modelContext.SemanticModel.Compilation.Assembly);
		}

		var argsType = "global::System.EventArgs";

		if (@event.Type is INamedTypeSymbol eventNamedType &&
			eventNamedType.DelegateInvokeMethod?.Parameters is { Length: 2 })
		{
			argsType = eventNamedType.DelegateInvokeMethod.Parameters[1].Type.GetFullyQualifiedName(modelContext.SemanticModel.Compilation);
		}

		this.ArgsType = argsType;
	}

	internal string ArgsType { get; }
	internal string AttributesDescription { get; }
	internal ITypeReferenceModel ContainingType { get; }
	internal string Name { get; }
	internal string? OverridingCodeValue { get; }
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal ITypeReferenceModel Type { get; }
}
