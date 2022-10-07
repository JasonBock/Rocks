using Microsoft.CodeAnalysis;
using Rocks.Builders;
using Rocks.Configuration;
using Rocks.Diagnostics;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks;

internal sealed class MockInformation
{
	internal MockInformation(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		SemanticModel model, ConfigurationValues configurationValues, BuildType buildType)
	{
		(this.ContainingAssemblyOfInvocationSymbol, this.Model, this.ConfigurationValues) =
			(containingAssemblyOfInvocationSymbol, model, configurationValues);

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

		if (typeToMock is INamedTypeSymbol namedType &&
			MockInformation.HasOpenGenerics(namedType))
		{
			diagnostics.Add(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Create(typeToMock));
		}

		var attributes = typeToMock.GetAttributes();
		var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

		if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
			(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || this.ConfigurationValues.TreatWarningsAsErrors)))
		{
			diagnostics.Add(CannotMockObsoleteTypeDiagnostic.Create(typeToMock));
		}

		var memberIdentifier = 0u;
		var shims = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

		this.Constructors = typeToMock.GetMockableConstructors(this.ContainingAssemblyOfInvocationSymbol);
		this.Methods = typeToMock.GetMockableMethods(
			this.ContainingAssemblyOfInvocationSymbol, shims, this.Model.Compilation, ref memberIdentifier);
		this.Properties = typeToMock.GetMockableProperties(
			this.ContainingAssemblyOfInvocationSymbol, shims, ref memberIdentifier);
		this.Events = typeToMock.GetMockableEvents(
			this.ContainingAssemblyOfInvocationSymbol);

		if(this.Methods.HasInaccessibleAbstractMembers || this.Properties.HasInaccessibleAbstractMembers ||
			this.Events.HasInaccessibleAbstractMembers)
		{
			diagnostics.Add(TypeHasInaccessibleAbstractMembersDiagnostic.Create(typeToMock));
		}

		if (this.Methods.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic) ||
			this.Properties.Results.Any(_ => _.Value.IsAbstract && _.Value.IsStatic))
		{
			diagnostics.Add(InterfaceHasStaticAbstractMembersDiagnostic.Create(typeToMock));
		}

		if (buildType == BuildType.Create && this.Methods.Results.Length == 0 && this.Properties.Results.Length == 0)
		{
			diagnostics.Add(TypeHasNoMockableMembersDiagnostic.Create(typeToMock));
		}

		if (typeToMock.TypeKind == TypeKind.Class && this.Constructors.Length == 0)
		{
			diagnostics.Add(TypeHasNoAccessibleConstructorsDiagnostic.Create(typeToMock));
		}

		this.Shims = shims.ToImmutableArray();
		this.Diagnostics = diagnostics.ToImmutable();

		if (!this.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
		{
			this.TypeToMock = new(typeToMock);
		}
	}

	private static bool HasOpenGenerics(INamedTypeSymbol type)
	{
		if (type.TypeArguments.Length > 0)
		{
			for (var i = 0; i < type.TypeArguments.Length; i++)
			{
				if (type.TypeArguments[i].Equals(type.TypeParameters[i]))
				{
					return true;
				}
			}
		}

		return false;
	}

	internal ConfigurationValues ConfigurationValues { get; }
	internal ImmutableArray<IMethodSymbol> Constructors { get; private set; }
	internal IAssemblySymbol ContainingAssemblyOfInvocationSymbol { get; }
	internal MockableEvents Events { get; private set; }
	internal ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	internal MockableMethods Methods { get; private set; }
	internal SemanticModel Model { get; }
	internal MockableProperties Properties { get; private set; }
	internal ImmutableArray<ITypeSymbol> Shims { get; private set; }
	internal MockedType? TypeToMock { get; private set; }
}