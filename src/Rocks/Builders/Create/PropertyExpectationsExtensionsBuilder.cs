using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsExtensionsBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Properties.Results.Length > 0)
		{
			writer.WriteLine();

			if (information.Properties.Results.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				PropertyExpectationsExtensionsBuilder.BuildProperties(writer, information);
				PropertyExpectationsExtensionsBuilder.BuildIndexers(writer, information);
			}

			if (information.Properties.Results.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				PropertyExpectationsExtensionsBuilder.BuildExplicitProperties(writer, information);
				PropertyExpectationsExtensionsBuilder.BuildExplicitIndexers(writer, information);
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
					PropertyAccessor.Get, typeGroup.Key.GetName());
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
					PropertyAccessor.Set, typeGroup.Key.GetName());
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
					PropertyAccessor.Get, typeGroup.Key.GetName());
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
					PropertyAccessor.Set, containingTypeName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, MockInformation information)
	{
		if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
		{
			writer.WriteLine($"internal static class PropertyGetterExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties.Results
				.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet ||
						_.Accessors == PropertyAccessor.GetAndInit)))
			{
				PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol, PropertyAccessor.Get);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		if (information.Properties.Results.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
		{
			writer.WriteLine($"internal static class PropertySetterExpectationsOf{information.TypeToMock!.FlattenedName}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var result in information.Properties.Results
				.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, information.ContainingAssemblyOfInvocationSymbol, PropertyAccessor.Set);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}