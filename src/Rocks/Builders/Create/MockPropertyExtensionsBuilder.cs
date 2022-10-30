using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockPropertyExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Properties.Results.Length > 0)
		{
			var typeToMockName = information.TypeToMock!.ReferenceableName;
			MockPropertyExtensionsBuilder.BuildProperties(writer, information, typeToMockName);
			MockPropertyExtensionsBuilder.BuildIndexers(writer, information, typeToMockName);
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> Indexers(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.GetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerGetterExpectations<{{typeToMockName}}> Getters(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerSetterExpectations<{{typeToMockName}}> Setters(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerInitializerExpectations<{{typeToMockName}}> Initializers(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				_.Value.IsIndexer)
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetFullyQualifiedName();
			var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> ExplicitIndexersFor{{flattenedContainingTypeName}}(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.GetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerGetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Getters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) && 
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerSetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Setters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerInitializerExpectations<{{typeToMockName}}, {{containingTypeName}}> Initializer(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, MockInformation information, string typeToMockName)
	{
		if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> Properties(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.GetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertyGetterExpectations<{{typeToMockName}}> Getters(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertySetterExpectations<{{typeToMockName}}> Setters(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertyInitializerExpectations<{{typeToMockName}}> Initializers(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				!_.Value.IsIndexer)
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetFullyQualifiedName();
			var flattenedContainingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> ExplicitPropertiesFor{{flattenedContainingTypeName}}(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.GetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitPropertySetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Setters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.Value.SetMethod!.CanBeSeenByContainingAssembly(information.ContainingAssemblyOfInvocationSymbol)))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitPropertyInitializerExpectations<{{typeToMockName}}, {{containingTypeName}}> Initializers(this global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}
		}
	}
}