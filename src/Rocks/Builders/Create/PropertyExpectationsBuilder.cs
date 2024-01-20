using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsBuilder
{
	internal static IEnumerable<ExpectationMapping> Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		var propertyMappings = new List<ExpectationMapping>();

		if (mockType.Properties.Length > 0)
		{
			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
			{
				propertyMappings.AddRange(PropertyExpectationsBuilder.BuildProperties(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));
				propertyMappings.AddRange(PropertyExpectationsBuilder.BuildIndexers(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));
			}

			if (mockType.Properties.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
			{
				propertyMappings.AddRange(PropertyExpectationsBuilder.BuildExplicitProperties(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));
				propertyMappings.AddRange(PropertyExpectationsBuilder.BuildExplicitIndexers(writer, mockType, expectationsFullyQualifiedName, adornmentsFQNsPipeline));
			}
		}

		return propertyMappings;
	}

	private static IEnumerable<ExpectationMapping> BuildExplicitIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName, 
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			var explicitTypeName = $"ExplicitIndexerExpectationsFor{containingTypeName}";

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
					internal sealed class ExplicitIndexerGetterExpectationsFor{{containingTypeName}}
					{
						internal ExplicitIndexerGetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupGetters)
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result,
						PropertyAccessor.Get, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitIndexerGetterExpectationsFor{containingTypeName}", $"Getters"));
			}

			var typeGroupSetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.SetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupSetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class ExplicitIndexerSetterExpectationsFor{{containingTypeName}}
					{
						internal ExplicitIndexerSetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupSetters)
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result,
						PropertyAccessor.Set, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitIndexerSetterExpectationsFor{containingTypeName}", $"Setters"));
			}

			var typeGroupInitializers = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.InitCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupInitializers.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class ExplicitIndexerInitializersExpectationsFor{{containingTypeName}}
					{
						internal ExplicitIndexerInitializersExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupInitializers)
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result,
						PropertyAccessor.Init, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitIndexerInitializersExpectationsFor{containingTypeName}", $"Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal ExplicitIndexerExpectationsFor{containingTypeName}({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.ExplicitIndexerExpectationsFor{containingTypeName}", $"ExplicitIndexersFor{containingTypeName}");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName, 
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal sealed class IndexerExpectations");
			writer.WriteLine("{");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class IndexerGetterExpectations
					{
						internal IndexerGetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.GetCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result, PropertyAccessor.Get, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.IndexerExpectations.IndexerGetterExpectations", "Getters"));
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
					internal sealed class IndexerSetterExpectations
					{
						internal IndexerSetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
						_.SetCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result, PropertyAccessor.Set, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.IndexerExpectations.IndexerSetterExpectations", "Setters"));
			}

			if (mockType.Properties.Any(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class IndexerInitializerExpectations
					{
						internal IndexerInitializerExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => _.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.InitCanBeSeenByContainingAssembly))
				{
					IndexerExpectationsIndexerBuilder.Build(writer, result, PropertyAccessor.Init, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.IndexerExpectations.IndexerInitializerExpectations", "Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal IndexerExpectations({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.IndexerExpectations", "Indexers");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildExplicitProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		foreach (var typeGroup in mockType.Properties
			.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
			.GroupBy(_ => _.ContainingType))
		{
			var containingTypeName = typeGroup.Key.FlattenedName;
			var explicitTypeName = $"ExplicitPropertyExpectationsFor{containingTypeName}";

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
					internal sealed class ExplicitPropertyGetterExpectationsFor{{containingTypeName}}
					{
						internal ExplicitPropertyGetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupGetters)
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result,
						PropertyAccessor.Get, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitPropertyGetterExpectationsFor{containingTypeName}", $"Getters"));
			}

			var typeGroupSetters = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
				_.SetCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupSetters.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class ExplicitPropertySetterExpectationsFor{{containingTypeName}}
					{
						internal ExplicitPropertySetterExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupSetters)
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result,
						PropertyAccessor.Set, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitPropertySetterExpectationsFor{containingTypeName}", $"Setters"));
			}

			var typeGroupInitializers = typeGroup.Where(
				_ => (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				_.InitCanBeSeenByContainingAssembly).ToArray();

			if (typeGroupInitializers.Length > 0)
			{
				writer.WriteLines(
					$$"""
					internal sealed class ExplicitPropertyInitializersExpectationsFor{{containingTypeName}}
					{
						internal ExplicitPropertyInitializersExpectationsFor{{containingTypeName}}({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in typeGroupInitializers)
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result,
						PropertyAccessor.Init, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.{explicitTypeName}.ExplicitPropertyInitializersExpectationsFor{containingTypeName}", $"Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal ExplicitPropertyExpectationsFor{containingTypeName}({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.ExplicitPropertyExpectationsFor{containingTypeName}", $"ExplicitPropertiesFor{containingTypeName}");
		}
	}

	private static IEnumerable<ExpectationMapping> BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName, 
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
		{
			writer.WriteLine($"internal sealed class PropertyExpectations");
			writer.WriteLine("{");
			writer.Indent++;

			var propertyProperties = new List<ExpectationMapping>();

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.GetCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class PropertyGetterExpectations
					{
						internal PropertyGetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.GetCanBeSeenByContainingAssembly))
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result, PropertyAccessor.Get, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.PropertyExpectations.PropertyGetterExpectations", "Getters"));
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
					internal sealed class PropertySetterExpectations
					{
						internal PropertySetterExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet) &&
						_.SetCanBeSeenByContainingAssembly))
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result, PropertyAccessor.Set, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.PropertyExpectations.PropertySetterExpectations", "Setters"));
			}

			if (mockType.Properties.Any(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
				 (_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
				 _.InitCanBeSeenByContainingAssembly))
			{
				writer.WriteLines(
					$$"""
					internal sealed class PropertyInitializerExpectations
					{
						internal PropertyInitializerExpectations({{expectationsFullyQualifiedName}} expectations) =>
							this.Expectations = expectations;
						
					""");
				writer.Indent++;

				foreach (var result in mockType.Properties
					.Where(_ => !_.IsIndexer && _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
						(_.Accessors == PropertyAccessor.Init || _.Accessors == PropertyAccessor.GetAndInit) &&
						_.InitCanBeSeenByContainingAssembly))
				{
					PropertyExpectationsPropertyBuilder.Build(writer, result, PropertyAccessor.Init, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
				}

				writer.WriteLine($"private {expectationsFullyQualifiedName} Expectations {{ get; }}");
				writer.Indent--;
				writer.WriteLine("}");

				propertyProperties.Add(new(
					$"{expectationsFullyQualifiedName}.PropertyExpectations.PropertyInitializerExpectations", "Initializers"));
			}

			writer.WriteLine();

			// Generate the constructor and properties.
			writer.WriteLine($"internal PropertyExpectations({expectationsFullyQualifiedName} expectations) =>");
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

			yield return new($"{expectationsFullyQualifiedName}.PropertyExpectations", "Properties");
		}
	}
}