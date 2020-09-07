using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Descriptors;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Rocks
{
	public sealed class MockInformation
	{
		public MockInformation(ITypeSymbol type, SemanticModel model)
		{
			this.Type = type;
			this.Model = model;
			this.Validate();
		}

		private void Validate()
		{
			var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
			var events = ImmutableArray.CreateBuilder<IEventSymbol>();
			var methods = ImmutableArray.CreateBuilder<IMethodSymbol>();
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

			this.Diagnostics = diagnostics.ToImmutable();
			this.Events = events.ToImmutable();
			this.Methods = methods.ToImmutable();
			this.Properties = properties.ToImmutable();
		}

		public ImmutableArray<IEventSymbol> Events { get; private set; }
		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public ImmutableArray<IMethodSymbol> Methods { get; private set; }
		private SemanticModel Model { get; }
		public ImmutableArray<IPropertySymbol> Properties { get; private set; }
		public ITypeSymbol Type { get; }
	}
}