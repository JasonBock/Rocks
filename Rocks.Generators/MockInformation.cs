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
		public MockInformation(ITypeSymbol typeToMock, IAssemblySymbol containingAssemblyOfInvocationSymbol, 
			SemanticModel model, Compilation compilation)
		{
			(this.TypeToMock, this.ContainingAssemblyOfInvocationSymbol, this.Model, this.Compilation) = 
				(typeToMock, containingAssemblyOfInvocationSymbol, model, compilation);
			this.Validate();
		}

		private void Validate()
		{
			var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
			var events = ImmutableArray.CreateBuilder<IEventSymbol>();
			var properties = ImmutableArray.CreateBuilder<IPropertySymbol>();

			if(this.TypeToMock.IsSealed)
			{
				diagnostics.Add(CannotMockSealedTypeDescriptor.Create(this.TypeToMock));
			}

			// TODO: Could we figure out if TreatWarningsAsErrors is true?
			var attributes = this.TypeToMock.GetAttributes();
			var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

			if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
			{
				diagnostics.Add(CannotMockObsoleteTypeDescriptor.Create(this.TypeToMock));
			}

			this.Constructors = this.TypeToMock.GetMockableConstructors(this.ContainingAssemblyOfInvocationSymbol);
			this.Events = events.ToImmutable();
			this.Methods = this.TypeToMock.GetMockableMethods(this.ContainingAssemblyOfInvocationSymbol, this.Compilation);
			this.Properties = properties.ToImmutable();

			if(this.Events.Length == 0 && this.Methods.Length == 0 && this.Properties.Length == 0)
			{
				diagnostics.Add(TypeHasNoMockableMembersDescriptor.Create(this.TypeToMock));
			}

			if(this.TypeToMock.TypeKind == TypeKind.Class && this.Constructors.Length == 0)
			{
				diagnostics.Add(TypeHasNoAccessibleConstructorsDescriptor.Create(this.TypeToMock));
			}

			this.Diagnostics = diagnostics.ToImmutable();
		}

		private Compilation Compilation { get; }
		public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
		public IAssemblySymbol ContainingAssemblyOfInvocationSymbol { get; }
		public ImmutableArray<IEventSymbol> Events { get; private set; }
		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public ImmutableArray<MethodMockableResult> Methods { get; private set; }
		private SemanticModel Model { get; }
		public ImmutableArray<IPropertySymbol> Properties { get; private set; }
		public ITypeSymbol TypeToMock { get; }
	}
}