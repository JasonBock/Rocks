using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Rocks.Builders
{
	internal static class MockEventsBuilder
	{
		internal static void Build(IndentedTextWriter writer, ImmutableArray<EventMockableResult> events,
			SortedSet<string> usings)
		{
			writer.WriteLine("#pragma warning disable CS0067");
			foreach(var @event in events)
			{
				if(@event.MustBeImplemented == MustBeImplemented.Yes)
				{
					writer.WriteLine(
						$"public {(@event.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}event {@event.Value.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}? {@event.Value.Name};");
				}
			}
			writer.WriteLine("#pragma warning restore CS0067");

			writer.WriteLine();

			usings.Add($"using {typeof(EventArgs).Namespace};");
			usings.Add($"using {typeof(BindingFlags).Namespace};");

			writer.WriteLine("void IMockWithEvents.Raise(string eventName, EventArgs args)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("var thisType = this.GetType();");
			writer.WriteLine("var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, ");
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