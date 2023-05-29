using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal record TypeReferenceModel
{
	internal TypeReferenceModel(ITypeSymbol type, Compilation compilation)
	{
		this.FullyQualifiedName = type.GetFullyQualifiedName();
		this.FlattenedName = type.GetName(TypeNameOption.Flatten);
		this.NoGenericsName = type.GetName(TypeNameOption.NoGenerics);
		this.IncludeGenericsName = type.GetName(TypeNameOption.IncludeGenerics);

		this.DelegateInvokeMethod = type is INamedTypeSymbol namedType ? namedType.DelegateInvokeMethod : null;
		this.NullableAnnotation = type.NullableAnnotation;

		this.AttributesDescription = type.GetAttributes().GetDescription(compilation, AttributeTargets.ReturnValue);
		this.Namespace = type.ContainingNamespace?.IsGlobalNamespace ?? false ?
			null : type.ContainingNamespace!.ToDisplayString();
		this.Kind = type.Kind;
		this.TypeKind = type.TypeKind;

		this.IsReferenceType = type.IsReferenceType;
		this.IsPointer = type.IsPointer();
		this.IsEsoteric = type.IsEsoteric();
		this.IsRefLikeType = type.IsRefLikeType;

		if (this.IsEsoteric)
		{
			this.PointerArgProjectedEvaluationDelegateName = 
				$"ArgumentEvaluationFor{type.GetName(TypeNameOption.Flatten)}";
			this.PointerArgProjectedName = 
				$"ArgumentFor{type.GetName(TypeNameOption.Flatten)}";
			this.RefLikeArgProjectedEvaluationDelegateName = 
				$"ArgEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";
			this.RefLikeArgProjectedName =
				$"ArgFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";
			this.RefLikeArgConstructorProjectedName =
				$"ArgFor{(type.IsOpenGeneric() ? type.GetName(TypeNameOption.NoGenerics) : type.GetName(TypeNameOption.Flatten))}";
		}
	}

   internal string? PointerArgProjectedEvaluationDelegateName { get; }
   internal string? PointerArgProjectedName { get; }

	internal string? RefLikeArgProjectedEvaluationDelegateName { get; }
	internal string? RefLikeArgProjectedName { get; }
	internal string? RefLikeArgConstructorProjectedName { get; }

	internal bool IsPointer { get; }
	internal string FullyQualifiedName { get; }
	internal bool IsEsoteric { get; }
   internal bool IsRefLikeType { get; }
   internal string AttributesDescription { get; }
	internal string FlattenedName { get; }
   internal string NoGenericsName { get; }
   internal string IncludeGenericsName { get; }
   internal IMethodSymbol? DelegateInvokeMethod { get; }
   internal NullableAnnotation NullableAnnotation { get; }
   internal bool IsRecord { get; }
	internal string? Namespace { get; }
   internal SymbolKind Kind { get; }
   internal TypeKind TypeKind { get; }
   internal bool IsReferenceType { get; }
}