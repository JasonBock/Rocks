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
		this.Name = type.GetName(TypeNameOption.NoGenerics);
		this.NullableAnnotation = type.NullableAnnotation;
		this.RequiresProjectedArgument = type.RequiresProjectedArgument(compilation);

		this.AttributesDescription = type.GetAttributes().GetDescription(compilation, AttributeTargets.Class);
		this.Namespace =
			type.ContainingNamespace is not null ?
				!type.ContainingNamespace.IsGlobalNamespace ?
					type.ContainingNamespace.ToDisplayString() :
					null :
				null;

		this.AllowsRefLikeType = (type as ITypeParameterSymbol)?.AllowsRefLikeType ?? false;
		this.TypeKind = type.TypeKind;
		this.SpecialType = type.SpecialType;

		if (type is INamedTypeSymbol namedType)
		{
			this.IsOpenGeneric = namedType.IsOpenGeneric();
			this.Constraints = namedType.GetConstraints(compilation);
			this.IsGenericType = namedType.IsGenericType;

			if (this.IsGenericType && !(this.TypeKind == TypeKind.TypeParameter))
			{
				this.TypeArguments = [.. namedType.TypeArguments.Select(_ => new TypeReferenceModel(_, compilation))];
				this.TypeParameters = [.. namedType.TypeParameters.Select(_ => new TypeReferenceModel(_, compilation))];
			}
			else
			{
				this.TypeArguments = ImmutableArray<TypeReferenceModel>.Empty;
				this.TypeParameters = ImmutableArray<TypeReferenceModel>.Empty;
			}
		}
		else
		{
			this.TypeArguments = ImmutableArray<TypeReferenceModel>.Empty;
			this.TypeParameters = ImmutableArray<TypeReferenceModel>.Empty;
		}

		this.NullableAnnotation = type.NullableAnnotation;
		this.IsRecord = type.IsRecord;
		this.IsReferenceType = type.IsReferenceType;
		this.IsPointer = type.IsPointer();
		this.IsRefLikeType = type.IsRefLikeType;
		this.IsTupleType = type.IsTupleType;

		var typeParameterTarget = this.IsPointer ?
			type.Kind == SymbolKind.PointerType ?
				((IPointerTypeSymbol)type).PointedAtType :
				((IFunctionPointerTypeSymbol)type).BaseType :
			type;
		this.IsBasedOnTypeParameter = typeParameterTarget?.IsOpenGeneric() ?? false;

		if (type.TypeKind == TypeKind.Pointer)
		{
			var (pointedAtCount, pointedAt) = type.GetPointerInformation();
			this.PointedAtCount = pointedAtCount;
			this.PointedAt = new TypeReferenceModel(pointedAt, compilation);
			this.PointerNames = string.Concat(Enumerable.Repeat("Pointer", (int)this.PointedAtCount));
		}
	}

	private static string BuildName(TypeReferenceModel current, TypeArgumentsNamingContext parentNamingContext)
	{
		static string GetNameForGeneric(TypeReferenceModel current, TypeArgumentsNamingContext parentNamingContext)
		{
			// I don't like this heuristic, but I can't think of another way to handle the case
			// where I get a "generic" type that's just an nullable type argument like `TElement?`.
			if (!current.FullyQualifiedName.Contains('<'))
			{
				return current.FullyQualifiedName;
			}
			else
			{
				if (parentNamingContext.NameCount == 0)
				{
					return current.FullyQualifiedName;
				}
				else
				{
					// I don't like this either. I have the feeling there's a hidden bug with nested types and type parameter names
					// that will pop up in the future.
					return $"{current.FullyQualifiedNameNoGenerics}<{string.Join(", ", current.TypeArguments.Select(_ => TypeReferenceModel.BuildName(_, parentNamingContext)))}>{(current.NullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty)}";
				}
			}
		}

		if (!current.IsOpenGeneric)
		{
			// This could be a type parameter. If so, we should check 
			// its' nullable annotation, and if it's "Annotated" and it ends with "?",
			// then we should chop off the "?".
			if (current.TypeKind == TypeKind.TypeParameter)
			{
				if (current.NullableAnnotation == NullableAnnotation.Annotated && current.FullyQualifiedName.EndsWith("?"))
				{
					return $"{parentNamingContext[current.FullyQualifiedName.Substring(0, current.FullyQualifiedName.Length - 1)]}?";
				}
				else
				{
					return parentNamingContext[current.FullyQualifiedName];
				}
			}
			else
			{
				return parentNamingContext[current.FullyQualifiedName];
			}
		}
		else
		{
			return current.IsTupleType ?
				$"({string.Join(", ", current.TypeArguments.Select(_ => TypeReferenceModel.BuildName(_, parentNamingContext)))})" :
				GetNameForGeneric(current, parentNamingContext);
		}
	}

	internal string BuildName(TypeArgumentsNamingContext parentNamingContext) =>
		TypeReferenceModel.BuildName(this, parentNamingContext);

	public override string ToString() => this.FullyQualifiedName;

	internal bool AllowsRefLikeType { get; }
	internal string AttributesDescription { get; }
	internal EquatableArray<Constraints> Constraints { get; }
	internal string FlattenedName { get; }
	internal string FullyQualifiedName { get; }
	internal string FullyQualifiedNameNoGenerics { get; }
	internal bool IsBasedOnTypeParameter { get; }
	internal bool IsGenericType { get; }
	internal bool IsOpenGeneric { get; }
	internal bool IsPointer { get; }
	internal bool IsRecord { get; }
	internal bool IsReferenceType { get; }
	internal bool IsRefLikeType { get; }
	internal bool IsTupleType { get; }
	internal string Name { get; }
	internal string? Namespace { get; }
	internal bool RequiresProjectedArgument { get; }
	internal NullableAnnotation NullableAnnotation { get; }
	internal TypeReferenceModel? PointedAt { get; }
	internal uint PointedAtCount { get; }
	internal string? PointerNames { get; }
	internal SpecialType SpecialType { get; }
	internal EquatableArray<TypeReferenceModel> TypeArguments { get; }
	internal EquatableArray<TypeReferenceModel> TypeParameters { get; }
	internal TypeKind TypeKind { get; }
}