using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record ConstructorPropertyModel
{
	internal ConstructorPropertyModel(IPropertySymbol value, Compilation compilation)
	{
		this.Type = new TypeReferenceModel(value.Type, compilation);
		this.Name = value.Name;
		this.IsRequired = value.IsRequired;
		this.IsIndexer = value.IsIndexer;
		this.Accessors = value.GetAccessors();
		this.CanBeSeenByContainingAssembly = value.CanBeSeenByContainingAssembly(compilation.Assembly, compilation);
		this.Parameters = value.Parameters.Select(_ => new ParameterModel(_, compilation)).ToImmutableArray();
		this.NullableAnnotation = value.NullableAnnotation;
		this.IsReferenceType = value.Type.IsReferenceType;
	}

	internal PropertyAccessor Accessors { get; }
	internal bool CanBeSeenByContainingAssembly { get; }
	internal bool IsIndexer { get; }
	internal bool IsReferenceType { get; }
	internal bool IsRequired { get; }
	internal string Name { get; }
	internal NullableAnnotation NullableAnnotation { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
	internal TypeReferenceModel Type { get; }
}