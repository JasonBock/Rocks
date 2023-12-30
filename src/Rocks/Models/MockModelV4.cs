﻿using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using Rocks.Discovery;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record MockModelV4
{
	internal static MockModelV4 Create(SyntaxNode node, ITypeSymbol typeToMock,
		SemanticModel model, BuildType buildType, bool shouldResolveShims)
	{
		var compilation = model.Compilation;

		// Do all the work to see if this is a type to mock.
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

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

		if (typeToMock is INamedTypeSymbol namedTypeToMock &&
			namedTypeToMock.HasOpenGenerics())
		{
			diagnostics.Add(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Create(node, typeToMock));
		}

		var attributes = typeToMock.GetAttributes();

		var obsoleteAttribute = model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName)!;

		if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
			_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
		{
			diagnostics.Add(CannotMockObsoleteTypeDiagnostic.Create(node, typeToMock));
		}

		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
		var containingAssembly = compilation.Assembly;

		var constructors = new MockableConstructorDiscovery(typeToMock, containingAssembly, obsoleteAttribute).Constructors;
		var methods = new MockableMethodDiscovery(typeToMock, compilation.Assembly, shims, compilation, ref memberIdentifier).Methods;
		var properties = new MockablePropertyDiscovery(typeToMock, containingAssembly, shims, ref memberIdentifier).Properties;
		var events = new MockableEventDiscovery(typeToMock, containingAssembly).Events;

		if (constructors.Length > 1)
		{
			var uniqueConstructors = new List<IMethodSymbol>(constructors.Length);

			foreach (var constructor in constructors)
			{
				if (uniqueConstructors.Any(_ => _.Match(constructor) == MethodMatch.Exact))
				{
					// We found a rare case where there are duplicate constructors.
					diagnostics.Add(DuplicateConstructorsDiagnostic.Create(node, typeToMock));
					break;
				}
				else
				{
					uniqueConstructors.Add(constructor);
				}
			}
		}

		foreach (var constructor in constructors)
		{
			diagnostics.AddRange(constructor.GetObsoleteDiagnostics(node, obsoleteAttribute));
		}

		foreach (var method in methods.Results)
		{
			diagnostics.AddRange(method.Value.GetObsoleteDiagnostics(node, obsoleteAttribute));
		}

		foreach (var property in properties.Results)
		{
			diagnostics.AddRange(property.Value.GetObsoleteDiagnostics(node, obsoleteAttribute));
		}

		foreach (var @event in events.Results)
		{
			diagnostics.AddRange(@event.Value.GetObsoleteDiagnostics(node, obsoleteAttribute));
		}

		if (methods.HasInaccessibleAbstractMembers || properties.HasInaccessibleAbstractMembers ||
			events.HasInaccessibleAbstractMembers || typeToMock.HasInaccessibleAstractMembersWithInvalidIdentifiers(containingAssembly))
		{
			diagnostics.Add(TypeHasInaccessibleAbstractMembersDiagnostic.Create(node, typeToMock));
		}

		if (methods.HasMatchWithNonVirtual)
		{
			diagnostics.Add(TypeHasMatchWithNonVirtualDiagnostic.Create(node, typeToMock));
		}

		if (methods.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic) ||
			properties.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic))
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

		return new(!isMockable ? null : new TypeMockModel(node, typeToMock, compilation, model, constructors, methods, properties, events, shims, shouldResolveShims),
			typeToMock.GetFullyQualifiedName(compilation),
			diagnostics.ToImmutable(), buildType);
	}

	private MockModelV4(TypeMockModel? type, string typeFullyQualifiedName,
		EquatableArray<Diagnostic> diagnostics, BuildType buildType) =>
		(this.Type, this.FullyQualifiedName, this.Diagnostics, this.BuildType) =
			(type, typeFullyQualifiedName, diagnostics, buildType);

	internal BuildType BuildType { get; }
	internal EquatableArray<Diagnostic> Diagnostics { get; }
	internal string FullyQualifiedName { get; }
	internal TypeMockModel? Type { get; }
}