using System;
using System.Collections.ObjectModel;

namespace Rocks.Templates
{
	internal static class EventTemplates
	{
		internal static string GetNonPublicEvent(string visibility, string eventType, string eventName) => $"{visibility} override event {eventType} {eventName};";

		internal static string GetEvent(string @override, string eventType, string eventName) => $"public {@override} event {eventType} {eventName};";

		internal static string GetEvents(ReadOnlyCollection<string> eventList) => 
$@"#pragma warning disable CS0067
		{string.Join(Environment.NewLine, eventList)}
#pragma warning restore CS0067";
	}
}
