using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsBuilderV4
{
	internal static IEnumerable<ExpectationMapping> Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		var propertyMappings = new List<ExpectationMapping>();

		if (mockType.Properties.Length > 0)
		{
			writer.WriteLines(
				$$"""
				internal sealed class {{mockType.Type.FlattenedName}}PropertiesExpectations
				{
				""");

			writer.Indent++;
			writer.WriteLine();

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				propertyMappings.AddRange(PropertyExpectationsBuilderV4.BuildProperties(writer, mockType, expectationsFullyQualifiedName));
				PropertyExpectationsBuilderV4.BuildIndexers(writer, mockType, expectationsFullyQualifiedName);
			}

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				PropertyExpectationsBuilderV4.BuildExplicitProperties(writer, mockType, expectationsFullyQualifiedName);
				PropertyExpectationsBuilderV4.BuildExplicitIndexers(writer, mockType, expectationsFullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		return propertyMappings;
	}

	private static void BuildExplicitIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
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
				ExplicitIndexerExpectationsIndexerBuilderV4.Build(writer, result,
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
				ExplicitIndexerExpectationsIndexerBuilderV4.Build(writer, result,
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
				ExplicitIndexerExpectationsIndexerBuilderV4.Build(writer, result,
					PropertyAccessor.Init, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
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
				IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Get);
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
				IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Set);
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
				IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Init);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildExplicitProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
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
				ExplicitPropertyExpectationsPropertyBuilderV4.Build(writer, result,
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
				ExplicitPropertyExpectationsPropertyBuilderV4.Build(writer, result,
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
				ExplicitPropertyExpectationsPropertyBuilderV4.Build(writer, result,
					PropertyAccessor.Init, typeGroup.Key.FullyQualifiedName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		var typeToMock = mockType.Type.FlattenedName;

		if (mockType.Properties.Any(_ => !_.IsIndexer))
		{
			writer.WriteLine($"internal sealed class {typeToMock}PropertyExpectations");
			writer.WriteLine("{");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}PropertyGetterExpectations
					{
						internal {{typeToMock}}PropertyGetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet ||
							_.Accessors == PropertyAccessor.GetAndInit)))
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result, PropertyAccessor.Get, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}PropertyExpectations.{typeToMock}PropertyGetterExpectations", "Getters"));
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}PropertySetterExpectations
					{
						internal {{typeToMock}}PropertyGetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result, PropertyAccessor.Set, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}PropertyExpectations.{typeToMock}PropertySetterExpectations", "Setters"));
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}PropertyInitializerExpectations
					{
						internal {{typeToMock}}PropertyInitializerExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit)))
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result, PropertyAccessor.Init, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}PropertyExpectations.{typeToMock}PropertyInitializerExpectations", "Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal {typeToMock}PropertyExpectations({expectationsFullyQualifiedName} expectations) =>");
			writer.Indent++;
			var thisExpectations = $"({string.Join(", ", propertyProperties.Select(_ => $"this.{_.PropertyName}"))})";
			var newExpectations = $"({string.Join(", ", propertyProperties.Select(_ => "new(expectations)"))})";
			writer.WriteLine($"{thisExpectations} = {newExpectations};");
			writer.Indent--;
			writer.WriteLine();

			foreach (var propertyProperty in propertyProperties)
			{
				writer.WriteLine($"internal {propertyProperty.PropertyExpectationTypeName} {propertyProperty.PropertyName} {{ get; }}");
			}

			writer.Indent--;
			writer.WriteLine("}");

			yield return new($"{expectationsFullyQualifiedName}.{typeToMock}PropertyExpectations", "Properties");
		}
	}
}