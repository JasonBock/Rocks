using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed class ModelContext
{
	private readonly Dictionary<ITypeSymbol, TypeReferenceModel> typeMap =
		new(SymbolEqualityComparer.IncludeNullability);

	internal ModelContext(SemanticModel semanticModel) =>
		this.SemanticModel = semanticModel;

	internal ITypeReferenceModel CreateTypeReference(ITypeSymbol typeSymbol)
	{
		if (this.typeMap.TryGetValue(typeSymbol, out var model))
		{
			return model;
		}
		else
		{
			var newModel = new TypeReferenceModel(typeSymbol, this);
			this.typeMap.Add(typeSymbol, newModel);
			return newModel;
		}
	}

	internal SemanticModel SemanticModel { get; }

	private sealed record TypeReferenceModel
		: ITypeReferenceModel
	{
		public TypeReferenceModel(ITypeSymbol type, ModelContext modelContext)
		{
			var compilation = modelContext.SemanticModel.Compilation;

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
					this.TypeArguments = [.. namedType.TypeArguments.Select(_ => modelContext.CreateTypeReference(_))];
					this.TypeParameters = [.. namedType.TypeParameters.Select(_ => modelContext.CreateTypeReference(_))];
				}
				else
				{
					this.TypeArguments = [];
					this.TypeParameters = [];
				}
			}
			else
			{
				this.TypeArguments = [];
				this.TypeParameters = [];
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
				this.PointedAt = new TypeReferenceModel(pointedAt, modelContext);
				this.PointerNames = string.Concat(Enumerable.Repeat("Pointer", (int)this.PointedAtCount));
			}
		}

		private static string BuildName(ITypeReferenceModel current, TypeArgumentsNamingContext parentNamingContext)
		{
			static string GetNameForGeneric(ITypeReferenceModel current, TypeArgumentsNamingContext parentNamingContext)
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

		public string BuildName(TypeArgumentsNamingContext parentNamingContext) =>
			TypeReferenceModel.BuildName(this, parentNamingContext);

		public override string ToString() => this.FullyQualifiedName;

		public bool Equals(ITypeReferenceModel other) =>
			this.Equals(other as TypeReferenceModel);

		public bool AllowsRefLikeType { get; }
		public string AttributesDescription { get; }
		public EquatableArray<Constraints> Constraints { get; }
		public string FlattenedName { get; }
		public string FullyQualifiedName { get; }
		public string FullyQualifiedNameNoGenerics { get; }
		public bool IsBasedOnTypeParameter { get; }
		public bool IsGenericType { get; }
		public bool IsOpenGeneric { get; }
		public bool IsPointer { get; }
		public bool IsRecord { get; }
		public bool IsReferenceType { get; }
		public bool IsRefLikeType { get; }
		public bool IsTupleType { get; }
		public string Name { get; }
		public string? Namespace { get; }
		public bool RequiresProjectedArgument { get; }
		public NullableAnnotation NullableAnnotation { get; }
		public ITypeReferenceModel? PointedAt { get; }
		public uint PointedAtCount { get; }
		public string? PointerNames { get; }
		public SpecialType SpecialType { get; }
		public EquatableArray<ITypeReferenceModel> TypeArguments { get; }
		public EquatableArray<ITypeReferenceModel> TypeParameters { get; }
		public TypeKind TypeKind { get; }
	}
}