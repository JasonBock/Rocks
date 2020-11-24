using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders
{
	internal static class MockEventsBuilder
	{
		private static void BuildImplementation(IndentedTextWriter writer, EventMockableResult @event)
		{
			var isOverride = @event.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

			writer.WriteLine(
				$"public {isOverride}event {@event.Value.Type.GetName()}? {@event.Value.Name};");
		}

		private static void BuildExplicitImplementation(IndentedTextWriter writer, EventMockableResult @event)
		{
			var eventType = @event.Value.Type.GetName();
			var name = $"{@event.Value.ContainingType.GetName(GenericsOption.FlattenGenerics)}.{@event.Value.Name}";
			var fieldName = $"{@event.Value.ContainingType.GetName(GenericsOption.FlattenGenerics)}_{@event.Value.Name}";

			writer.WriteLine($"private {eventType}? {fieldName};");
			writer.WriteLine($"event {eventType}? {name}");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"add => this.{fieldName} += value;");
			writer.WriteLine($"remove => this.{fieldName} -= value;");
			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void Build(IndentedTextWriter writer, ImmutableArray<EventMockableResult> events)
		{
			writer.WriteLine("#pragma warning disable CS0067");
			
			foreach(var @event in events)
			{
				if(@event.MustBeImplemented == MustBeImplemented.Yes)
				{
					var attributes = @event.Value.GetAttributes();

					if (attributes.Length > 0)
					{
						writer.WriteLine(attributes.GetDescription());
					}

					if(@event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						MockEventsBuilder.BuildImplementation(writer, @event);
					}
					else
					{
						MockEventsBuilder.BuildExplicitImplementation(writer, @event);
					}

					writer.WriteLine();
				}
			}

			writer.WriteLine("#pragma warning restore CS0067");
			writer.WriteLine();

			writer.WriteLine("void IMockWithEvents.Raise(string fieldName, EventArgs args)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("var thisType = this.GetType();");
			writer.WriteLine("var eventDelegate = (MulticastDelegate)thisType.GetField(fieldName, ");
			writer.Indent++;
			writer.WriteLine("BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this)!;");
			writer.Indent--;
			writer.WriteLine();
			writer.WriteLine("if (eventDelegate is not null)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foreach (var handler in eventDelegate.GetInvocationList())");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("handler.Method.Invoke(handler.Target, new object[]{this, args});");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}