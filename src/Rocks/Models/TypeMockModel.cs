using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

/// <summary>
/// Defines a type that can be mocked.
/// </summary>
internal record TypeMockModel
{
	/// <summary>
	/// Creates a new <see cref="TypeMockModel" /> instance.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="compilation"></param>
	/// <param name="constructors"></param>
	/// <param name="methods"></param>
	/// <param name="properties"></param>
	/// <param name="events"></param>
	/// <param name="shims"></param>
	/// <remarks>
	/// A <see cref="TypeMockModel" /> should only be created from
	/// <see cref="MockModel.Create(ITypeSymbol, SemanticModel, Builders.BuildType)" />.
	/// Note that shims are also <see cref="TypeMockModel" />, and are 
	/// created within this constructor.
	/// </remarks>
	internal TypeMockModel(
		ITypeSymbol type, Compilation compilation,
		ImmutableArray<IMethodSymbol> constructors, MockableMethods methods,
		MockableProperties properties, MockableEvents events,
		HashSet<ITypeSymbol> shims)
	{
		this.MockType = new TypeReferenceModel(type, compilation);

		// TODO: Remember to sort all array so "equatable" will work,
		// EXCEPT FOR parameter order (including generic parameters).
		// Those have to stay in the order they exist in the definition.
		this.Constructors = constructors.Select(_ => 
			new ConstructorModel(_, this.MockType, compilation)).ToImmutableArray();
		this.Methods = methods.Results.Select(_ =>
			new MethodModel(_.Value, this.MockType, compilation, _.RequiresExplicitInterfaceImplementation,
				_.RequiresOverride, _.MemberIdentifier)).ToImmutableArray();
		this.Properties = properties.Results.Select(_ =>
			new PropertyModel(_.Value, this.MockType, compilation,
				_.RequiresExplicitInterfaceImplementation, _.RequiresOverride, 
				_.Accessors, _.MemberIdentifier)).ToImmutableArray();
		this.Events = events.Results.Select(_ =>
			new EventModel(_.Value, this.MockType, _.RequiresExplicitInterfaceImplementation,
				_.RequiresOverride)).ToImmutableArray();
		// TODO: These shims will have to be TypeModel instances.
		this.Shims = shims.Select(_ =>
			new TypeReferenceModel(_, compilation)).ToImmutableArray();

		this.ConstructorProperties = type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly))
			.Select(_ => new ConstructorPropertyModel(_, compilation))
			.ToImmutableArray();
	}

	internal TypeReferenceModel MockType { get; }
	internal EquatableArray<ConstructorModel> Constructors { get; }
	internal EquatableArray<EventModel> Events { get; }
   internal EquatableArray<TypeReferenceModel> Shims { get; }
   internal EquatableArray<ConstructorPropertyModel> ConstructorProperties { get; }
   internal EquatableArray<MethodModel> Methods { get; }
	internal EquatableArray<PropertyModel> Properties { get; }
}