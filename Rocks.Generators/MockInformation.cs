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
				diagnostics.Add(Diagnostic.Create(new DiagnosticDescriptor(
					CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title,
					string.Format(CultureInfo.CurrentCulture, CannotMockSealedTypeDescriptor.Message, this.Type.Name),
					DescriptorConstants.Usage, DiagnosticSeverity.Info, true,
					helpLinkUri: HelpUrlBuilder.Build(
						CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title)), this.Type.Locations[0]));
			}

			// TODO: Could we figure out if TreatWarningsAsErrors is true?
			var attributes = this.Type.GetAttributes();
			var obsoleteAttribute = this.Model.Compilation.GetTypeByMetadataName(typeof(ObsoleteAttribute).FullName);

			if (attributes.Any(_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				_.ConstructorArguments.Any(_ => _.Value is bool error && error)))
			{
				diagnostics.Add(Diagnostic.Create(new DiagnosticDescriptor(
					CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title,
					string.Format(CultureInfo.CurrentCulture, CannotMockObsoleteTypeDescriptor.Message, this.Type.Name),
					DescriptorConstants.Usage, DiagnosticSeverity.Info, true,
					helpLinkUri: HelpUrlBuilder.Build(
						CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title)), this.Type.Locations[0]));
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