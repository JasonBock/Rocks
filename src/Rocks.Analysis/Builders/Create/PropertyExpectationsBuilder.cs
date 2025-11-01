using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class PropertyExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType,
		List<PropertyModel> properties,
		string expectationsFullyQualifiedName, string propertyExpectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		if (properties.Count > 0)
		{
			PropertyExpectationsBuilder.BuildProperties(writer, mockType, properties, expectationsFullyQualifiedName, propertyExpectationsFullyQualifiedName, adornmentsFQNsPipeline);
			PropertyExpectationsBuilder.BuildIndexers(writer, mockType, properties, expectationsFullyQualifiedName, propertyExpectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}

	private static void BuildIndexers(IndentedTextWriter writer, TypeMockModel mockType,
		List<PropertyModel> properties,
		string expectationsFullyQualifiedName, string propertyExpectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var index = 0;

		foreach (var property in properties.Where(property => property.IsIndexer))
		{
			var namingContext = new VariablesNamingContext(property.Parameters);

			writer.WriteLines(
				$$"""
				internal sealed class Indexer{{index}}Expectations
				{
					private readonly {{expectationsFullyQualifiedName}} @{{namingContext["parent"]}};
				""");

			writer.Indent++;

			var indexerArguments = new List<string>();

			foreach (var parameter in property.Parameters)
			{
				var argumentTypeName = ProjectionBuilder.BuildArgument(
					parameter.Type, new TypeArgumentsNamingContext(), parameter.RequiresNullableAnnotation);

				if (parameter.Type.IsPointer)
				{
					indexerArguments.Add($"{argumentTypeName} @{parameter.Name}");
				}
				else
				{
					var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;

					indexerArguments.Add($"{argumentTypeName} @{parameter.Name}");
				}

				writer.WriteLine($"private readonly {indexerArguments[indexerArguments.Count - 1]};");
			}

			writer.WriteLine();

			// Constructor
			writer.WriteLines(
				$$"""
				internal Indexer{{index}}Expectations({{expectationsFullyQualifiedName}} @{{namingContext["parent"]}}, {{string.Join(", ", indexerArguments)}})
				{
				""");

			writer.Indent++;

			foreach (var parameter in property.Parameters)
			{
				writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
			}

			writer.WriteLine($"this.@{namingContext["parent"]} = @{namingContext["parent"]};");

			foreach (var parameter in property.Parameters)
			{
				writer.WriteLine($"this.@{parameter.Name} = @{parameter.Name};");
			}

			writer.Indent--;
			writer.WriteLine("}");
			writer.WriteLine();

			// Gets and sets
			var constructorValues = IndexerExpectationsIndexerBuilder.Build(writer, property, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

			writer.Indent--;

			writer.WriteLines(
				$$"""
				}

				internal {{propertyExpectationsFullyQualifiedName}}.Indexer{{index}}Expectations this[{{string.Join(", ", indexerArguments)}}] => new(this.parent, {{string.Join(", ", property.Parameters.Select(parameter => $"@{parameter.Name}"))}});

				""");

			if (constructorValues is not null)
			{
				writer.WriteLines(
					$$"""
					internal {{propertyExpectationsFullyQualifiedName}}.Indexer{{index}}Expectations this[{{constructorValues.ConstructorParameters}}] => new(this.parent, {{constructorValues.ThisParameters}});

					""");
			}
			index++;
		}
	}

	private static void BuildProperties(IndentedTextWriter writer, TypeMockModel mockType,
		List<PropertyModel> properties,
		string expectationsFullyQualifiedName, string propertyExpectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		foreach (var property in properties.Where(property => !property.IsIndexer))
		{
			writer.WriteLines(
				$$"""
				internal sealed class {{property.Name}}PropertyExpectations
				{
					private readonly {{expectationsFullyQualifiedName}} parent;

					internal {{property.Name}}PropertyExpectations({{expectationsFullyQualifiedName}} parent) => 
						this.parent = parent;
				
				""");

			writer.Indent++;

			PropertyExpectationsPropertyBuilder.Build(writer, mockType, property, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

			writer.Indent--;
			writer.WriteLines(
				$$"""
				}

				internal {{propertyExpectationsFullyQualifiedName}}.{{property.Name}}PropertyExpectations {{property.Name}} => new(this.parent);

				""");
		}
	}
}