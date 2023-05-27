using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal record TypeReferenceModel
{
	internal TypeReferenceModel(ITypeSymbol type, Compilation compilation)
	{
		this.FullyQualifiedName = type.GetFullyQualifiedName();
		this.IsEsoteric = type.IsEsoteric();
		this.AttributesDescription = type.GetAttributes().GetDescription(compilation, AttributeTargets.ReturnValue);
		this.IsPointer = type.IsPointer();
		this.Namespace = type.ContainingNamespace?.IsGlobalNamespace ?? false ?
			null : type.ContainingNamespace!.ToDisplayString();
		this.FlattenedName = type.GetName(TypeNameOption.Flatten);
	}

	internal bool IsPointer { get; }
	internal string FullyQualifiedName { get; }
	internal bool IsEsoteric { get; }
   internal string AttributesDescription { get; }
	internal string FlattenedName { get; }
	internal bool IsRecord { get; }
	internal string? Namespace { get; }
}