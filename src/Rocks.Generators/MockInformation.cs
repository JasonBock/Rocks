using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks
{
	internal sealed class MockInformation
	{
		public MockInformation(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, SemanticModel model)
		{
			(this.TypeToMock, this.ContainingAssemblyOfInvocationSymbol, this.Model) = 
				(typeToMock, containingAssemblyOfInvocationSymbol, model);
			this.Validate();
		}

		private static bool HasOpenGenerics(INamedTypeSymbol type)
		{ 
			if(type.TypeArguments.Length > 0)
			{
				for(var i = 0; i < type.TypeArguments.Length; i++)
				{
					if(type.TypeArguments[i].Equals(type.TypeParameters[i]))
					{
						return true;
					}
				}
			}

			return false;
		}

		private void Validate()
		{
			var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

			if(this.TypeToMock.IsSealed)
			{
				diagnostics.Add(CannotMockSealedTypeDescriptor.Create(this.TypeToMock));
			}

			if(this.TypeToMock is INamedTypeSymbol namedType &&
				MockInformation.HasOpenGenerics(namedType))
			{
				diagnostics.Add(CannotSpecifyTypeWithOpenGenericsDescriptor.Create(this.TypeToMock));
			}

			// TODO: Could we figure out if TreatWarningsAsErrors is true?
			// Maybe the same way we figure out the .editorconfig settings...
			var attributes = this.TypeToMock.GetAttributes();
			var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

			if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
			{
				diagnostics.Add(CannotMockObsoleteTypeDescriptor.Create(this.TypeToMock));
			}

			var memberIdentifier = 0u;

			this.Constructors = this.TypeToMock.GetMockableConstructors(this.ContainingAssemblyOfInvocationSymbol);
			this.Methods = this.TypeToMock.GetMockableMethods(
				this.ContainingAssemblyOfInvocationSymbol, ref memberIdentifier);
			this.Properties = this.TypeToMock.GetMockableProperties(
				this.ContainingAssemblyOfInvocationSymbol, ref memberIdentifier);
			this.Events = this.TypeToMock.GetMockableEvents(
				this.ContainingAssemblyOfInvocationSymbol);

			if (this.Methods.Length == 0 && this.Properties.Length == 0)
			{
				diagnostics.Add(TypeHasNoMockableMembersDescriptor.Create(this.TypeToMock));
			}

			if(this.TypeToMock.TypeKind == TypeKind.Class && this.Constructors.Length == 0)
			{
				diagnostics.Add(TypeHasNoAccessibleConstructorsDescriptor.Create(this.TypeToMock));
			}

			this.Diagnostics = diagnostics.ToImmutable();
		}

		public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
		public IAssemblySymbol ContainingAssemblyOfInvocationSymbol { get; }
		public ImmutableArray<EventMockableResult> Events { get; private set; }
		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public ImmutableArray<MethodMockableResult> Methods { get; private set; }
		private SemanticModel Model { get; }
		public ImmutableArray<PropertyMockableResult> Properties { get; private set; }
		public ITypeSymbol TypeToMock { get; }
	}
}