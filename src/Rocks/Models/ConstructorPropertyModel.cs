using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

/// <summary>
/// Defines a property that is either required and/or init
/// and can be set during the construction of a mock.
/// </summary>
internal record ConstructorPropertyModel
{
	/// <summary>
	/// Creates a new <see cref="ConstructorPropertyModel"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IPropertySymbol"/> to obtain information from.</param>
	/// <param name="compilation">The complation.</param>
	internal ConstructorPropertyModel(IPropertySymbol value, Compilation compilation) 
	{
		this.Name = value.Name;
		this.IsRequired = value.IsRequired;
		this.IsIndexer = value.IsIndexer;
		this.Accessors = value.GetAccessors();
		this.CanBeSeenByContainingAssembly = value.CanBeSeenByContainingAssembly(compilation.Assembly);
		this.Parameters = value.Parameters.Select(_ => new ParameterModel(_, compilation)).ToImmutableArray();
		this.NullableAnnotation = value.NullableAnnotation;
		this.IsReferenceType = value.Type.IsReferenceType;
	}

	internal EquatableArray<ParameterModel> Parameters { get; }
   internal NullableAnnotation NullableAnnotation { get; }
   internal bool IsReferenceType { get; }
   internal bool CanBeSeenByContainingAssembly { get; }
   internal string Name { get; }
   internal bool IsRequired { get; }
   internal bool IsIndexer { get; }
   internal PropertyAccessor Accessors { get; }
}
