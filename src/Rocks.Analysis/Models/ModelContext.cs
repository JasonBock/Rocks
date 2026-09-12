using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Create;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis.Models;

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

			if (type is IArrayTypeSymbol arraySymbol)
			{
				this.ArrayElementType = modelContext.CreateTypeReference(arraySymbol.ElementType);
				this.ArrayRank = arraySymbol.Rank;
			}

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

			if (this.ArrayElementType is not null)
			{
				// ITypeSymbol.Name is empty for arrays, which produced empty
				// slots (e.g. (String,,CancellationToken)). Arrays render as
				// Element[] with rank commas.
				this.XmlCommentName = $"{this.ArrayElementType.XmlCommentName}[{new string(',', this.ArrayRank - 1)}]";
			}
			else if (this.IsGenericType && this.TypeArguments.Length > 0)
			{
				// ITypeSymbol.Name drops generic arguments, leaving the bare
				// open-generic name (e.g. Use(List)), which strict consumers
				// reject with CS1574/CS1580.
				this.XmlCommentName = GetGenericXmlCommentName(this);
			}
			else
			{
				this.XmlCommentName = this.Name;
			}
		}

		private static string GetGenericXmlCommentName(ITypeReferenceModel current)
		{
			// Fully qualified outer name with recursively rendered arguments:
			// Name{args} when no argument transitively contains a constructed
			// generic (braces keep the XML valid, matching the existing {T}
			// method-arity convention), otherwise escaped angle brackets
			// Name&lt;args&gt; (Roslyn rejects nested braces with CS1584).
			// Bare names do not resolve in generated files (only using
			// Rocks.Extensions); method type parameters stay bare (in scope).
			var outerName = GetQualifiedXmlCommentName(current);
			var useAngles = current.TypeArguments.Any(ContainsConstructedGeneric);
			var arguments = string.Join(",", current.TypeArguments.Select(_ => GetGenericArgumentXmlCommentName(_, useAngles)));
			return useAngles ? $"{outerName}&lt;{arguments}&gt;" : $"{outerName}{{{arguments}}}";
		}

		private static string GetGenericArgumentXmlCommentName(ITypeReferenceModel type, bool useAngles)
		{
			if (type.ArrayElementType is not null)
			{
				return $"{GetGenericArgumentXmlCommentName(type.ArrayElementType, useAngles)}[{new string(',', type.ArrayRank - 1)}]";
			}
			if (type.IsGenericType && type.TypeArguments.Length > 0)
			{
				var outerName = GetQualifiedXmlCommentName(type);
				var arguments = string.Join(",", type.TypeArguments.Select(_ => GetGenericArgumentXmlCommentName(_, true)));
				return useAngles ? $"{outerName}&lt;{arguments}&gt;" : $"{outerName}{{{arguments}}}";
			}
			return GetQualifiedXmlCommentName(type);
		}

		private static bool ContainsConstructedGeneric(ITypeReferenceModel type) =>
			(type.IsGenericType && type.TypeArguments.Length > 0) ||
			(type.ArrayElementType is not null && ContainsConstructedGeneric(type.ArrayElementType));

		private static readonly Dictionary<SpecialType, string> SpecialTypeXmlCommentNames = new()
		{
			[SpecialType.System_Object] = "global::System.Object",
			[SpecialType.System_Boolean] = "global::System.Boolean",
			[SpecialType.System_Char] = "global::System.Char",
			[SpecialType.System_SByte] = "global::System.SByte",
			[SpecialType.System_Byte] = "global::System.Byte",
			[SpecialType.System_Int16] = "global::System.Int16",
			[SpecialType.System_UInt16] = "global::System.UInt16",
			[SpecialType.System_Int32] = "global::System.Int32",
			[SpecialType.System_UInt32] = "global::System.UInt32",
			[SpecialType.System_Int64] = "global::System.Int64",
			[SpecialType.System_UInt64] = "global::System.UInt64",
			[SpecialType.System_Decimal] = "global::System.Decimal",
			[SpecialType.System_Single] = "global::System.Single",
			[SpecialType.System_Double] = "global::System.Double",
			[SpecialType.System_String] = "global::System.String",
			[SpecialType.System_IntPtr] = "global::System.IntPtr",
			[SpecialType.System_UIntPtr] = "global::System.UIntPtr",
		};

		private static string GetQualifiedXmlCommentName(ITypeReferenceModel type)
		{
			if (type.TypeKind == TypeKind.TypeParameter)
			{
				return type.Name.TrimEnd('?');
			}
			// FullyQualifiedNameNoGenerics renders Nullable<T> as T? shorthand
			// (constructed Nullable<T> does not reliably report its SpecialType)
			// and special types as C# keywords (int, string): neither is valid
			// in crefs, so both map to fully qualified names.
			if (type.SpecialType == SpecialType.System_Nullable_T || type.FullyQualifiedNameNoGenerics.EndsWith("?", StringComparison.Ordinal))
			{
				return "global::System.Nullable";
			}

			return SpecialTypeXmlCommentNames.TryGetValue(type.SpecialType, out var specialTypeName) ? specialTypeName : StripGenericArity(type.FullyQualifiedNameNoGenerics);
		}

		private static string StripGenericArity(string name)
		{
			var result = name;
			for (var index = result.IndexOf('`'); index >= 0; index = result.IndexOf('`', index))
			{
				var end = index + 1;
				while (end < result.Length && char.IsDigit(result[end]))
				{
					end++;
				}
				result = result.Remove(index, end - index);
			}
			return result;
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
						return $"{current.FullyQualifiedNameNoGenerics}<{string.Join(", ", current.TypeArguments.Select(_ => BuildName(_, parentNamingContext)))}>{(current.NullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty)}";
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
					$"({string.Join(", ", current.TypeArguments.Select(_ => BuildName(_, parentNamingContext)))})" :
					GetNameForGeneric(current, parentNamingContext);
			}
		}

		public string BuildName(TypeArgumentsNamingContext parentNamingContext) =>
			BuildName(this, parentNamingContext);

		public override string ToString() => this.FullyQualifiedName;

		public bool Equals(ITypeReferenceModel other) =>
			this.Equals(other as TypeReferenceModel);

		public bool AllowsRefLikeType { get; }
		public ITypeReferenceModel? ArrayElementType { get; }
		public int ArrayRank { get; }
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
		public string XmlCommentName { get; }
	}
}