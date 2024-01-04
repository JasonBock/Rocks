using Microsoft.CodeAnalysis;
using Rocks.Discovery;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record TypeMockModel
{
   internal TypeMockModel(
		SyntaxNode node, ITypeSymbol type, Compilation compilation, SemanticModel model,
	   ImmutableArray<IMethodSymbol> constructors, MockableMethods methods,
	   MockableProperties properties, MockableEvents events,
	   HashSet<ITypeSymbol> shims, bool shouldResolveShims)
   {
	  this.Type = new TypeReferenceModel(type, compilation);

	  // TODO: Remember to sort all array so "equatable" will work,
	  // EXCEPT FOR parameter order (including generic parameters).
	  // Those have to stay in the order they exist in the definition.
	  this.Aliases = compilation.GetAliases();
	  this.Constructors = constructors.Select(_ =>
		  new ConstructorModel(_, this.Type, compilation)).ToImmutableArray();
	  this.Methods = methods.Results.Select(_ =>
		  new MethodModel(_.Value, this.Type, compilation, _.RequiresExplicitInterfaceImplementation,
			  _.RequiresOverride, _.RequiresHiding, _.MemberIdentifier)).ToImmutableArray();
	  this.Properties = properties.Results.Select(_ =>
		  new PropertyModel(_.Value, this.Type, compilation,
			  _.RequiresExplicitInterfaceImplementation, _.RequiresOverride,
			  _.Accessors, _.MemberIdentifier)).ToImmutableArray();
	  this.Events = events.Results.Select(_ =>
		  new EventModel(_.Value, this.Type, compilation,
			  _.RequiresExplicitInterfaceImplementation, _.RequiresOverride)).ToImmutableArray();
	  this.Shims = shouldResolveShims ?
		  shims.Select(_ =>
			  MockModel.Create(node, _, model, BuildType.Create, false)!.Type!).ToImmutableArray() :
		  ImmutableArray<TypeMockModel>.Empty;

	  this.ConstructorProperties = type.GetMembers().OfType<IPropertySymbol>()
		  .Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
			  _.CanBeSeenByContainingAssembly(compilation.Assembly))
		  .Select(_ => new ConstructorPropertyModel(_, this.Type, compilation))
		  .ToImmutableArray();
   }

   internal EquatableArray<string> Aliases { get; }
   internal TypeReferenceModel Type { get; }
   internal EquatableArray<ConstructorModel> Constructors { get; }
   internal EquatableArray<EventModel> Events { get; }
   internal EquatableArray<TypeMockModel> Shims { get; }
   internal EquatableArray<ConstructorPropertyModel> ConstructorProperties { get; }
   internal EquatableArray<MethodModel> Methods { get; }
   internal EquatableArray<PropertyModel> Properties { get; }
}