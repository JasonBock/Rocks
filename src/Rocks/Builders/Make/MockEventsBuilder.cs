using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockEventsBuilder
{
	private static void BuildImplementation(IndentedTextWriter writer, EventMockableResult @event)
	{
		var isOverride = @event.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var declareNullable = @event.Value.Type.NullableAnnotation == NullableAnnotation.Annotated ? string.Empty : "?";
		var isPublic = @event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{@event.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;

		writer.WriteLine(
			$"{isPublic}{isOverride}event {@event.Value.Type.GetName()}{declareNullable} {@event.Value.Name};");
	}

	private static void BuildExplicitImplementation(IndentedTextWriter writer, EventMockableResult @event)
	{
		var eventType = @event.Value.Type.GetName();
		var name = $"{@event.Value.ContainingType.GetName(TypeNameOption.Flatten)}.{@event.Value.Name}";
		var fieldName = $"{@event.Value.ContainingType.GetName(TypeNameOption.Flatten)}_{@event.Value.Name}";

		writer.WriteLine($"private {eventType}? {fieldName};");
		writer.WriteLine($"event {eventType}? {name}");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"add => this.{fieldName} += value;");
		writer.WriteLine($"remove => this.{fieldName} -= value;");
		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, ImmutableArray<EventMockableResult> events,
		Compilation compilation)
	{
		writer.WriteLine("#pragma warning disable CS0067");

		foreach (var @event in events)
		{
			var attributes = @event.Value.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			if (@event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
			{
				MockEventsBuilder.BuildImplementation(writer, @event);
			}
			else
			{
				MockEventsBuilder.BuildExplicitImplementation(writer, @event);
			}
		}

		writer.WriteLine("#pragma warning restore CS0067");
	}
}