using Microsoft.CodeAnalysis;
using Rocks.Analysis.Discovery;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis.Models;

internal sealed record TypeMockModel
{
	internal TypeMockModel(
		SyntaxNode node, ITypeSymbol type, ITypeSymbol? expectationsInformationSource, ModelContext modelContext,
		ImmutableArray<IMethodSymbol> constructors, MockableMethods methods,
		MockableProperties properties, MockableEvents events,
		HashSet<ITypeSymbol> shims, TypeMockModelMemberCount memberCount, bool shouldResolveShims, BuildType buildType)
	{
		var semanticModel = modelContext.SemanticModel;
		var compilation = semanticModel.Compilation;

		this.Type = modelContext.CreateTypeReference(type);

		if (expectationsInformationSource is not null)
		{
			var expectationsInformationSourceType = modelContext.CreateTypeReference(expectationsInformationSource);
			(this.ExpectationsName, this.ExpectationsNameNoGenerics, this.ExpectationsFullyQualifiedName, this.ExpectationsNamespace) =
				compilation.GetExpectationsName(expectationsInformationSourceType, buildType, true);
			this.IsPartial = true;
			this.Accessibility = expectationsInformationSource.GetAccessibilityValue(compilation.Assembly);
			this.ExpectationsIsSealed = expectationsInformationSourceType.TypeKind == TypeKind.Class && expectationsInformationSource.IsSealed;
			this.AdornmentsFlattenedName = expectationsInformationSourceType.FlattenedName;
		}
		else
		{
			(this.ExpectationsName, this.ExpectationsNameNoGenerics, this.ExpectationsFullyQualifiedName, this.ExpectationsNamespace) =
				compilation.GetExpectationsName(this.Type, buildType, false);
			this.IsPartial = false;
			this.Accessibility = "internal";
			this.ExpectationsIsSealed = true;
			this.AdornmentsFlattenedName = this.Type.FlattenedName;
		}

		this.MemberCount = memberCount;

		// TODO: Remember to sort all array so "equatable" will work,
		// EXCEPT FOR parameter order (including generic parameters).
		// Those have to stay in the order they exist in the definition.
		this.Aliases = compilation.GetAliases();
		this.Constructors = [.. constructors.Select(_ => new ConstructorModel(_, modelContext))];
		this.Methods = [.. methods.Results.Select(_ =>
			new MethodModel(_.Value, this.Type, modelContext, _.RequiresExplicitInterfaceImplementation,
				_.RequiresOverride, _.RequiresHiding, _.MemberIdentifier))];
		this.Properties = [.. properties.Results.Select(_ =>
			new PropertyModel(_.Value, this.Type, modelContext,
				_.RequiresExplicitInterfaceImplementation, _.RequiresOverride,
				_.Accessors, _.MemberIdentifier))];
		this.Events = [.. events.Results.Select(_ =>
			new EventModel(_.Value, modelContext,
				_.RequiresExplicitInterfaceImplementation, _.RequiresOverride))];
		this.Shims = shouldResolveShims ?
			[.. shims.Select(_ => MockModel.Create(node, _, null, modelContext, BuildType.Create, false).Information!.Type)] :
			[];

		this.ConstructorProperties = [..type.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => (_.IsRequired || _.GetAccessors() == PropertyAccessor.Init || _.GetAccessors() == PropertyAccessor.GetAndInit) &&
				_.CanBeSeenByContainingAssembly(compilation.Assembly, compilation))
			.Select(_ => new ConstructorPropertyModel(_, modelContext))];

		this.ExpectationsPropertyName = this.GetExpectationsPropertyName();
		this.Projections = this.GetProjections();
	}

	private EquatableArray<ITypeReferenceModel> GetProjections()
	{
		var projections = new HashSet<ITypeReferenceModel>();

		foreach (var method in this.Methods)
		{
			foreach (var parameter in method.Parameters)
			{
				if (parameter.Type.RequiresProjectedArgument)
				{
					projections.Add(parameter.Type);
				}
			}

			if (method.ReturnType.RequiresProjectedArgument)
			{
				projections.Add(method.ReturnType);
			}
		}

		foreach (var property in this.Properties)
		{
			if (property.Type.RequiresProjectedArgument)
			{
				projections.Add(property.Type);
			}

			foreach (var parameter in property.Parameters)
			{
				if (parameter.Type.RequiresProjectedArgument)
				{
					projections.Add(parameter.Type);
				}
			}
		}

		foreach (var constructor in this.Constructors)
		{
			foreach (var parameter in constructor.Parameters)
			{
				if (parameter.Type.RequiresProjectedArgument)
				{
					projections.Add(parameter.Type);
				}
			}
		}

		return [.. projections];
	}

	private string GetExpectationsPropertyName()
	{
		const string ExpectationsName = "Expectations";

		var memberNames = new HashSet<string>(
			this.Methods.Select(_ => _.Name).Concat(this.Properties.Select(_ => _.Name)));

		var expectationsPropertyName = ExpectationsName;
		var index = 2;

		while (memberNames.Contains(expectationsPropertyName))
		{
			expectationsPropertyName = $"{ExpectationsName}{index++}";
		}

		return expectationsPropertyName;
	}

	internal string Accessibility { get; }
	internal string AdornmentsFlattenedName { get; }
	internal EquatableArray<string> Aliases { get; }
	internal EquatableArray<ConstructorPropertyModel> ConstructorProperties { get; }
	internal EquatableArray<ConstructorModel> Constructors { get; }
	internal string ExpectationsFullyQualifiedName { get; }
	internal bool ExpectationsIsSealed { get; }
	internal string ExpectationsName { get; }
	internal string ExpectationsNameNoGenerics { get; }
	internal string? ExpectationsNamespace { get; }
	internal string ExpectationsPropertyName { get; }
	internal EquatableArray<EventModel> Events { get; }
	internal bool IsPartial { get; }
	internal TypeMockModelMemberCount MemberCount { get; }
	internal EquatableArray<MethodModel> Methods { get; }
	internal EquatableArray<PropertyModel> Properties { get; }
	internal EquatableArray<ITypeReferenceModel> Projections { get; }
	internal EquatableArray<TypeMockModel> Shims { get; }
	internal ITypeReferenceModel Type { get; }
}