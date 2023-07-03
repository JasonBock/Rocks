using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

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
		var name = $"{@event.ContainingType.FlattenedName}.{@event.Name}";
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
				MockEventsBuilder.BuildImplementation(writer, @event);
			}
			else
			{
				MockEventsBuilder.BuildExplicitImplementation(writer, @event);
			}
		}

		writer.WriteLines(
			"""
			#pragma warning restore CS0067

			void global::Rocks.IRaiseEvents.Raise(string @fieldName, global::System.EventArgs @args)
			{
				var @thisType = this.GetType();
				var @eventDelegate = (global::System.MulticastDelegate)thisType.GetField(@fieldName, 
					global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic)!.GetValue(this)!;
				
				if (@eventDelegate is not null)
				{
					foreach (var @handler in @eventDelegate.GetInvocationList())
					{
						@handler.Method.Invoke(@handler.Target, new object[]{this, @args});
					}
				}
			}
			""");
	}
}