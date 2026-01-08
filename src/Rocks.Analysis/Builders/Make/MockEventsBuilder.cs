using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Analysis.Builders.Make;

internal static class MockEventsBuilder
{
	private static void BuildImplementation(IndentedTextWriter writer, EventModel @event)
	{
		var isOverride = @event.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var declareNullable = @event.Type.NullableAnnotation == NullableAnnotation.Annotated ? string.Empty : "?";
		var isPublic = @event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{@event.OverridingCodeValue} " : string.Empty;

		writer.WriteLine(
			$"{isPublic}{isOverride}event {@event.Type.FullyQualifiedName}{declareNullable} {@event.Name};");
	}

	private static void BuildExplicitImplementation(IndentedTextWriter writer, EventModel @event)
	{
		var eventType = @event.Type.FullyQualifiedName;
		var name = $"{@event.ContainingType.FullyQualifiedName}.{@event.Name}";
		var fieldName = $"{@event.ContainingType.FlattenedName}_{@event.Name}";

		writer.WriteLines(
			$$"""
			private {{eventType}}? {{fieldName}};
			event {{eventType}}? {{name}}
			{
				add => this.{{fieldName}} += value;
				remove => this.{{fieldName}} -= value;
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, ImmutableArray<EventModel> events)
	{
		writer.WriteLine("#pragma warning disable CS0067");

		foreach (var @event in events)
		{
			if (@event.AttributesDescription.Length > 0)
			{
				writer.WriteLine(@event.AttributesDescription);
			}

			if (@event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
			{
			BuildImplementation(writer, @event);
			}
			else
			{
			BuildExplicitImplementation(writer, @event);
			}
		}

		writer.WriteLine("#pragma warning restore CS0067");
	}
}