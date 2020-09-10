using Microsoft.CodeAnalysis;
using Rocks.Descriptors;
using Rocks.Extensions;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks
{
	public interface IX
	{
		internal void Foo();
	}
	public sealed class MockInformation
	{
		public MockInformation(ITypeSymbol type, SemanticModel model, Compilation compilation)
		{
			(this.Type, this.Model, this.Compilation) = (type, model, compilation);
			this.Validate();
		}

		private void Validate()
		{
			var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
			var constructors = ImmutableArray.CreateBuilder<IMethodSymbol>();
			var events = ImmutableArray.CreateBuilder<IEventSymbol>();
			var properties = ImmutableArray.CreateBuilder<IPropertySymbol>();

			if(this.Type.IsSealed)
			{
				diagnostics.Add(CannotMockSealedTypeDescriptor.Create(this.Type));
			}

			// TODO: Could we figure out if TreatWarningsAsErrors is true?
			var attributes = this.Type.GetAttributes();
			var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

			if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
			{
				diagnostics.Add(CannotMockObsoleteTypeDescriptor.Create(this.Type));
			}

			this.Constructors = constructors.ToImmutable();
			this.Events = events.ToImmutable();
			this.Methods = this.Type.GetMockableMethods(this.Compilation);
			this.Properties = properties.ToImmutable();

			if(this.Events.Length == 0 && this.Methods.Length == 0 && this.Properties.Length == 0)
			{
				diagnostics.Add(TypeHasNoMockableMembersDescriptor.Create(this.Type));
			}

			this.Diagnostics = diagnostics.ToImmutable();
		}

		private Compilation Compilation { get; }
		public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
		public ImmutableArray<IEventSymbol> Events { get; private set; }
		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public ImmutableArray<MethodMockableResult> Methods { get; private set; }
		private SemanticModel Model { get; }
		public ImmutableArray<IPropertySymbol> Properties { get; private set; }
		public ITypeSymbol Type { get; }
	}
}