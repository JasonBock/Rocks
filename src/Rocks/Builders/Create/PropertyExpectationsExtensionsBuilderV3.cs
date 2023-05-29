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

	private static void BuildExplicitIndexers(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var typeGroup in information.Properties.Results
			.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
					_.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitIndexerGetterExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol,
					PropertyAccessor.Get, typeGroup.Key.GetFullyQualifiedName());
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitIndexerSetterExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol, 
					PropertyAccessor.Set, typeGroup.Key.GetFullyQualifiedName());
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitIndexerInitializerExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol,
					PropertyAccessor.Init, typeGroup.Key.GetFullyQualifiedName());
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
				_.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class IndexerGetterExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties.Results
				.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet ||
					_.Accessors == PropertyAccessor.GetAndInit)))
			{
				IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Get);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
		{
			writer.WriteLine($"internal static class IndexerSetterExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties.Results
				.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Set);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (information.Properties.Results.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class IndexerInitializerExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties.Results
				.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Init);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildExplicitProperties(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var typeGroup in information.Properties.Results
			.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || 
					_.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitPropertyGetterExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol, 
					PropertyAccessor.Get, typeGroup.Key.GetFullyQualifiedName());
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitPropertySetterExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol, 
					PropertyAccessor.Set, typeGroup.Key.GetFullyQualifiedName());
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		foreach (var typeGroup in information.Properties.Results
			.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
				(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit))
			.GroupBy(_ => _.Value.ContainingType))
		{
			var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
			writer.WriteLine($"internal static class ExplicitPropertyInitializerExpectationsOf{information.TypeToMock!.FlattenedName}For{containingTypeName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in typeGroup)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol,
					PropertyAccessor.Init, typeGroup.Key.GetFullyQualifiedName());
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