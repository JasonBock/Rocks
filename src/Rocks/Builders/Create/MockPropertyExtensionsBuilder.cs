using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockPropertyExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		if (mockType.Properties.Length > 0)
		{
			var typeToMockName = mockType.Type.FullyQualifiedName;
			MockPropertyExtensionsBuilder.BuildProperties(writer, mockType, typeToMockName);
			MockPropertyExtensionsBuilder.BuildIndexers(writer, mockType, typeToMockName);
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType, string typeToMockName)
	{
		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> Indexers(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerGetterExpectations<{{typeToMockName}}> Getters(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerSetterExpectations<{{typeToMockName}}> Setters(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.IndexerInitializerExpectations<{{typeToMockName}}> Initializers(this global::Rocks.Expectations.IndexerExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				_.IsIndexer)
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FullyQualifiedName;
			var flattenedContainingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> ExplicitIndexersFor{{flattenedContainingTypeName}}(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (typeGroup.Any(_ => _.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerGetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Getters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => _.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerSetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Setters(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => _.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitIndexerInitializerExpectations<{{typeToMockName}}, {{containingTypeName}}> Initializer(this global::Rocks.Expectations.ExplicitIndexerExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, string typeToMockName)
	{
		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> Properties(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertyGetterExpectations<{{typeToMockName}}> Getters(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertySetterExpectations<{{typeToMockName}}> Setters(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				_.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.PropertyInitializerExpectations<{{typeToMockName}}> Initializers(this global::Rocks.Expectations.PropertyExpectations<{{typeToMockName}}> @self) =>
						new(@self);

					""");
			}
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				!_.IsIndexer)
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FullyQualifiedName;
			var flattenedContainingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLines(
				$$"""
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> ExplicitPropertiesFor{{flattenedContainingTypeName}}(this global::Rocks.Expectations.Expectations<{{typeToMockName}}> @self) =>
					new(@self);

				""");

			if (typeGroup.Any(_ => _.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => _.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal static global::Rocks.Expectations.ExplicitPropertySetterExpectations<{{typeToMockName}}, {{containingTypeName}}> Setters(this global::Rocks.Expectations.ExplicitPropertyExpectations<{{typeToMockName}}, {{containingTypeName}}> @self) =>
						new(@self);

					""");
			}

			if (typeGroup.Any(_ => _.InitCanBeSeenByContainingAssembly))
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