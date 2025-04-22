using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Create;

namespace Rocks.Analysis.Models;

internal interface ITypeReferenceModel
	: IEquatable<ITypeReferenceModel>
{
	string BuildName(TypeArgumentsNamingContext parentNamingContext);

	bool AllowsRefLikeType { get; }
	string AttributesDescription { get; }
	EquatableArray<Constraints> Constraints { get; }
	string FlattenedName { get; }
	string FullyQualifiedName { get; }
	string FullyQualifiedNameNoGenerics { get; }
	bool IsBasedOnTypeParameter { get; }
	bool IsGenericType { get; }
	bool IsOpenGeneric { get; }
	bool IsPointer { get; }
	bool IsRecord { get; }
	bool IsReferenceType { get; }
	bool IsRefLikeType { get; }
	bool IsTupleType { get; }
	string Name { get; }
	string? Namespace { get; }
	bool RequiresProjectedArgument { get; }
	NullableAnnotation NullableAnnotation { get; }
	ITypeReferenceModel? PointedAt { get; }
	uint PointedAtCount { get; }
	string? PointerNames { get; }
	SpecialType SpecialType { get; }
	EquatableArray<ITypeReferenceModel> TypeArguments { get; }
	EquatableArray<ITypeReferenceModel> TypeParameters { get; }
	TypeKind TypeKind { get; }
}