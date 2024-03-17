using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record TypeReferenceModel
{
	internal TypeReferenceModel(ITypeSymbol type, Compilation compilation)
	{
		this.FullyQualifiedName = type.GetFullyQualifiedName(compilation);
		this.FullyQualifiedNameNoGenerics = type.GetFullyQualifiedName(compilation, false);
		this.FlattenedName = type.GetName(TypeNameOption.Flatten);
		this.NullableAnnotation = type.NullableAnnotation;

		this.AttributesDescription = type.GetAttributes().GetDescription(compilation, AttributeTargets.Class);
		this.Namespace =
			type.ContainingNamespace is not null ?
				!type.ContainingNamespace.IsGlobalNamespace ?
					type.ContainingNamespace.ToDisplayString() :
					null :
				null;

		this.Kind = type.Kind;
		this.TypeKind = type.TypeKind;

		if (type is INamedTypeSymbol namedType)
		{
			this.IsOpenGeneric = namedType.IsOpenGeneric();
			this.Constraints = namedType.GetConstraints(compilation);
			this.TypeArguments = namedType.TypeArguments.Select(_ => new TypeReferenceModel(_, compilation)).ToImmutableArray();
			this.TypeParameters = namedType.TypeParameters.Select(_ => new TypeReferenceModel(_, compilation)).ToImmutableArray();
		}

		this.IsRecord = type.IsRecord;
		this.IsReferenceType = type.IsReferenceType;
		this.IsPointer = type.IsPointer();
		this.IsEsoteric = type.IsEsoteric();
		this.IsRefLikeType = type.IsRefLikeType;
		this.IsTupleType = type.IsTupleType;

		var typeParameterTarget = type.IsPointer() ?
			type.Kind == SymbolKind.PointerType ?
				((IPointerTypeSymbol)type).PointedAtType :
				((IFunctionPointerTypeSymbol)type).BaseType :
			type;
		this.IsBasedOnTypeParameter = typeParameterTarget?.IsOpenGeneric() ?? false;

		if (this.IsEsoteric)
		{
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
				$"ArgumentEvaluationFor{type.GetName(TypeNameOption.Flatten)}";
			this.RefLikeArgProjectedName =
				$"ArgumentFor{type.GetName(TypeNameOption.Flatten)}";
			this.RefLikeArgConstructorProjectedName =
				$"ArgumentFor{type.GetName(TypeNameOption.Flatten)}";
		}
	}

	// TODO: If this is a value tuple, then we need to format
	// with $"(...)"
	private static string BuildName(TypeReferenceModel current, TypeArgumentsNamingContext parentNamingContext) => 
		!current.IsOpenGeneric ?
			parentNamingContext[current.FullyQualifiedName] :
			current.IsTupleType ?
				$"({string.Join(", ", current.TypeArguments.Select(_ => TypeReferenceModel.BuildName(_, parentNamingContext)))})" :
				$"{current.FullyQualifiedNameNoGenerics}<{string.Join(", ", current.TypeArguments.Select(_ => TypeReferenceModel.BuildName(_, parentNamingContext)))}>";

	internal string BuildName(TypeArgumentsNamingContext parentNamingContext) =>
		TypeReferenceModel.BuildName(this, parentNamingContext);
	
	internal string BuildName(TypeReferenceModel parent) =>
		TypeReferenceModel.BuildName(this, new TypeArgumentsNamingContext(parent));

	public override string ToString() => this.FullyQualifiedName;

   internal string AttributesDescription { get; }
	internal EquatableArray<Constraints> Constraints { get; }
	internal string FlattenedName { get; }
	internal string FullyQualifiedName { get; }
	internal string FullyQualifiedNameNoGenerics { get; }
	internal bool IsBasedOnTypeParameter { get; }
	internal bool IsEsoteric { get; }
	internal bool IsOpenGeneric { get; }
	internal bool IsPointer { get; }
	internal bool IsRecord { get; }
	internal bool IsReferenceType { get; }
	internal bool IsRefLikeType { get; }
	internal bool IsTupleType { get; }
	internal SymbolKind Kind { get; }
	internal string? Namespace { get; }
	internal NullableAnnotation NullableAnnotation { get; }
	internal string? PointerArgParameterType { get; }
	internal string? PointerArgProjectedEvaluationDelegateName { get; }
	internal string? PointerArgProjectedName { get; }
	internal string? RefLikeArgProjectedEvaluationDelegateName { get; }
	internal string? RefLikeArgProjectedName { get; }
	internal string? RefLikeArgConstructorProjectedName { get; }
	internal EquatableArray<TypeReferenceModel> TypeArguments { get; }
	internal EquatableArray<TypeReferenceModel> TypeParameters { get; }
	internal TypeKind TypeKind { get; }
}