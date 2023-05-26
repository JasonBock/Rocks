using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal record TypeModel
{
	internal TypeModel(
		ITypeSymbol type, Compilation compilation,
		EquatableArray<ConstructorModel> constructors, EquatableArray<MethodModel> methods,
		EquatableArray<PropertyModel> properties, EquatableArray<EventModel> events)
	{
		(this.Constructors, this.Methods, this.Properties, this.Events) =
			(constructors, methods, properties, events);

		this.Namespace = type.ContainingNamespace?.IsGlobalNamespace ?? false ?
			null : type.ContainingNamespace!.ToDisplayString();
		this.FlattenedName = type.GetName(TypeNameOption.Flatten);
		this.FullyQualifiedName = type.GetFullyQualifiedName();
		this.IsRecord = type.IsRecord;
		this.ConstructorProperties = type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly))
			.Select(_ => new ConstructorPropertyModel(_))
			.ToImmutableArray();
	}

	internal EquatableArray<ConstructorModel> Constructors { get; }
	internal EquatableArray<EventModel> Events { get; }
	internal string FlattenedName { get; }
	internal string FullyQualifiedName { get; }
	internal bool IsRecord { get; }
   internal EquatableArray<ConstructorPropertyModel> ConstructorProperties { get; }
   internal EquatableArray<MethodModel> Methods { get; }
	internal string? Namespace { get; }
	internal EquatableArray<PropertyModel> Properties { get; }
}