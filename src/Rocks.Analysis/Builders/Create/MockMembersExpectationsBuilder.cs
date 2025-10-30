using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockMembersExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		writer.WriteLines(
			$$"""
			private readonly {{expectationsFullyQualifiedName}}.SetupsExpectations setups;

			internal sealed class SetupsExpectations
			{
				private readonly {{expectationsFullyQualifiedName}} parent;

				internal SetupsExpectations({{expectationsFullyQualifiedName}} parent) =>
					this.parent = parent;

			""");
		writer.Indent++;

		// First, generate methods, properties and indexers that are not explicit.
		var methods = type.Methods.Where(
			method => method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
			.ToList();
		MethodExpectationsBuilder.Build(writer, type, methods, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

		var properties = type.Properties.Where(
			property => property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
			.ToList();
		PropertyExpectationsBuilder.Build(writer, type, properties, expectationsFullyQualifiedName, $"{expectationsFullyQualifiedName}.SetupsExpectations", adornmentsFQNsPipeline);

		// Then, get all of the type names that need explicit implementation.
		var explicitTypes = new HashSet<ITypeReferenceModel>(
			type.Methods.Where(method => method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes).Select(method => method.ContainingType)
				.Concat(type.Properties.Where(property => property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes).Select(properties => properties.ContainingType)));

		foreach (var explicitType in explicitTypes)
		{
			var explicitExpectationName = $$"""ExplicitFor{{explicitType.Name}}Expectations""";

			// TODO: How do we disambiguate between two interfaces that have the same name, even fully-qualified?
			// Or two interfaces that have the same name but different type parameter counts?
			writer.WriteLines(
				$$"""
				internal sealed class {{explicitExpectationName}}
				{
					private readonly {{expectationsFullyQualifiedName}} parent;

					internal {{explicitExpectationName}}({{expectationsFullyQualifiedName}} parent) =>
						this.parent = parent;

				""");
			writer.Indent++;

			// For each explicit implementation type, generate methods, properties, and indexers.
			var explicitMethods = type.Methods.Where(
				method => 
					method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					method.ContainingType == explicitType)
				.ToList();
			MethodExpectationsBuilder.Build(writer, type, explicitMethods, expectationsFullyQualifiedName, adornmentsFQNsPipeline);

			var explicitProperties = type.Properties.Where(
				property => 
					property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes &&
					property.ContainingType == explicitType)
				.ToList();
			PropertyExpectationsBuilder.Build(writer, type, explicitProperties, expectationsFullyQualifiedName, $"{expectationsFullyQualifiedName}.SetupsExpectations.{explicitExpectationName}", adornmentsFQNsPipeline);

			writer.Indent--;
			writer.WriteLines(
				$$"""
				}

				internal {{expectationsFullyQualifiedName}}.SetupsExpectations.{{explicitExpectationName}} ExplicitFor{{explicitType.Name}} { get => new(this.parent); }

				""");
		}

		writer.Indent--;
		writer.WriteLines(
			$$"""
			}

			internal {{expectationsFullyQualifiedName}}.SetupsExpectations Setups => this.setups;

			""");
	}
}