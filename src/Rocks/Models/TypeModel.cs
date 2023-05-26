using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal record TypeModel
{
	internal TypeModel(
		ITypeSymbol type,
		EquatableArray<ConstructorModel> constructors, EquatableArray<MethodModel> methods,
		EquatableArray<PropertyModel> properties, EquatableArray<EventModel> events)
	{
		var @namespace = type.ContainingNamespace?.IsGlobalNamespace ?? false ?
			null : type.ContainingNamespace!.ToDisplayString();

		(this.Constructors, this.Methods, this.Properties, this.Events, this.Namespace, this.FlattenedName, this.FullyQualifiedName) =
			(constructors, methods, properties, events, @namespace, type.GetName(TypeNameOption.Flatten), type.GetFullyQualifiedName());
	}

	internal EquatableArray<ConstructorModel> Constructors { get; }
	internal EquatableArray<EventModel> Events { get; }
	internal string FlattenedName { get; }
	internal string FullyQualifiedName { get; }
	internal EquatableArray<MethodModel> Methods { get; }
	internal string? Namespace { get; }
	internal EquatableArray<PropertyModel> Properties { get; }
}