using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

// TODO: Maybe this should be called "MockModel"
// as it's essentially a holder of all the "root" thing.
internal record TypeModel
{
	internal TypeModel(
		ITypeSymbol type, Compilation compilation,
		EquatableArray<ConstructorModel> constructors, EquatableArray<MethodModel> methods,
		EquatableArray<PropertyModel> properties, EquatableArray<EventModel> events)
	{
		(this.Constructors, this.Methods, this.Properties, this.Events) =
			(constructors, methods, properties, events);

		this.Type = new TypeReferenceModel(type, compilation);
		this.ConstructorProperties = type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly))
			.Select(_ => new ConstructorPropertyModel(_, compilation))
			.ToImmutableArray();
	}

	internal TypeReferenceModel Type { get; }
	internal EquatableArray<ConstructorModel> Constructors { get; }
	internal EquatableArray<EventModel> Events { get; }
	internal EquatableArray<ConstructorPropertyModel> ConstructorProperties { get; }
   internal EquatableArray<MethodModel> Methods { get; }
	internal EquatableArray<PropertyModel> Properties { get; }
}