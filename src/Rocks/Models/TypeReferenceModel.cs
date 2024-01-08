﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record TypeReferenceModel
{
	internal TypeReferenceModel(ITypeSymbol type, Compilation compilation)
	{
		this.FullyQualifiedName = type.GetFullyQualifiedName(compilation);
		this.FlattenedName = type.GetName(TypeNameOption.Flatten);
		this.NoGenericsName = type.GetName(TypeNameOption.NoGenerics);
		this.IncludeGenericsName = type.GetName(TypeNameOption.IncludeGenerics);

		this.NullableAnnotation = type.NullableAnnotation;

		this.AttributesDescription = type.GetAttributes().GetDescription(compilation, AttributeTargets.Class);
		this.Namespace = type.ContainingNamespace is not null ?
			!type.ContainingNamespace.IsGlobalNamespace ?
				type.ContainingNamespace.ToDisplayString() :
				string.Empty :
			string.Empty;

		this.Kind = type.Kind;
		this.TypeKind = type.TypeKind;

		this.IsRecord = type.IsRecord;
		this.IsReferenceType = type.IsReferenceType;
		this.IsPointer = type.IsPointer();
		this.IsEsoteric = type.IsEsoteric();
		this.IsRefLikeType = type.IsRefLikeType;

		var typeParameterTarget = type.IsPointer() ?
			type.Kind == SymbolKind.PointerType ?
				((IPointerTypeSymbol)type).PointedAtType :
				((IFunctionPointerTypeSymbol)type).BaseType :
			type;
		this.IsBasedOnTypeParameter = typeParameterTarget?.IsOpenGeneric() ?? false;

		if (this.IsEsoteric)
		{
			if (type.Kind == SymbolKind.PointerType)
			{
				(var count, var pointerType) = type.GetPointerInformation();
				this.PointerCount = count;
				this.PointerType = new TypeReferenceModel(pointerType, compilation);
			}
			else if (type is IFunctionPointerTypeSymbol functionPointerType &&
				functionPointerType.BaseType is not null)
			{
				this.PointerCount = 1;
				this.PointerType = new TypeReferenceModel(((IFunctionPointerTypeSymbol)type).BaseType!, compilation);
			}

			this.EsotericArgumentProjectedEvaluationDelegateName =
				$"ArgumentEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}{(this.PointerCount > 0 ? this.PointerCount : string.Empty)}";
			this.EsotericArgumentProjectedName =
				$"ArgumentFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}{(this.PointerCount > 0 ? this.PointerCount : string.Empty)}";
			this.EsotericArgumentConstructorProjectedName =
				$"ArgumentFor{(type.IsOpenGeneric() ? type.GetName(TypeNameOption.NoGenerics) : type.GetName(TypeNameOption.Flatten))}{(this.PointerCount > 0 ? this.PointerCount : string.Empty)}";

			// TODO: Need to remove properties here that I really don't need/use

			this.PointerArgProjectedEvaluationDelegateName =
				$"ArgumentEvaluationFor{type.GetName(TypeNameOption.Flatten)}";
			this.PointerArgProjectedName =
				$"ArgumentFor{type.GetName(TypeNameOption.Flatten)}";

			if (this.IsBasedOnTypeParameter && this.IsPointer)
			{
				this.PointerArgParameterType = ((IPointerTypeSymbol)type).PointedAtType.Name;
			}

			this.RefLikeArgProjectedEvaluationDelegateName =
				$"ArgEvaluationFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";
			this.RefLikeArgProjectedName =
				$"ArgFor{(type.IsOpenGeneric() ? type.GetName() : type.GetName(TypeNameOption.Flatten))}";
			this.RefLikeArgConstructorProjectedName =
				$"ArgFor{(type.IsOpenGeneric() ? type.GetName(TypeNameOption.NoGenerics) : type.GetName(TypeNameOption.Flatten))}";
		}
	}

	internal string AttributesDescription { get; }
	internal string? EsotericArgumentConstructorProjectedName { get; }
	internal string? EsotericArgumentProjectedEvaluationDelegateName { get; }
	internal string? EsotericArgumentProjectedName { get; }
	internal string FlattenedName { get; }
	internal string FullyQualifiedName { get; }
	internal string IncludeGenericsName { get; }
	internal bool IsBasedOnTypeParameter { get; }
	internal bool IsEsoteric { get; }
	internal bool IsPointer { get; }
	internal bool IsRecord { get; }
	internal bool IsReferenceType { get; }
	internal bool IsRefLikeType { get; }
	internal SymbolKind Kind { get; }
	// TODO: Why is Namespace not "string?".
	internal string Namespace { get; }
	internal string NoGenericsName { get; }
	internal NullableAnnotation NullableAnnotation { get; }
	internal string? PointerArgParameterType { get; }
	internal string? PointerArgProjectedEvaluationDelegateName { get; }
	internal string? PointerArgProjectedName { get; }
	internal int PointerCount { get; }
	internal TypeReferenceModel? PointerType { get; }
	internal string? RefLikeArgProjectedEvaluationDelegateName { get; }
	internal string? RefLikeArgProjectedName { get; }
	internal string? RefLikeArgConstructorProjectedName { get; }
	internal TypeKind TypeKind { get; }
}