using Microsoft.CodeAnalysis;
using Rocks.Builders;
using Rocks.Configuration;
using Rocks.Diagnostics;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks;

internal sealed class MockInformation
{
	public MockInformation(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		SemanticModel model, ConfigurationValues configurationValues, BuildType buildType)
	{
		(this.TypeToMock, this.ContainingAssemblyOfInvocationSymbol, this.Model, this.ConfigurationValues) =
			(typeToMock, containingAssemblyOfInvocationSymbol, model, configurationValues);
		this.Validate(buildType);
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

	private void Validate(BuildType buildType)
	{
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

		if (this.TypeToMock.IsSealed)
		{
			diagnostics.Add(CannotMockSealedTypeDiagnostic.Create(this.TypeToMock));
		}

		if (this.TypeToMock is INamedTypeSymbol namedType &&
			MockInformation.HasOpenGenerics(namedType))
		{
			diagnostics.Add(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Create(this.TypeToMock));
		}

		var attributes = this.TypeToMock.GetAttributes();
		var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

		if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
			(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || this.ConfigurationValues.TreatWarningsAsErrors)))
		{
			diagnostics.Add(CannotMockObsoleteTypeDiagnostic.Create(this.TypeToMock));
		}

		var memberIdentifier = 0u;

		this.Constructors = this.TypeToMock.GetMockableConstructors(this.ContainingAssemblyOfInvocationSymbol);
		this.Methods = this.TypeToMock.GetMockableMethods(
			this.ContainingAssemblyOfInvocationSymbol, ref memberIdentifier);
		this.Properties = this.TypeToMock.GetMockableProperties(
			this.ContainingAssemblyOfInvocationSymbol, ref memberIdentifier);
		this.Events = this.TypeToMock.GetMockableEvents(
			this.ContainingAssemblyOfInvocationSymbol);

		if (buildType == BuildType.Create && this.Methods.Length == 0 && this.Properties.Length == 0)
		{
			diagnostics.Add(TypeHasNoMockableMembersDiagnostic.Create(this.TypeToMock));
		}

		if (this.TypeToMock.TypeKind == TypeKind.Class && this.Constructors.Length == 0)
		{
			diagnostics.Add(TypeHasNoAccessibleConstructorsDiagnostic.Create(this.TypeToMock));
		}

		this.Diagnostics = diagnostics.ToImmutable();
	}

	public ConfigurationValues ConfigurationValues { get; }
	public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
	public IAssemblySymbol ContainingAssemblyOfInvocationSymbol { get; }
	public ImmutableArray<EventMockableResult> Events { get; private set; }
	public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
	public ImmutableArray<MethodMockableResult> Methods { get; private set; }
	public SemanticModel Model { get; }
	public ImmutableArray<PropertyMockableResult> Properties { get; private set; }
	public ITypeSymbol TypeToMock { get; }
}