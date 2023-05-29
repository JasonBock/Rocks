using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsExtensionsBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		if (mockType.Properties.Length > 0)
		{
			writer.WriteLine();

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				PropertyExpectationsExtensionsBuilderV3.BuildProperties(writer, mockType);
				PropertyExpectationsExtensionsBuilderV3.BuildIndexers(writer, mockType);
			}

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				PropertyExpectationsExtensionsBuilderV3.BuildExplicitProperties(writer, mockType);
				PropertyExpectationsExtensionsBuilderV3.BuildExplicitIndexers(writer, mockType);
			}
		}
	}

	private static void BuildExplicitIndexers(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
					_.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitIndexerGetterExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result,
					PropertyAccessor.Get, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitIndexerSetterExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result, 
					PropertyAccessor.Set, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitIndexerInitializerExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result, 
					PropertyAccessor.Init, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType)
	{
		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
				_.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class IndexerGetterExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet ||
					_.Accessors == PropertyAccessor.GetAndInit)))
			{
				IndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result, PropertyAccessor.Get);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
		{
			writer.WriteLine($"internal static class IndexerSetterExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				IndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result, PropertyAccessor.Set);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class IndexerInitializerExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				IndexerExpectationsExtensionsIndexerBuilderV3.Build(writer, result, PropertyAccessor.Init);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildExplicitProperties(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
					_.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitPropertyGetterExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, 
					PropertyAccessor.Get, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitPropertySetterExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, 
					PropertyAccessor.Set, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in mockType.Properties
			.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			writer.WriteLine($"internal static class ExplicitPropertyInitializerExpectationsOf{mockType.Type.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, 
					PropertyAccessor.Init, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, TypeMockModel mockType)
	{
		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class PropertyGetterExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet ||
						_.Accessors == PropertyAccessor.GetAndInit)))
			{
				PropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, PropertyAccessor.Get);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
		{
			writer.WriteLine($"internal static class PropertySetterExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				PropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, PropertyAccessor.Set);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class PropertyInitializerExpectationsOf{mockType.Type.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in mockType.Properties
				.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				PropertyExpectationsExtensionsPropertyBuilderV3.Build(writer, result, PropertyAccessor.Init);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}