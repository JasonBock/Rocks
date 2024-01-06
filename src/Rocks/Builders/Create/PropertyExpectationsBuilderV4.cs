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
			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				propertyMappings.AddRange(PropertyExpectationsBuilderV4.BuildProperties(writer, mockType, expectationsFullyQualifiedName));
				propertyMappings.AddRange(PropertyExpectationsBuilderV4.BuildIndexers(writer, mockType, expectationsFullyQualifiedName));
			}

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				propertyMappings.AddRange(PropertyExpectationsBuilderV4.BuildExplicitProperties(writer, mockType, expectationsFullyQualifiedName));
				propertyMappings.AddRange(PropertyExpectationsBuilderV4.BuildExplicitIndexers(writer, mockType, expectationsFullyQualifiedName));
			}
		}

		return propertyMappings;
	}

	private static IEnumerable<ExpectationMapping> BuildExplicitIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
			.GroupBy(_ => _.ContainingType))
		{
			var typeToMock = mockType.Type.FlattenedName;
			var containingTypeName = typeGroup.Key.FlattenedName;
			var explicitTypeName = $"{typeToMock}ExplicitIndexerExpectationsFor{containingTypeName}";

			writer.WriteLines(
				$$"""
				internal sealed class {{explicitTypeName}}
				{
				""");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			var typeGroupGetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.GetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupGetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitIndexerGetterExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitIndexerGetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupGetters)
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result,
						PropertyAccessor.Get, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitIndexerGetterExpectationsFor{containingTypeName}", $"Getters"));
			}

			var typeGroupSetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.SetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupSetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitIndexerSetterExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitIndexerSetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupSetters)
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result,
						PropertyAccessor.Set, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitIndexerSetterExpectationsFor{containingTypeName}", $"Setters"));
			}

			var typeGroupInitializers = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.InitCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupInitializers.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitIndexerInitializersExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitIndexerInitializersExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupInitializers)
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result,
						PropertyAccessor.Init, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitIndexerInitializersExpectationsFor{containingTypeName}", $"Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal {typeToMock}ExplicitIndexerExpectationsFor{containingTypeName}({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.{typeToMock}ExplicitIndexerExpectationsFor{containingTypeName}", $"ExplicitIndexersFor{containingTypeName}");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		if (mockType.Properties.Any(_ => _.IsIndexer))
		{
			var typeToMock = mockType.Type.FlattenedName;

			writer.WriteLine($"internal sealed class {typeToMock}IndexerExpectations");
			writer.WriteLine("{");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}IndexerGetterExpectations
					{
						internal {{typeToMock}}IndexerGetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.GetCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Get, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}IndexerExpectations.{typeToMock}IndexerGetterExpectations", "Getters"));
			}

			if (propertyProperties.Count > 0)
			{
				writer.WriteLine();
			}

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				 _.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}IndexerSetterExpectations
					{
						internal {{typeToMock}}IndexerSetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
						_.SetCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Set, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}IndexerExpectations.{typeToMock}IndexerSetterExpectations", "Setters"));
			}

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}IndexerInitializerExpectations
					{
						internal {{typeToMock}}IndexerInitializerExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.InitCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilderV4.Build(writer, result, PropertyAccessor.Init, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}IndexerExpectations.{typeToMock}IndexerInitializerExpectations", "Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal {typeToMock}IndexerExpectations({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.{typeToMock}IndexerExpectations", "Indexers");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildExplicitProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
			.GroupBy(_ => _.ContainingType))
		{
			var typeToMock = mockType.Type.FlattenedName;
			var containingTypeName = typeGroup.Key.FlattenedName;
			var explicitTypeName = $"{typeToMock}ExplicitPropertyExpectationsFor{containingTypeName}";

			writer.WriteLines(
				$$"""
				internal sealed class {{explicitTypeName}}
				{
				""");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			var typeGroupGetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.GetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupGetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitPropertyGetterExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitPropertyGetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupGetters)
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result,
						PropertyAccessor.Get, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitPropertyGetterExpectationsFor{containingTypeName}", $"Getters"));
			}

			var typeGroupSetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.SetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupSetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitPropertySetterExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitPropertySetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupSetters)
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result,
						PropertyAccessor.Set, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitPropertySetterExpectationsFor{containingTypeName}", $"Setters"));
			}

			var typeGroupInitializers = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.InitCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupInitializers.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}ExplicitPropertyInitializersExpectationsFor{{containingTypeName}}
					{
						internal {{typeToMock}}ExplicitPropertyInitializersExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupInitializers)
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result,
						PropertyAccessor.Init, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.{typeToMock}ExplicitPropertyInitializersExpectationsFor{containingTypeName}", $"Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal {typeToMock}ExplicitPropertyExpectationsFor{containingTypeName}({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.{typeToMock}ExplicitPropertyExpectationsFor{containingTypeName}", $"ExplicitPropertiesFor{containingTypeName}");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		if (mockType.Properties.Any(_ => !_.IsIndexer))
		{
			var typeToMock = mockType.Type.FlattenedName;

			writer.WriteLine($"internal sealed class {typeToMock}PropertyExpectations");
			writer.WriteLine("{");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.GetCanBeSeenByContainingAssembly))
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
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.GetCanBeSeenByContainingAssembly))
				{
					PropertyExpectationsPropertyBuilderV4.Build(writer, result, PropertyAccessor.Get, expectationsFullyQualifiedName);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{typeToMock}PropertyExpectations.{typeToMock}PropertyGetterExpectations", "Getters"));
			}

			if (propertyProperties.Count > 0)
			{
				writer.WriteLine();
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				 _.SetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class {{typeToMock}}PropertySetterExpectations
					{
						internal {{typeToMock}}PropertySetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
						_.SetCanBeSeenByContainingAssembly))
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
				 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.InitCanBeSeenByContainingAssembly))
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
						(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.InitCanBeSeenByContainingAssembly))
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