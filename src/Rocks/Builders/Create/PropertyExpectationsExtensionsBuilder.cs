﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class PropertyExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Properties.Length > 0)
			{
				writer.WriteLine();

				if (information.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					PropertyExpectationsExtensionsBuilder.BuildProperties(writer, information);
					PropertyExpectationsExtensionsBuilder.BuildIndexers(writer, information);
				}

				if (information.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
				{
					PropertyExpectationsExtensionsBuilder.BuildExplicitProperties(writer, information);
					PropertyExpectationsExtensionsBuilder.BuildExplicitIndexers(writer, information);
				}
			}
		}

		private static void BuildExplicitIndexers(IndentedTextWriter writer, MockInformation information)
		{
			foreach (var typeGroup in information.Properties
				.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
				writer.WriteLine($"internal static class {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}For{containingTypeName}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in typeGroup)
				{
					ExplicitIndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Get, typeGroup.Key.GetName());
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			foreach (var typeGroup in information.Properties
				.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
				writer.WriteLine($"internal static class {WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}For{containingTypeName}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in typeGroup)
				{
					ExplicitIndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Set, typeGroup.Key.GetName());
				}

				writer.Indent--;
				writer.WriteLine("}");
			}
		}

		private static void BuildIndexers(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static class {WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties
					.Where(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Get);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static class {WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties
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
			foreach (var typeGroup in information.Properties
				.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
				writer.WriteLine($"internal static class {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Getter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}For{containingTypeName}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in typeGroup)
				{
					ExplicitPropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Get, typeGroup.Key.GetName());
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			foreach (var typeGroup in information.Properties
				.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
				.GroupBy(_ => _.Value.ContainingType))
			{
				var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
				writer.WriteLine($"internal static class {WellKnownNames.Explicit}{WellKnownNames.Property}{WellKnownNames.Setter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}For{containingTypeName}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in typeGroup)
				{
					ExplicitPropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Set, containingTypeName);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}
		}

		private static void BuildProperties(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static class {WellKnownNames.Property}{WellKnownNames.Getter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties
					.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Get);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (information.Properties.Any(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLine($"internal static class {WellKnownNames.Property}{WellKnownNames.Setter}{WellKnownNames.Expectations}Of{information.TypeToMock.GetName(TypeNameOption.Flatten)}{WellKnownNames.Extensions}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var result in information.Properties
					.Where(_ => !_.Value.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Set);
				}

				writer.Indent--;
				writer.WriteLine("}");
			}
		}
	}
}