using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;
using Rocks.Analysis.Discovery;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Models;

internal sealed record MockModel
{
	internal static MockModel Create(SyntaxNode node, ITypeSymbol typeToMock, ITypeSymbol? expectationsInformationSource,
		ModelContext modelContext, BuildType buildType, bool shouldResolveShims)
	{
		var compilation = modelContext.SemanticModel.Compilation;

		// Do all the work to see if this is a type to mock.
		var diagnostics = new List<Diagnostic>();

		if (typeToMock.SpecialType == SpecialType.System_Delegate ||
			typeToMock.SpecialType == SpecialType.System_MulticastDelegate ||
			typeToMock.SpecialType == SpecialType.System_Enum ||
			typeToMock.SpecialType == SpecialType.System_ValueType)
		{
			diagnostics.Add(CannotMockSpecialTypesDiagnostic.Create(node, typeToMock));
		}

		if (typeToMock.IsSealed)
		{
			diagnostics.Add(CannotMockSealedTypeDiagnostic.Create(node, typeToMock));
		}

		if (typeToMock is INamedTypeSymbol namedTypeToMock && namedTypeToMock.IsGenericType && !namedTypeToMock.IsOpenGeneric())
		{
			diagnostics.Add(TypeIsClosedGenericDiagnostic.Create(node, typeToMock));
		}

		var attributes = typeToMock.GetAttributes();

		var obsoleteAttribute = compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName)!;

		if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
			_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
		{
			diagnostics.Add(CannotMockObsoleteTypeDiagnostic.Create(node, typeToMock));
		}

		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
		var containingAssembly = compilation.Assembly;

		var constructors = new MockableConstructorDiscovery(typeToMock, containingAssembly, obsoleteAttribute, compilation).Constructors;
		var methods = new MockableMethodDiscovery(typeToMock, compilation.Assembly, shims, compilation, ref memberIdentifier).Methods;
		var methodMemberCount = methods.Results.Length;

		var properties = new MockablePropertyDiscovery(typeToMock, containingAssembly, shims, ref memberIdentifier, compilation).Properties;
		var propertyMemberCount = (int)memberIdentifier - methodMemberCount;
		var events = new MockableEventDiscovery(typeToMock, containingAssembly, compilation).Events;

		foreach (var constructor in constructors)
		{
			var diagnostic = constructor.GetObsoleteDiagnostic(node, obsoleteAttribute);

			if (diagnostic is not null)
			{
				diagnostics.Add(diagnostic);
			}
		}

		foreach (var method in methods.Results)
		{
			var diagnostic = method.Value.GetObsoleteDiagnostic(node, obsoleteAttribute);

			if (diagnostic is not null)
			{
				diagnostics.Add(diagnostic);
			}
		}

		foreach (var property in properties.Results)
		{
			var diagnostic = property.Value.GetObsoleteDiagnostic(node, obsoleteAttribute);

			if (diagnostic is not null)
			{
				diagnostics.Add(diagnostic);
			}
		}

		foreach (var @event in events.Results)
		{
			var diagnostic = @event.Value.GetObsoleteDiagnostic(node, obsoleteAttribute);

			if (diagnostic is not null)
			{
				diagnostics.Add(diagnostic);
			}
		}

		if (methods.HasInaccessibleAbstractMembers || properties.HasInaccessibleAbstractMembers ||
			events.HasInaccessibleAbstractMembers || typeToMock.HasInaccessibleAstractMembersWithInvalidIdentifiers(containingAssembly, compilation))
		{
			diagnostics.Add(TypeHasInaccessibleAbstractMembersDiagnostic.Create(node, typeToMock));
		}

		if (methods.HasStaticAbstractMembers || properties.HasStaticAbstractMembers)
		{
			diagnostics.Add(InterfaceHasStaticAbstractMembersDiagnostic.Create(node, typeToMock));
		}

		if (buildType == BuildType.Create && methods.Results.Length == 0 && properties.Results.Length == 0)
		{
			diagnostics.Add(TypeHasNoMockableMembersDiagnostic.Create(node, typeToMock));
		}

		if (typeToMock.TypeKind == TypeKind.Class && constructors.Length == 0)
		{
			diagnostics.Add(TypeHasNoAccessibleConstructorsDiagnostic.Create(node, typeToMock));
		}

		var isMockable = !diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error);

		return new(
			!isMockable ? null :
				new MockModelInformation(
					new TypeMockModel(node, typeToMock, expectationsInformationSource, modelContext,
						constructors, methods, properties, events,
						shims, new TypeMockModelMemberCount(methodMemberCount, propertyMemberCount), shouldResolveShims, buildType),
					buildType),
			[.. diagnostics]);
	}

	private MockModel(MockModelInformation? information, EquatableArray<Diagnostic> diagnostics) =>
		(this.Information, this.Diagnostics) =
			(information, diagnostics);

	internal MockModelInformation? Information { get; }
	internal EquatableArray<Diagnostic> Diagnostics { get; }
}