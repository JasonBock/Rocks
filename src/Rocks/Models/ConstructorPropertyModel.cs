using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record ConstructorPropertyModel
{
	internal ConstructorPropertyModel(IPropertySymbol value, ModelContext modelContext)
	{
		this.Type = modelContext.CreateTypeReference(value.Type);
		this.Name = value.Name;
		this.IsRequired = value.IsRequired;
		this.IsIndexer = value.IsIndexer;
		this.Accessors = value.GetAccessors();
		this.CanBeSeenByContainingAssembly = value.CanBeSeenByContainingAssembly(
			modelContext.SemanticModel.Compilation.Assembly, 
			modelContext.SemanticModel.Compilation);
		this.Parameters = [.. value.Parameters.Select(_ => new ParameterModel(_, modelContext))];
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
	internal ITypeReferenceModel Type { get; }
}