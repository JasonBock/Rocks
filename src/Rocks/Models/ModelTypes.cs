using Microsoft.CodeAnalysis;
using Rocks.Builders;
using Rocks.Diagnostics;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal record MockedInformation
{
	internal static MockedInformation Create(ITypeSymbol typeToMock, SemanticModel model, BuildType buildType)
	{
		var compilation = model.Compilation;
		var treatWarningsAsErrors = compilation.Options.GeneralDiagnosticOption == ReportDiagnostic.Error;

		// Do all the work to see if this is a type to mock.
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

		if (typeToMock.SpecialType == SpecialType.System_Delegate ||
			typeToMock.SpecialType == SpecialType.System_MulticastDelegate ||
			typeToMock.SpecialType == SpecialType.System_Enum ||
			typeToMock.SpecialType == SpecialType.System_ValueType)
		{
			diagnostics.Add(CannotMockSpecialTypesDiagnostic.Create(typeToMock));
		}

		if (typeToMock.IsSealed)
		{
			diagnostics.Add(CannotMockSealedTypeDiagnostic.Create(typeToMock));
		}

		if (typeToMock is INamedTypeSymbol namedTypeToMock &&
			namedTypeToMock.HasOpenGenerics())
		{
			diagnostics.Add(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Create(typeToMock));
		}

		var attributes = typeToMock.GetAttributes();
		var obsoleteAttribute = model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

		if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
			(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors)))
		{
			diagnostics.Add(CannotMockObsoleteTypeDiagnostic.Create(typeToMock));
		}

		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
		var containingAssembly = compilation.Assembly;

		var constructors = typeToMock.GetMockableConstructors(containingAssembly);
		var methods = typeToMock.GetMockableMethods(compilation.Assembly, shims, compilation, ref memberIdentifier);
		var properties = typeToMock.GetMockableProperties(containingAssembly, shims, ref memberIdentifier);
		var events = typeToMock.GetMockableEvents(containingAssembly);

		foreach (var constructor in constructors)
		{
			if (constructor.Parameters.Any(_ => _.Type.GetAttributes().Any(
				_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
					(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors))))
			{
				diagnostics.Add(MemberUsesObsoleteTypeDiagnostic.Create(constructor));
			}
		}

		foreach (var method in methods.Results)
		{
			if (method.Value.Parameters.Any(_ => _.Type.GetAttributes().Any(
				_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
					(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors))) ||
				!method.Value.ReturnsVoid && method.Value.ReturnType.GetAttributes().Any(
					_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
						(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors)))
			{
				diagnostics.Add(MemberUsesObsoleteTypeDiagnostic.Create(method.Value));
			}
		}

		foreach (var property in properties.Results)
		{
			if (property.Value.Parameters.Any(_ => _.Type.GetAttributes().Any(
				_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
					(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors))) ||
				property.Value.Type.GetAttributes().Any(
					_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
						(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors)))
			{
				diagnostics.Add(MemberUsesObsoleteTypeDiagnostic.Create(property.Value));
			}
		}

		if (methods.HasInaccessibleAbstractMembers || properties.HasInaccessibleAbstractMembers ||
			events.HasInaccessibleAbstractMembers)
		{
			diagnostics.Add(TypeHasInaccessibleAbstractMembersDiagnostic.Create(typeToMock));
		}

		if (methods.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic) ||
			properties.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic))
		{
			diagnostics.Add(InterfaceHasStaticAbstractMembersDiagnostic.Create(typeToMock));
	  }

		if (buildType == BuildType.Create && methods.Results.Length == 0 && properties.Results.Length == 0)
		{
			diagnostics.Add(TypeHasNoMockableMembersDiagnostic.Create(typeToMock));
		}

		if (typeToMock.TypeKind == TypeKind.Class && constructors.Length == 0)
		{
			diagnostics.Add(TypeHasNoAccessibleConstructorsDiagnostic.Create(typeToMock));
		}

		return new(!diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error) ?
			new(ImmutableArray<MockedConstructor>.Empty, ImmutableArray<MockedMethod>.Empty,
				ImmutableArray<MockedProperty>.Empty, ImmutableArray<MockedIndexer>.Empty,
				ImmutableArray<MockedEvent>.Empty) : 
			null, 
			diagnostics.ToImmutable());
	}

	private MockedInformation(MockedType? type, EquatableArray<Diagnostic> diagnostics) =>
		(this.Type, this.Diagnostics) = (type, diagnostics);

	internal EquatableArray<Diagnostic> Diagnostics { get; }
	internal MockedType? Type { get; }
}

// Remember to sort all array so "equatable" will work
internal record MockedType(
	EquatableArray<MockedConstructor> Constructors, EquatableArray<MockedMethod> Methods,
	EquatableArray<MockedProperty> Properties, EquatableArray<MockedIndexer> Indexers,
	EquatableArray<MockedEvent> Events);

internal record MockedConstructor;

internal record MockedMethod;

internal record MockedProperty;

internal record MockedIndexer;

internal record MockedEvent;

internal record MockAttribute;

internal record MockedParameter;